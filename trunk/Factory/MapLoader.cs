using System;
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
using FPSGame.Core.AI;

namespace FPSGame.Factory
{
    public class MapLoader
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

            map = new DefaultMap(matrixSize, name, des, missionDes, elemSize);

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
                                obj = TerrainFactory.CreateBrick(topleft.X + (i + 0.5f) * elemSize, topleft.Y, topleft.Z + (j + 0.5f) * elemSize, elemSize, index, data);
                                map.AddConstantObject(obj, i, j);
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

                    obj = TerrainFactory.CreateModel(mname, file, topleft.X + (x) * elemSize, topleft.Y, topleft.Z - (y) * elemSize, scale, new Vector3(3f, 0, -4), Vector3.Zero, 1000);
                    map.AddObject(obj);
                }
            }

            //obj = TerrainFactory.CreateModel(ResourceManager.TERRORIST_WEAPON, "colt-xm177", topleft.X + 14, topleft.Y, 50, 0.005f, new Vector3(0, 0, 0), 1);
            //map.AddObject(obj);

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
                    float rot = 0;
                    Vector3 patrolTo = Vector3.Zero;
                    String initstate = "idle";
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
                        else if (child.Name == "initial-rotation")
                        {
                            rot = float.Parse(child.InnerText);
                        }
                        else if (child.Name == "initial-state")
                        {
                            initstate = child.InnerText;
                        }
                        else if (child.Name == "patrol-to")
                        {
                            s = child.InnerText.Split(c);
                            patrolTo = new Vector3(topleft.X + (int.Parse(s[1]) + 0.5f) * elemSize, topleft.Y, topleft.Z + (int.Parse(s[0]) + 0.5f) * elemSize);
                        }
                    }

                    SkinnedModel m = FPSGame.GetInstance().LoadModel<SkinnedModel>(file, mname);
                    Model wpnModel = ResourceManager.GetResource<Model>(ResourceManager.TERRORIST_WEAPON);
                    IDictionary<String, Vector3> animFixRot = new Dictionary<String, Vector3>();
                    IDictionary<String, Vector3> animFixPos = new Dictionary<String, Vector3>();

                    animFixPos.Add("Idle", new Vector3(-0.3f, -0.1f, -0.2f));
                    animFixPos.Add("Shoot", Vector3.Zero);
                    animFixPos.Add("Run", Vector3.Zero);
                    animFixRot.Add("Idle", new Vector3((float)Math.PI*0.0f, -(float)Math.PI * 0.1f, -(float)Math.PI * 0.35f));
                    animFixRot.Add("Shoot", new Vector3(0, -(float)Math.PI * 0.11f, -(float)Math.PI * 0.38f));
                    animFixRot.Add("Run", new Vector3(0, -(float)Math.PI * 0.11f, -(float)Math.PI * 0.38f));
                    SimpleObject3D weapon = new SimpleObject3D(wpnModel, 0.005f, 1);
                    SimpleCharacter enemy = new SimpleCharacter(m, 0.35f, Vector3.Zero, animFixPos, animFixRot, null, 2.8f);
                    enemy.SetPosition(new Vector3(topleft.X + (y + 0.5f) * elemSize, topleft.Y, topleft.Z + (x + 0.5f) * elemSize));
                    enemy.SetRotation(new Vector3(0, (float)Math.PI/180*rot, 0));
                    enemy.AttachObject(weapon, "R_Hand2");

                    //init AI
                    IEnemyState initState = null;
                    switch(initstate)
                    {
                        case "idle":
                            initState = new IdleState(enemy);
                            break;
                        case "guard":
                            initState = new GuardState(enemy);
                            break;
                        case "patrol":
                            initState = new PatrolState(enemy, enemy.GetPosition(), patrolTo);
                            break;
                    }

                    enemy.InitAI(new DefEnemyAI(enemy), initState);
                    map.AddEnemy((SimpleCharacter)enemy);
                }
            }

            System.Console.Write("Map readed successfully!\n");
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
        private float elemSize;
        private Graph graph;

        public int GetWidth()
        {
            return width;
        }

        public int GetHeight()
        {
            return height;
        }

        public IDisplayObject[] GetFullMap()
        {
            //initialize full map with maximum value
            ArrayList full = new ArrayList();
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix.Length; j++)
                {
                    IDisplayObject obj = matrix[i][j];
                    if (obj != null)
                        full.Add(obj);
                }
            }
            foreach(IDisplayObject obj in objects)
            {
                full.Add(obj);
            }
            foreach (IDisplayObject obj in enemies)
            {
                full.Add(obj);
            }
            return (IDisplayObject[])full.ToArray(typeof(IDisplayObject));
        }

        public DefaultMap(Vector2 size, String name, String des, String missionDes, float elemSize)
        {
            this.name = name;
            this.des = des;
            this.missionDes = missionDes;
            this.width = (int)size.Y;
            this.height = (int)size.X;
            this.elemSize = elemSize;

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

        private void BuildGraph()
        {
            System.Console.Write("Building graph...\n");
            bool b = false;
            GraphVertex v = null;
            ArrayList vertices = new ArrayList();
            ArrayList edges = new ArrayList();

            //find the first vertex
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {   
                    IDisplayObject obj = matrix[i][j];
                    if (obj != null)
                    {
                        if (!b)
                        {
                            b = true;
                            v = new GraphVertex(new Vector2(i, j));
                            break;
                        }
                    }
                }
            }

            System.Console.Write("Found the first vertex...\n");

            if (!b) return;
            Queue scan = new Queue();
            Queue scanned = new Queue();

            scan.Enqueue(v);

            graph = new Graph(width, height);
            graph.AddVertex(v);

            System.Console.Write("Scanning edges...\n");
            while (scan.Count > 0)
            {
                GraphVertex gv = (GraphVertex)scan.Dequeue();
                scanned.Enqueue(gv);
                if (!vertices.Contains(gv))
                    vertices.Add(gv);
                Vector2 gvPos = gv.GetVertex();

                //find adjacent vertices
                
                //find north vertex
                AddVertex(vertices, edges, gv, new Vector2(gvPos.X, gvPos.Y - 1), scan, scanned);

                //find south vertex
                AddVertex(vertices, edges, gv, new Vector2(gvPos.X, gvPos.Y + 1), scan, scanned);

                //find west vertex
                AddVertex(vertices, edges, gv, new Vector2(gvPos.X - 1, gvPos.Y), scan, scanned);

                //find north vertex
                AddVertex(vertices, edges, gv, new Vector2(gvPos.X + 1, gvPos.Y), scan, scanned);
            }

            System.Console.Write("Graph builded...\n");
        }

        public Graph GetGraph()
        {
            return graph;
        }

        private void AddVertex(ArrayList vertices, ArrayList edges, GraphVertex gv, Vector2 vector, Queue scan, Queue scanned)
        {
            System.Console.Write("Checking " + vector.X + ", " + vector.Y + " for Vertex(" + gv.GetVertex().X + ", " + gv.GetVertex().Y + ")...");
            GraphVertex v2 = graph.FindVertex(vector);
            if (v2 == null)
            {
                System.Console.Write("Not found\n");
                v2 = new GraphVertex(vector);
            }
            else
                System.Console.Write("Found\n");
            if (IsAdjacent(gv, v2))
            {
                //add to the vertices list
                vertices.Add(v2);

                System.Console.Write("Adjacent found: Vertex(" + gv.GetVertex().X + ", " + gv.GetVertex().Y + ") and Vertex(" + v2.GetVertex().X + ", " + v2.GetVertex().Y + ")\n");

                if (!scan.Contains(v2) && !scanned.Contains(v2))
                {
                    scan.Enqueue(v2);
                    graph.AddVertex(v2);
                }

                //add new edge
                GraphEdge edge = gv.FindEdge(vector);
                if (edge == null)
                {
                    edge = new GraphEdge(gv, v2);
                    edges.Add(edges);
                    graph.AddEdge(edge);
                }
            }
        }

        private bool IsAdjacent(GraphVertex v1, GraphVertex v2)
        {
            Vector2 src = v1.GetVertex();
            Vector2 dst = v2.GetVertex();

            if (dst.X < 0 || dst.Y < 0) return false;
            if (dst.X > width - 1 || dst.Y > height - 1) return false;
            if (matrix[(int)dst.X][(int)dst.Y] == null) return false;
            return true;
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

        public float GetElemSize()
        {
            return elemSize;
        }

        public void OnLoad()
        {
            //build a graph on top of the current matrix
            BuildGraph();

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
