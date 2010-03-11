using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPSGame.Core;

namespace FPSGame.Engine.GameState
{
    public interface IGameState : IObject, Drawable
    {
        String GetName();

        void SetNextState(IGameState state);

        void SetPrevState(IGameState state);

        void GoNext();

        void GoBack();

        bool IsStarted();

        void StartOver();
    }
}
