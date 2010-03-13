using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPSGame.Core;

namespace FPSGame.Sprite
{
    public interface IComponent : Drawable, IActionDispatcher
    {
        double GetX();

        double GetY();

        double GetWidth();

        double GetHeight();

        void SetX(double x);

        void SetY(double y);

        void Dispose();

        void SwitchTextureOn();

        void SwitchTextureOff();
    }
}
