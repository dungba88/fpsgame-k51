using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FPSGame.Core;

namespace FPSGame.Object.Simulation
{
    public interface IDisplayObject2D : IObject , Drawable
    {
        Vector2 GetPosition();

        void SetPosition(Vector2 pos);

        double GetRotation();

        void SetRotation(Vector2 rot);
    }
}
