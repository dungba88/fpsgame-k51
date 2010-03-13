using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPSGame.Sprite;

namespace FPSGame.Core
{
    public class ActionEvent : IActionEvent
    {
        private IComponent source;
        private double x;
        private double y;

        public IActionEvent CopyEventWithSource(IComponent source)
        {
            return new ActionEvent(source, x, y);
        }

        public ActionEvent(IComponent source, double x, double y)
        {
            this.source = source;
            this.x = x;
            this.y = y;
        }

        public IComponent GetSource()
        {
            return source;
        }

        public double GetX()
        {
            return x;
        }

        public double GetY()
        {
            return y;
        }
    }
}
