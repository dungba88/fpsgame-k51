using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FPSGame.Core;

namespace FPSGame.Engine
{
    interface ICamera : IObject
    {
        Matrix GetView();

        void SetView(Matrix m);

        Matrix GetProjection();

        void SetProjection(Matrix m);
    }
}
