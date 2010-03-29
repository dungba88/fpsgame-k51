using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPSGame.Object;
using Microsoft.Xna.Framework;
using System.Collections;

namespace FPSGame.Core.AI
{
    public class SimpleState : IEnemyState
    {
        private IEnemyState state;
        private SimpleCharacter enemy;
        private bool dead;
        private bool updated;
        private ArrayList plugins;

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
            String var = "Session_State_initPos" + GetCharacter().GetId();
            //try to load the initial position
            if (StateSessionStorage.IsVarRegistered(var))
            {
                initPos = StateSessionStorage.LoadVar<Vector3>(var);
            }
            else
            {
                initPos = GetCharacter().GetPosition();
            }
            //store the initial position
            StateSessionStorage.StoreVar(var, initPos);
            foreach (StatePlugin plg in plugins)
            {
                plg.Begin();
            }
        }

        public SimpleState(SimpleCharacter character)
        {
            enemy = character;
            plugins = new ArrayList();
        }

        public void AddPlugin(StatePlugin plg)
        {
            plugins.Add(plg);
            plg.Begin();
        }

        public void RemovePlugin(StatePlugin plg)
        {
            plg.End();
            plugins.Remove(plg);
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (StatePlugin plg in plugins)
            {
                plg.Update(gameTime);
            }
            updated = true;
        }

        public virtual void End()
        {
            dead = true;
            foreach (StatePlugin plg in plugins)
            {
                plg.End();
            }
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

        public virtual bool CanInterrupted()
        {
            return false;
        }
    }
}
