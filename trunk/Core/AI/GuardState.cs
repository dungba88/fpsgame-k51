using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPSGame.Object;

namespace FPSGame.Core.AI
{
    public class GuardState : IdleState
    {
        public GuardState(SimpleCharacter character)
            : base(character)
        {
        }

        public override int GetStateNo()
        {
            return 1;
        }
    }
}
