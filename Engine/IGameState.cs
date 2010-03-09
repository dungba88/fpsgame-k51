using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPSGame.Core;

namespace FPSGame.Engine
{
    interface IGameState : IObject
    {
        void SetNextState(IGameState state);

        void SetPrevState(IGameState state);

        void GoNext();

        void GoBack();

        bool IsStarted();
    }
}
