using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FPSGame.Engine
{
    interface ICamera
    {
        public Matrix GetView();

        public void SetView(Matrix m);

        public Matrix GetProjection();

        public void SetProjection(Matrix m);
    }
}
