using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FPSGame.Core
{
    public class MathUtils
    {
        public static Matrix CreateTranslation(Vector3 pos)
        {
            return Matrix.CreateTranslation(pos);
        }

        public static Matrix CreateRotation(Vector3 rot)
        {
            return Matrix.CreateRotationX(rot.X)*Matrix.CreateRotationY(rot.Y)*Matrix.CreateRotationZ(rot.Z);
        }
    }
}
