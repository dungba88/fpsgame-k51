using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FPSGame.Engine;
using FPSGame.Factory;

namespace FPSGame.Core
{
    public class MathUtils
    {
        public static Vector2 GetMatrixVector(Vector3 pos, float elemSize)
        {
            pos.X = (int)(pos.X / elemSize);
            pos.Z = (int)(pos.Z / elemSize);
            return new Vector2(pos.X, pos.Z);
        }

        public static Vector3 GetWorldVector(Vector2 pos, float elemSize)
        {
            pos.X = (int)((pos.X + 0.5f) * elemSize);
            pos.Y = (int)((pos.Y + 0.5f) * elemSize);
            return new Vector3(pos.X, 0, pos.Y);
        }

        public static bool Equal(Vector3 v1, Vector3 v2, float elemSize)
        {
            Vector2 v2_1 = GetMatrixVector(v1, elemSize);
            Vector2 v2_2 = GetMatrixVector(v2, elemSize);

            if (v2_1.X == v2_2.X && v2_1.Y == v2_2.Y)
                return true;
            return false;
        }

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

        public static int Random(int min, int max)
        {
            Random r = new Random();
            return r.Next(min, max);
        }

        public static float ClampFloat(float value, float min, float max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        public static float GetAngleBetweenPoint(Vector3 src, Vector3 dst)
        {
            return GetAngleFromVector(GetDiff(src, dst));
        }

        public static float GetAngleFromVector(Vector3 diff)
        {
            return (float)Math.Atan2(diff.Z, diff.X);
        }

        public static Vector3 GetDiff(Vector3 src, Vector3 dst)
        {
            return new Vector3(dst.Z - src.Z, 0, dst.X - src.X);
        }

        public static bool IsInRange(Vector3 src, Vector3 dst, Vector3 check)
        {
            float xmax = Math.Max(src.X, dst.X);
            float zmax = Math.Max(src.Z, dst.Z);
            float xmin = Math.Min(src.X, dst.X);
            float zmin = Math.Min(src.Z, dst.Z);
            if (check.X < xmin || check.X > xmax) return false;
            if (check.Z < zmin || check.Z > zmax) return false;
            return true;
        }

        public static float GetDistance(Vector3 src, Vector3 dst)
        {
            return (float)Math.Sqrt((dst.X - src.X) * (dst.X - src.X) + (dst.Z - src.Z) * (dst.Z - src.Z));
        }

        public static bool IsBound(float min, float max, float angle)
        {
            if (Math.Abs(min) > Math.PI * 2)
                min = min % ((float)Math.PI * 2);
            if (Math.Abs(max) > Math.PI * 2)
                max = max % ((float)Math.PI * 2);
            if (Math.Abs(angle) > Math.PI * 2)
                angle = angle % ((float)Math.PI * 2);

            //FPSGame.GetInstance().SetInfo(min + " " + angle + " " + max + " || ");
            if (angle >= min && angle <= max)
                return true;

            if (min < 0) min += (float)Math.PI * 2;
            if (max < 0) max += (float)Math.PI * 2;
            if (angle < 0) angle += (float)Math.PI * 2;

            if (angle >= min && angle <= max)
                return true;

            float minv = Math.Min(min, max);
            float maxv = Math.Max(min, max);

            //FPSGame.GetInstance().AppendInfo(minv + " " + angle + " " + maxv + " || ");

            if (angle < minv || angle > maxv)
                return false;
            return true;
        }
    }
}
