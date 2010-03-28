using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FPSGame.Core.AI
{
    public interface IEnemyState : IObject
    {
        IEnemyState GetNextState();

        void SetNextState(IEnemyState state);

        int GetStateNo();

        bool IsUpdated();

        Vector3 GetInitialPosition();
    }
}
