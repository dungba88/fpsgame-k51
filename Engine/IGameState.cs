using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPSGame.Core;

namespace FPSGame.Engine
{
    interface IGameState : IObject
    {
        public void SetNextState(IGameState state);

        public void SetPrevState(IGameState state);

        public void GoNext();

        public void GoBack();

        public bool IsStarted();
    }
}
