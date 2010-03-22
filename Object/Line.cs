using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections;
using FPSGame.Core;

namespace FPSGame.Object
{
    public class Line
    {
        Texture2D pixel;

        /// <summary>
        /// Gets/sets the position of the primitive line object.
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// Creates a new primitive line object.
        /// </summary>
        /// <param name="graphicsDevice">The Graphics Device object to use.</param>
        public Line(Color[] pixelCols)
        {
            GraphicsDevice graphDevice = FPSGame.GetInstance().GraphicsDevice;
            // create pixels
            pixel = new Texture2D(graphDevice, 1, 1, 1, TextureUsage.None, SurfaceFormat.Color);
            pixel.SetData<Color>(pixelCols);

            Position = new Vector2(0, 0);
        }

        /// <summary>
        /// Renders the primtive line object.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to use to render the primitive line object.</param>
        public void Render(Vector2 src, Vector2 dst, Color col, float Depth)
        {
                Vector2 vector1 = src;
                Vector2 vector2 = dst;

                // calculate the distance between the two vectors
                float distance = Vector2.Distance(vector1, vector2);

                // calculate the angle between the two vectors
                float angle = (float)Math.Atan2((double)(vector2.Y - vector1.Y),
                    (double)(vector2.X - vector1.X));

                SpriteBatch spriteBatch = FPSGame.GetInstance().GetSpriteBatch();
                // stretch the pixel between the two vectors
                spriteBatch.Draw(pixel,
                    Position + vector1,
                    null,
                    col,
                    angle,
                    Vector2.Zero,
                    new Vector2(distance, 1),
                    SpriteEffects.None,
                    Depth);
        }
    }
}
