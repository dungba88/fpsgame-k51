using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FPSGame.Core
{
    interface IObserver
    {
        void Notify(IGameEvent evt);
    }
}
