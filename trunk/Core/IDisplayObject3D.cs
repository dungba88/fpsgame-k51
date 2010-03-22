using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FPSGame.Core
{
    public interface IDisplayObject3D : IDisplayObject
    {
        Vector3 GetPosition();

        void SetPosition(Vector3 pos);

        Vector3 GetRotation();

        void SetRotation(Vector3 rot);

        Matrix GetWorld();

        void SetWorld(Matrix world);
    }
}
