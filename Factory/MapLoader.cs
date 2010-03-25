﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FPSGame.Core;
using System.Collections;
using System.Xml;
using FPSGame.Object;
using FPSGame.Engine;
using Microsoft.Xna.Framework.Graphics;
using XNAnimation;

namespace FPSGame.Factory
{
    class MapLoader
    {
        private static MapLoader ldr = new MapLoader();
        private IMap map;
        public int objloaded;

        public static MapLoader GetInstance()
        {
            return ldr;
        }

        private MapLoader()
        {

        }

        public IMap GetMap()
        {
            return map;
        }

        public void BuildMap(String path)
        {
            String name;
            String des;
            String missionDes;
            Vector2 matrixSize;
            float elemSize;
            Vector3 topleft;
            Vector3 ppos;
            float angle;

            String[] s;
            XmlNodeList nodes;
            IEnumerator enums;

            ppos = Vector3.Zero;
            angle = 0.0f;

            objloaded = 0;
            //parse xml
            XmlDocument xml = new XmlDocument();
            xml.Load(path);
            xml.PreserveWhitespace = true;

            nodes = xml.GetElementsByTagName("name");
            name = nodes.Item(0).InnerText;

            nodes = xml.GetElementsByTagName("description");
            des = nodes.Item(0).InnerText;

            nodes = xml.GetElementsByTagName("mission-description");
            missionDes = nodes.Item(0).InnerText;

            char[] c = { ' ' };

            nodes = xml.GetElementsByTagName("matrix-size");
            s = nodes.Item(0).InnerText.Split(c);
            matrixSize = new Vector2(float.Parse(s[0]), float.Parse(s[1]));

            nodes = xml.GetElementsByTagName("elem-size");
            elemSize = float.Parse(nodes.Item(0).InnerText);

            nodes = xml.GetElementsByTagName("top-left");
            s = nodes.Item(0).InnerText.Split(c);
            topleft = new Vector3(float.Parse(s[0]), float.Parse(s[1]), float.Parse(s[2]));

            map = new DefaultMap(matrixSize, name, des, missionDes);

            nodes = xml.GetElementsByTagName("player");
            enums = nodes.Item(0).ChildNodes.GetEnumerator();
            while (enums.MoveNext())
            {
                XmlNode node = (XmlNode)enums.Current;
                if (node.Name == "position")
                {
                    s = node.InnerText.Split(c);
                    ppos = new Vector3(float.Parse(s[0]), float.Parse(s[1]), float.Parse(s[2]));
                }
                else if (node.Name == "angle-of-view")
                {
                    angle = float.Parse(node.InnerText);
                }
            }

            IDisplayObject3D obj;
            obj = TerrainFactory.CreateRoof(topleft.X + map.GetWidth() * elemSize / 2, topleft.Y + elemSize * 1.2f, topleft.Z + map.GetHeight() * elemSize / 2, elemSize * map.GetWidth(), elemSize * map.GetHeight());
            obj.Begin();

            nodes = xml.GetElementsByTagName("content");
            enums = nodes.Item(0).ChildNodes.GetEnumerator();
            int i=0;

            //load map terrain
            while (enums.MoveNext())
            {
                XmlNode node = (XmlNode)enums.Current;
                if (node.Name == "matrixRow")
                {
                    s = node.InnerText.Split(new char[] { ' ' });
                    //parse content
                    Vector2 index;
                    for (int j = 0; j < s.Length; j++)
                    {
                        String data = s[j];
                        switch (data)
                        {
                            case "*":
                                break;
                            case "0":
                                index = new Vector2(i, j);
                                //obj = TerrainFactory.CreateBrick(topleft.X + (i + 0.5f) * elemSize, topleft.Y, topleft.Z + (j + 0.5f) * elemSize, elemSize, index, data);
                                //map.AddConstantObject(obj, i, j);
                                break;
                            default:
                                break;
                        }
                    }
                    i++;
                }
            }

            //load object
            nodes = xml.GetElementsByTagName("models");
            enums = nodes.Item(0).ChildNodes.GetEnumerator();
            i = 0;
            while (enums.MoveNext())
            {
                XmlNode node = (XmlNode)enums.Current;
                if (node.Name == "model")
                {
                    String mname = "";
                    String file = "";
                    int x = 0;
                    int y = 0;
                    float scale = 1;
                    IEnumerator enum1 = node.ChildNodes.GetEnumerator();
                    while (enum1.MoveNext())
                    {
                        XmlNode child = (XmlNode)enum1.Current;
                        if (child.Name == "name")
                            mname = child.InnerText;
                        else if (child.Name == "file-name")
                        {
                            file = child.InnerText;
                        }
                        else if (child.Name == "position")
                        {
                            s = child.InnerText.Split(c);
                            x = int.Parse(s[0]);
                            y = int.Parse(s[1]);
                        }
                        else if (child.Name == "scale")
                        {
                            scale = float.Parse(child.InnerText);
                        }
                    }

                    obj = TerrainFactory.CreateModel(mname, file, topleft.X + (x) * elemSize, topleft.Y, topleft.Z - (y) * elemSize, scale, new Vector3(3f, 0, -4), 1000);
                    map.AddObject(obj);
                }
            }

            //load enemy
            nodes = xml.GetElementsByTagName("enemies");
            enums = nodes.Item(0).ChildNodes.GetEnumerator();
            i = 0;
            while (enums.MoveNext())
            {
                XmlNode node = (XmlNode)enums.Current;
                if (node.Name == "enemy")
                {
                    String mname = "";
                    String file = "";
                    int x = 0;
                    int y = 0;
                    IEnumerator enum1 = node.ChildNodes.GetEnumerator();
                    while (enum1.MoveNext())
                    {
                        XmlNode child = (XmlNode)enum1.Current;
                        if (child.Name == "name")
                            mname = child.InnerText;
                        else if (child.Name == "file-name")
                        {
                            file = child.InnerText;
                        }
                        else if (child.Name == "position")
                        {
                            s = child.InnerText.Split(c);
                            x = int.Parse(s[0]);
                            y = int.Parse(s[1]);
                        }
                    }

                    SkinnedModel m = FPSGame.GetInstance().LoadModel<SkinnedModel>(file, mname);
                    obj = new SimpleCharacter(m, 0.35f, Vector3.Zero, null, 2.8f);
                    obj.SetPosition(new Vector3(topleft.X + (y + 0.5f) * elemSize, topleft.Y, topleft.Z + (x + 0.5f) * elemSize));
                    map.AddEnemy((SimpleCharacter)obj);
                }
            }

            map.OnLoad();
        }

