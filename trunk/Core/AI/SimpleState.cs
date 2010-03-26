using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPSGame.Object;
using Microsoft.Xna.Framework;

namespace FPSGame.Core.AI
{
    public class SimpleState : IEnemyState
    {
        private IEnemyState state;
        private SimpleCharacter enemy;
        private bool dead;

        public SimpleCharacter GetCharacter()
        {
            return enemy;
        }

        public virtual int GetStateNo()
        {
            return 0;
        }

        public virtual void Begin()
        {
            dead = false;
        }

        public SimpleState(SimpleCharacter character)
        {
            enemy = character;
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

        public IEnemyState GetNextState()
        {
            return state;
        }

        public void SetNextState(IEnemyState state)
        {
            this.state = state;
        }
    }
}
