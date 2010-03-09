using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FPSGame.Core
{
    interface IDisplayObject : IObject
    {
        Vector3 getPosition();

        void setPosition(Vector3 pos);

        Vector3 getRotation();

        void setRotation(Vector3 rot);

        Matrix getWorld();

        Matrix setWorld();
    }
}
