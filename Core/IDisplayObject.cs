using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FPSGame.Core
{
    interface IDisplayObject : IObject, Drawable
    {
        Vector3 GetPosition();

        void SetPosition(Vector3 pos);

        Vector3 GetRotation();

        void SetRotation(Vector3 rot);

        Matrix GetWorld();

        void SetWorld(Matrix world);
    }
}
