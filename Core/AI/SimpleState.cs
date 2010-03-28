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
        private bool updated;

        private Vector3 initPos;

        public Vector3 GetInitialPosition()
        {
            return initPos;
        }

        public bool IsUpdated()
        {
            return updated;
        }

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
            updated = false;
            //try to load the initial position
            if (StateSessionStorage.IsVarRegistered("Session_State_initPos" + GetCharacter().GetId()))
                initPos = StateSessionStorage.LoadVar<Vector3>("Session_State_initPos" + GetCharacter().GetId());
            else
                initPos = GetCharacter().GetPosition();
        }

        public SimpleState(SimpleCharacter character)
        {
            enemy = character;
        }

        public virtual void Update(GameTime gameTime)
        {
            updated = true;
        }

        public virtual void End()
        {
            //store the initial position
            StateSessionStorage.StoreVar("Session_State_initPos" + GetCharacter().GetId(), initPos);
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
