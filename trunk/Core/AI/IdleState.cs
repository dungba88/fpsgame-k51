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
        public const float MAX_DELAY = 10000;   //10000ms or 10s

        private float delay;

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

            base.Update(gameTime);
        }

        public override void End()
        {
            base.End();
        }
    }
}
