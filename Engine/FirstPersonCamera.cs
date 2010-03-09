using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FPSGame.Engine
{
    class FirstPersonCamera : Camera
    {
        public FirstPersonCamera(Game game, Vector3 pos, Vector3 target, Vector3 up)
            : base(game, pos, target, up)
        {
        }

        public void Strafe(double speed)
        {
        }

        public void Yaw(double angle)
        {
        }

        public void Pitch(double angle)
        {
        }
    }
}
