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

        public static BoundingBox GetBoundingBox(Vector3 pos, Vector3 normal, Vector3 up, float width, float depth, float height)
        {
            Vector3 cross = Vector3.Cross(normal, up);
            Vector3[] vertices = new Vector3[8];
            float w = width;
            float d = depth;
            float h = height;

            vertices[0] = pos + normal * d / 2 + up * h / 2 + cross * w / 2;
            vertices[1] = pos + normal * d / 2 + up * h / 2 - cross * w / 2;
            vertices[2] = pos + normal * d / 2 - up * h / 2 + cross * w / 2;
            vertices[3] = pos + normal * d / 2 - up * h / 2 - cross * w / 2;
            vertices[4] = pos - normal * d / 2 + up * h / 2 + cross * w / 2;
            vertices[5] = pos - normal * d / 2 + up * h / 2 - cross * w / 2;
            vertices[6] = pos - normal * d / 2 - up * h / 2 + cross * w / 2;
            vertices[7] = pos - normal * d / 2 - up * h / 2 - cross * w / 2;
            BoundingBox box = BoundingBox.CreateFromPoints(vertices);
            return box;
        }

        public static float ClampFloat(float value, float min, float max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
    }
}
