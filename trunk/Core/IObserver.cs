using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPSGame.Engine.GameEvent;

namespace FPSGame.Core
{
    interface IObserver
    {
        void Notify(IGameEvent evt);
    }
}
