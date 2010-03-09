using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FPSGame.Core
{
    interface IDisplayObject
    {
        public Vector3 getPosition();

        public void setPosition(Vector3 pos);

        public Vector3 getRotation();

        public void setRotation(Vector3 rot);

        public Matrix getWorld();

        public Matrix setWorld();
    }
}
