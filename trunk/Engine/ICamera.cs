using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FPSGame.Core;

namespace FPSGame.Engine
{
    public interface ICamera : IObject, IGameComponent
    {
        Matrix GetView();

        void SetView(Matrix m);

        Matrix GetProjection();

        void SetProjection(Matrix m);

        Vector3 GetPosition();

        void SetPosition(Vector3 pos);

        Vector3 GetDirection();

        void SetDirection(Vector3 dir);

        Vector3 GetUpVector();

        void SetUpVector(Vector3 up);
    }
}
