using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FPSGame.Engine.GameEvent;

namespace FPSGame.Core.AI
{
    public interface IEnemyAI
    {
        void Begin();

        void Update(GameTime gameTime);

        void Notify(IGameEvent evt);

        void SetInitialState(IEnemyState state);
    }
}
