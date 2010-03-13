using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPSGame.Sprite;

namespace FPSGame.Core
{
    public interface IActionEvent
    {
        IActionEvent CopyEventWithSource(IComponent source);

        IComponent GetSource();

        double GetX();

        double GetY();
    }
}
