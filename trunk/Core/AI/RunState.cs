using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPSGame.Object;
using Microsoft.Xna.Framework;
using FPSGame.Engine;

namespace FPSGame.Core.AI
{
    public class RunState : SimpleState
    {
        private Vector2 target; //the target to run to
        //private int lastGameTime;

        public RunState(SimpleCharacter character)
            : base (character)
        {
            
        }

        public Vector2 GetTarget()
        {
            return target;
        }

        public void SetTarget(Vector2 target)
        {
            this.target = target;
        }

        public override void Update(GameTime gameTime)
        {
            ////using a searching algorithm for every frame will result in greatly reduced performance
            ////so we will wait for a few moment before using the algorithm again
            //lastGameTime += gameTime.ElapsedGameTime.Milliseconds;
            //if (lastGameTime < GetResonableWaitingTime())
            //    return;

            ////if we wait long enough, then we can call the algorithm
            ////TODO: Create a heuristic search algorithm and call it here

            base.Update(gameTime);
        }

        private void UpdateProperties()
        {
            
        }

        //Get the reasonable to wait between each call of the algorithm
        //the closer the player is, the lesser the time is
        public int GetResonableWaitingTime()
        {
            UpdateProperties();
            int time = 0;

            //TODO: Write an algorithm that calculate the reasonable delay time
            //based on the distance between player and this object

            return time;
        }
    }
}
