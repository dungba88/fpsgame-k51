using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FPSGame.Core
{
    public interface Drawable
    {
        void Draw(GameTime gameTime);

        void Draw3D(GameTime gameTime);
    }
}
