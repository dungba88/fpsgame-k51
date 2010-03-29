using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FPSGame.Core.AI
{
    public class StatePlugin : IObject
    {
        private bool dead;
        private IEnemyState state;

        public StatePlugin(IEnemyState state)
        {
            this.state = state;
        }

        public IEnemyState GetState()
        {
            return state;
        }

        public virtual void Begin()
        {
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void End()
        {
            dead = true;
        }

        public bool IsDead()
        {
            return dead;
        }
    }
}
