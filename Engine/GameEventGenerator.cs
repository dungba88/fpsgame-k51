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
        public const String EVENT_PLAYER_NOT_SPOTTED = "playernotspotted";

        public const String EVENT_SPOT_PLAYER = "spotplayer";

        public const String EVENT_PLAYER_SHOOT = "playershoot";

        public const String EVENT_PLAYER_MOVE = "playermove";

        public static void GenerateEvent(IObject src, IObject target, String eventData, String actionData, bool reqSrc, bool reqTarget)
        {
            DefGameEvent evt = new DefGameEvent(src, target, eventData, actionData, reqSrc, reqTarget);
            ObjectManager.GetInstance().NotifyAllObservers(evt);
        }
    }
}