        public void UnloadMap()
        {
            map.OnUnload();
            map = null;
        }
    }

    class DefaultMap : IMap
    {
        private IDisplayObject[][] matrix;
        private ArrayList objects;
        private String name;
        private String des;
        private String missionDes;
        private int width;
        private int height;
        private ArrayList enemies;

        public int GetWidth()
        {
            return width;
        }

        public int GetHeight()
        {
            return height;
        }

        public DefaultMap(Vector2 size, String name, String des, String missionDes)
        {
            this.name = name;
            this.des = des;
            this.missionDes = missionDes;
            this.width = (int)size.Y;
            this.height = (int)size.X;

            matrix=new IDisplayObject[width][];
            for (int i = 0; i < matrix.Length;i++ )
            {
                matrix[i] = new IDisplayObject[height];
            }

            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    matrix[i][j] = null;
                }
            }
            objects = new ArrayList();
            enemies = new ArrayList();
        }

        public String GetName()
        {
            return name;
        }

        public String GetDescription()
        {
            return des;
        }

        public String GetMissionDescription()
        {
            return missionDes;
        }

        public void OnLoad()
        {
            for (int i = 0; i < matrix.Length;i++ )
            {
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    IDisplayObject obj = matrix[i][j];
                    if (obj == null) continue;
                    obj.Begin();
                }
            }

            foreach (IDisplayObject obj in objects)
            {
                if (obj == null) continue;
                obj.Begin();
            }

            foreach (IDisplayObject obj in enemies)
            {
                if (obj == null) continue;
                obj.Begin();
            }
        }

        public SimpleCharacter[] GetEnemies()
        {
            return (SimpleCharacter[])enemies.ToArray(typeof(SimpleCharacter));
        }

        public void AddEnemy(SimpleCharacter enemy)
        {
            enemies.Add(enemy);
        }

        public IDisplayObject[][] GetMatrix()
        {
            return matrix;
        }

        public void AddConstantObject(IDisplayObject obj, int x, int y)
        {
            matrix[x][y] = obj;
        }

        public IDisplayObject[] GetObjects()
        {
            return (IDisplayObject[])objects.ToArray(typeof(IDisplayObject));
        }

        public void AddObject(IDisplayObject obj)
        {
            objects.Add(obj);
        }

        public void RemoveObject(IDisplayObject obj)
        {
            objects.Remove(obj);
        }

        public void OnUnload()
        {
            for (int i = 0; i < matrix.Length;i++ )
            {
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    IDisplayObject obj = matrix[i][j];
                    if (obj == null) continue;
                    obj.End();
                }
            }
            matrix = null;

            foreach (IDisplayObject obj in objects)
            {
                if (obj == null) continue;
                obj.End();
            }
            objects = null;

            foreach (IDisplayObject obj in enemies)
            {
                if (obj == null) continue;
                obj.End();
            }
            enemies = null;
        }
    }
}
