using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPSGame.Core;
using FPSGame.Factory;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FPSGame.Object
{
    public class Brick : Wall3D
    {
        private Vector2 index;
        private Wall3D[] walls;
        private String symbol;

        public Brick(Vector2 index, Vector3 pos, Vector3 normal, Vector3 up, float width, float height, Texture2D texture, String symbol)
            : base(pos, normal, up, width, height, texture)
        {
            this.index = index;
            this.walls = new Wall3D[4];
            this.symbol = symbol;
        }

        public String GetSymbol()
        {
            return symbol;
        }

        public override void Begin()
        {
            base.Begin();
            BuildWalls();
        }

        private bool CheckWestSide()
        {
            int x = (int)index.X;
            int y = (int)index.Y;

            if (x <= 0) return true;
            IMap map = MapLoader.GetInstance().GetMap();
            if (map == null) return true;

            IDisplayObject[][] objects = map.GetMatrix();
            if (objects[x - 1][y] == null) return true;
            return false;
        }

        private bool CheckWestSide0()
        {
            int x = (int)index.X;
            int y = (int)index.Y;

            IMap map = MapLoader.GetInstance().GetMap();
            if (map == null) return true;

            IDisplayObject[][] objects = map.GetMatrix();
            if (x <= 0 || x >= map.GetWidth() - 1) return false;

            Brick left = (Brick)objects[x - 1][y];
            Brick right = (Brick)objects[x + 1][y];
            if (left == null || right == null) return false;

            if (left.GetSymbol() == "1") return false;
            if (right.GetSymbol() == "1") return false;
            return true;
        }

        private bool CheckEastSide()
        {
            int x = (int)index.X;
            int y = (int)index.Y;

            IMap map = MapLoader.GetInstance().GetMap();
            if (map == null) return true;

            IDisplayObject[][] objects = map.GetMatrix();
            if (x >= map.GetWidth() - 1) return true;

            if (objects[x + 1][y] == null) return true;
            return false;
        }

        private bool CheckNorthSide()
        {
            int x = (int)index.X;
            int y = (int)index.Y;

            if (y == 0) return true;
            IMap map = MapLoader.GetInstance().GetMap();
            if (map == null) return true;

            IDisplayObject[][] objects = map.GetMatrix();
            if (objects[x][y - 1] == null) return true;
            return false;
        }

        private bool CheckNorthSide0()
        {
            int x = (int)index.X;
            int y = (int)index.Y;

            IMap map = MapLoader.GetInstance().GetMap();
            if (map == null) return true;

            IDisplayObject[][] objects = map.GetMatrix();
            if (y <= 0 || y >= map.GetHeight() - 1) return false;

            Brick top = (Brick)objects[x][y - 1];
            Brick bottom = (Brick)objects[x][y + 1];
            if (top == null || bottom == null) return false;

            if (top.GetSymbol() == "1") return false;
            if (bottom.GetSymbol() == "1") return false;
            return true;
        }

        private bool CheckSouthSide()
        {
            int x = (int)index.X;
            int y = (int)index.Y;

            IMap map = MapLoader.GetInstance().GetMap();
            if (map == null) return true;

            IDisplayObject[][] objects = map.GetMatrix();
            if (y >= map.GetHeight() - 1) return true;
            if (objects[x][y + 1] == null) return true;
            return false;
        }

        private Wall3D CreateWall(float x, float baseY, float z, Vector3 normal)
        {
            float wallHeight = width * 1.2f;
            return (Wall3D)TerrainFactory.CreateWall(x, baseY + wallHeight / 2, z, width, wallHeight, normal);
        }

        private void BuildWalls()
        {
            if (symbol == "*") return;
            Vector3 pos = GetPosition();
            if (walls[0] == null)
            {
                if (CheckWestSide())
                {
                    walls[0] = CreateWall(pos.X - width / 2, pos.Y, pos.Z, new Vector3(1, 0, 0));
                    walls[0].Begin();
                }
            }

            if (walls[1] == null)
            {
                if (CheckEastSide())
                {
                    walls[1] = CreateWall(pos.X + width / 2, pos.Y, pos.Z, new Vector3(-1, 0, 0));
                    walls[1].Begin();
                }
            }

            if (walls[2] == null)
            {
                if (CheckNorthSide())
                {
                    walls[2] = CreateWall(pos.X, pos.Y, pos.Z - height / 2, new Vector3(0, 0, 1));
                    walls[2].Begin();
                }
            }

            if (walls[3] == null)
            {
                if (CheckSouthSide())
                {
                    walls[3] = CreateWall(pos.X, pos.Y, pos.Z + height / 2, new Vector3(0, 0, -1));
                    walls[3].Begin();
                }
            }
        }

        public override bool CollideWith(IBoxShaped obj)
        {
            return false;
        }
    }
}
