using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPSGame.Object;
using Microsoft.Xna.Framework;

namespace FPSGame.Core.AI
{
    public class GuardState : IdleState
    {
        public const float MAX_DELAY = 10000;   //10000ms or 10s

        private float delay;

        private float initRot;

        public GuardState(SimpleCharacter character)
            : base(character)
        {
        }

        public override void Begin()
        {
            delay = 0;
            int id = GetCharacter().GetId();
            //try to find previously saved session
            if (StateSessionStorage.IsVarRegistered("Session_Guard_initRot" + id))
                initRot = StateSessionStorage.LoadVar<float>("Session_Guard_initRot" + id);
            else
                initRot = GetCharacter().GetRotation().Y;
            GetCharacter().Guard(true);
            base.Begin();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            
            //rotate this unit to a random rotation
            delay += gameTime.ElapsedGameTime.Milliseconds;
            if (delay >= MAX_DELAY)
            {
                int r = (MathUtils.Random(0, +60) - 30);
                float f = r * (float)Math.PI / 180 + initRot;
                Vector3 rot = GetCharacter().GetRotation();
                rot.Y = f;
                FPSGame.GetInstance().SetInfo(rot.Y * 180 / Math.PI + "");
                GetCharacter().SetRotation(rot);
                delay = 0;
            }
        }

        public override void End()
        {
            //store the session
            int id = GetCharacter().GetId();
            StateSessionStorage.StoreVar("Session_Guard_initRot" + id, initRot);

            base.End();
        }

        public override int GetStateNo()
        {
            return 1;
        }
    }
}
