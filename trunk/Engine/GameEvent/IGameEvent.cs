using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPSGame.Core;

namespace FPSGame.Engine.GameEvent
{
    public interface IGameEvent
    {
        IObject GetSource();

        IObject GetTarget();

        String GetEventName();

        String GetEventData();

        String GetActionData();

        bool RequireSource();

        bool RequireTarget();

        bool MatchSource(IGameEvent evt);

        bool MatchTarget(IGameEvent evt);
    }
}