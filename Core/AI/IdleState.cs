using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FPSGame.Object;

namespace FPSGame.Core.AI
{
    public class IdleState : SimpleState
    {
        public override int GetStateNo()
        {
            return 1;
        }

        public IdleState(SimpleCharacter character)
            : base(character)
        {
            
        }

        public override void Begin()
        {
            base.Begin();
        }

        public override void Update(GameTime gameTime)
        {
            GetCharacter().Idle();
        }

        public override void End()
        {
            base.End();
        }
    }
}
