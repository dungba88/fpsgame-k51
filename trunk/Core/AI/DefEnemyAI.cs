using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPSGame.Object;
using FPSGame.Engine;

namespace FPSGame.Core.AI
{
    public class DefEnemyAI : IEnemyAI
    {
        #region IEnemyAI Members

        private SimpleCharacter character;

        private IEnemyState initialState;

        private IEnemyState currentState;

        public DefEnemyAI(SimpleCharacter character, IEnemyState initialState)
        {
            this.character = character;
            this.initialState = initialState;
            SetState(initialState);
        }

        public void SetState(IEnemyState state)
        {
            if (currentState != null)
                currentState.End();
            currentState = state;
            currentState.Begin();
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            currentState.Update(gameTime);
        }

        public void Notify(global::FPSGame.Engine.GameEvent.IGameEvent evt)
        {
            //System.Windows.Forms.MessageBox.Show(evt.GetSource() + " / " + character);
            if (evt.RequireSource() && evt.GetSource() != character) return;

            if (evt.GetEventData() == GameEventGenerator.EVENT_SPOT_PLAYER)
            {
                //player is spotted, try to shoot him!
                if (!(currentState is ShootState))
                    SetState(new ShootState(character));
            }
        }

        #endregion
    }
}
