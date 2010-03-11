using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPSGame.Core;
using Microsoft.Xna.Framework;

namespace FPSGame.Engine.GameState
{
    public abstract class SimpleGameState : IGameState
    {
        private bool started;
        private bool ended;
        private IGameState prevState;
        private IGameState nextState;

        public void StartOver()
        {
            this.started = false;
            this.ended = false;
            Begin();
        }

        public bool IsStarted()
        {
            return started;
        }

        public bool IsDead()
        {
            return ended;
        }

        public SimpleGameState()
        {
            this.started = false;
            this.ended = false;
        }

        public void Begin()
        {
            if (!CheckNotStarted())
                return;

            started = true;
            OnBegin();
        }

        public void Update(GameTime gameTime)
        {
            if (!CheckRunning())
                return;

            OnUpdate(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            if (!CheckRunning())
                return;

            OnDraw(gameTime);
        }

        public void End()
        {
            if (!CheckRunning())
                return;

            started = false;
            ended = true;
            OnEnd();
        }

        public bool CheckNotStarted()
        {
            if (started)
            {
                Debugger.GetInstance().OutputFatalError("GameState [" + GetName() + "] already started");
                return false;
            }
            if (ended)
            {
                Debugger.GetInstance().OutputFatalError("GameState [" + GetName() + "] already ended");
                return false;
            }
            return true;
        }

        public bool CheckRunning()
        {
            if (!started)
            {
                Debugger.GetInstance().OutputFatalError("GameState [" + GetName() + "] not yet started");
                return false;
            }
            if (ended)
            {
                Debugger.GetInstance().OutputFatalError("GameState [" + GetName() + "] already ended");
                return false;
            }
            return true;
        }

        public void SetPrevState(IGameState state)
        {
            this.prevState = state;
        }

        public void SetNextState(IGameState state)
        {
            this.nextState = state;
        }

        public void GoBack()
        {
            this.prevState.SetNextState(this);
            FPSGame.GetInstance().SetGameState(prevState);
        }

        public void GoNext()
        {
            this.nextState.SetPrevState(this);
            FPSGame.GetInstance().SetGameState(nextState);
        }

        public abstract void OnBegin();

        public abstract void OnUpdate(GameTime gameTime);

        public abstract void OnDraw(GameTime gameTime);

        public abstract void OnEnd();

        public abstract String GetName();
    }
}
