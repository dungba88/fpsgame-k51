using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FPSGame.Core
{
    interface IGameEvent
    {
        public IObject GetSource();

        public String GetEventData();
    }
}
