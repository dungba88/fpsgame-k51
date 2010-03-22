﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPSGame.Core;
using FPSGame.Object;
using Microsoft.Xna.Framework;
using FPSGame.Engine;
using Microsoft.Xna.Framework.Graphics;

namespace FPSGame.Factory
{
    public class TerrainFactory
    {
        public static IDisplayObject CreateRoof(float x, float y, float z, float width, float height)
        {
            Vector3 pos = new Vector3(x, y, z);
            return new Brick(Vector2.Zero, pos, Vector3.Down, Vector3.Backward, width, height, ResourceManager.GetResource<Texture2D>(ResourceManager.CEILING_TEXTURE), "*");
        }

        public static IDisplayObject CreateBrick(float x, float y, float z, float size, Vector2 index, String symbol)
        {
            Vector3 pos = new Vector3(x, y, z);
            //System.Windows.Forms.MessageBox.Show(x + "/" + y + "/" + z);
            return new Brick(index, pos, Vector3.Up, Vector3.Backward, size, size, ResourceManager.GetResource<Texture2D>(ResourceManager.FLOOR_TEXTURE), symbol);
        }

        public static IDisplayObject CreateWall(float x, float y, float z, float width, float height, Vector3 normal)
        {
            Vector3 pos = new Vector3(x, y, z);
            //new Wall3D(pos, Vector3.Negate(normal), Vector3.Up, width, height, ResourceManager.GetResource<Texture2D>(ResourceManager.WALL_TEXTURE)).Begin();
            return new Wall3D(pos, normal, Vector3.Up, width, height, ResourceManager.GetResource<Texture2D>(ResourceManager.WALL_TEXTURE));
        }

        public static IDisplayObject CreateWall(float x, float y, float z, float width, float height, Vector3 normal, Texture2D texture)
        {
            Vector3 pos = new Vector3(x, y, z);
            //new Wall3D(pos, Vector3.Negate(normal), Vector3.Up, width, height, ResourceManager.GetResource<Texture2D>(ResourceManager.WALL_TEXTURE)).Begin();
            return new Wall3D(pos, normal, Vector3.Up, width, height, texture);
        }

        public static IDisplayObject CreateBrickWithBars(float x, float y, float z, float size, Vector2 index, String symbol)
        {
            Vector3 pos = new Vector3(x, y, z);
            return new BarBrick(index, pos, Vector3.Up, Vector3.Backward, size, size, ResourceManager.GetResource<Texture2D>(ResourceManager.FLOOR_TEXTURE), symbol);
        }

        public static IDisplayObject CreateDoor(float x, float y, float z, float size)
        {
            return null;
        }
    }
}
