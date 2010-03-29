using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPSGame.Engine.GameEvent;
using FPSGame.Core;

namespace FPSGame.Engine
{
    public class GameEventGenerator
    {
        public const String EVENT_PLAYER_NOT_SPOTTED = "OUT_playernotspotted";

        public const String EVENT_SPOT_PLAYER = "OUT_spotplayer";

        public const String EVENT_PLAYER_SHOOT = "OUT_playershoot";

        public const String EVENT_PLAYER_MOVE = "OUT_playermove";

        public const String EVENT_PLAYER_HIT = "OUT_playerhit";

        public const String EVENT_RESET_STATE = "IN_resetstate";

        public static void GenerateEvent(IObject src, IObject target, String eventName, String eventData, String actionData, bool reqSrc, bool reqTarget)
        {
            if (FPSGame.INVISIBLE_MODE && eventName.StartsWith("OUT")) return;
            DefGameEvent evt = new DefGameEvent(src, target, eventName, eventData, actionData, reqSrc, reqTarget);
            ObjectManager.GetInstance().NotifyAllObservers(evt);
        }
    }
}
