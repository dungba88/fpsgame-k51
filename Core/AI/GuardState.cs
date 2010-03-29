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
        private float initRot;

        public GuardState(SimpleCharacter character)
            : base(character)
        {
        }

        public override void Begin()
        {
            int id = GetCharacter().GetId();
            //try to find previously saved session
            if (StateSessionStorage.IsVarRegistered("Session_Guard_initRot" + id))
                initRot = StateSessionStorage.LoadVar<float>("Session_Guard_initRot" + id);
            else
                initRot = GetCharacter().GetRotation().Y;
            GetCharacter().Guard(true);
            AddPlugin(new RotatePlugin(this, initRot));

            base.Begin();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
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
