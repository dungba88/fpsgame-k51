using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FPSGame.Engine;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Forms;

namespace FPSGame.Object
{
    public class BarBrick : Brick
    {
        public BarBrick(Vector2 index, Vector3 pos, Vector3 normal, Vector3 up, float width, float height, Texture2D texture, String symbol)
            : base(index, pos, normal, up, width, height, texture, symbol)
        {

        }

        public override void Draw3D(GameTime gameTime)
        {
            base.Draw3D(gameTime);
        }

        protected override void BuildWalls()
        {
            base.BuildWalls();
            for (int i = 0; i < 4; i++)
            {
                Wall3D wall = GetWalls()[i];
                if (wall == null)
                {
                    CreateDoubleSideWall(i, ResourceManager.GetResource<Texture2D>(ResourceManager.JAIL_BARS));
                    return;
                }
            }
        }
    }
}
