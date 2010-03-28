using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPSGame.Core;

namespace FPSGame.Engine.GameEvent
{
    public class DefGameEvent : IGameEvent
    {
        private IObject src;
        private IObject target;
        private String eventData;
        private String actionData;
        private String eventName;
        private bool reqSrc;
        private bool reqTarget;

        public DefGameEvent(IObject src, IObject target, String eventName, String eventData, String actionData, bool reqSrc, bool reqTarget)
        {
            this.src = src;
            this.eventName = eventName;
            this.target = target;
            this.eventData = eventData;
            this.actionData = actionData;
            this.reqSrc = reqSrc;
            this.reqTarget = reqTarget;
        }

        public IObject GetSource()
        {
            return src;
        }

        public IObject GetTarget()
        {
            return target;
        }

        public String GetEventName()
        {
            return eventName;
        }

        public String GetEventData()
        {
            return eventData;
        }

        public String GetActionData()
        {
            return actionData;
        }

        public bool RequireSource()
        {
            return reqSrc;
        }

        public bool RequireTarget()
        {
            return reqTarget;
        }

        public bool MatchSource(IGameEvent evt)
        {
            //always return true if this game event does not require a source object
            if (!this.reqSrc) return true;

            if (src == evt.GetSource()) return true;
            return false;
        }

        public bool MatchTarget(IGameEvent evt)
        {
            //always return true if this game event does not require a target object
            if (!this.reqTarget) return true;

            if (target == evt.GetTarget()) return true;
            return false;
        }
    }
}
