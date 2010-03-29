using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPSGame.Object;
using Microsoft.Xna.Framework;
using FPSGame.Engine;

namespace FPSGame.Core.AI
{
    public class FollowState : RunState
    {
        public const int MAX_WAIT = 500;

        private StatePlugin plugin;
        private int wait = 0;
        private int count = 0;

        public FollowState(SimpleCharacter character, Vector3 source, Vector3 target)
            : base(character, source, target)
        {
        }

        public override void StoreSession(string var)
        {
            //do nothing here
        }

        public override void OnDestinationReached()
        {
            wait++;
            if (wait >= MAX_WAIT)
            {
                Vector3 src = StateSessionStorage.LoadVar<Vector3>("Session_State_initPos"+GetCharacter().GetId());
                Vector3 dst = GetTarget();
                if (src == dst)
                {
                    GameEventGenerator.GenerateEvent(GetCharacter(), default(IObject), GameEventGenerator.EVENT_RESET_STATE, "", "", true, false);
                    return;
                }
                SetSource(dst);
                SetTarget(src);
                base.Begin();
                destReached = false;
                return;
            }
            float f = MathUtils.Random(0, 360) * (float)Math.PI / 180;
            plugin = new RotatePlugin(this, f);
            if (plugin != null)
                AddPlugin(plugin);
            GetCharacter().Idle();
            base.OnDestinationReached();
        }

        public override bool CanInterrupted()
        {
            return false;
        }
    }
}
