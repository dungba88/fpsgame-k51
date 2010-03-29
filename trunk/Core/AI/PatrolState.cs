using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPSGame.Object;
using Microsoft.Xna.Framework;
using FPSGame.Engine;

namespace FPSGame.Core.AI
{
    public class PatrolState : RunState
    {
        public const int MAX_WAIT = 500;
        private int wait = 0;
        private StatePlugin plugin;

        public PatrolState(SimpleCharacter character, Vector3 source, Vector3 target)
            : base(character, source, target)
        {
        }

        public override void Begin()
        {
            String svar = "Session_Patrol_Source" + GetCharacter().GetId();
            String tvar = "Session_Patrol_Target" + GetCharacter().GetId();
            //try to load target
            if (StateSessionStorage.IsVarRegistered(svar))
                this.SetSource(StateSessionStorage.LoadVar<Vector3>(svar));
            if (StateSessionStorage.IsVarRegistered(tvar))
                this.SetTarget(StateSessionStorage.LoadVar<Vector3>(tvar));

            base.Begin();
        }

        public override void OnDestinationReached()
        {
            wait++;
            if (wait >= MAX_WAIT)
            {
                wait = 0;
                Vector3 src = GetSource();
                Vector3 dst = GetTarget();
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

        public override void End()
        {
            //save the target
            StateSessionStorage.StoreVar("Session_Patrol_Source" + GetCharacter().GetId(), GetSource());
            StateSessionStorage.StoreVar("Session_Patrol_Target" + GetCharacter().GetId(), GetTarget());
        }
    }
}
