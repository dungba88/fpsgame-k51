using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FPSGame.Core.AI
{
    public class RotatePlugin : StatePlugin
    {
        public const float MAX_DELAY = 10000;   //10000ms or 10s

        private float delay;

        private float initRot;

        public RotatePlugin(IEnemyState state, float initRot)
            : base(state)
        {
            this.initRot = initRot;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            //rotate this unit to a random rotation
            delay += gameTime.ElapsedGameTime.Milliseconds;
            if (delay >= MAX_DELAY)
            {
                int r = (MathUtils.Random(0, +60) - 30);
                float f = r * (float)Math.PI / 180 + initRot;
                Vector3 rot = GetState().GetCharacter().GetRotation();
                rot.Y = f;
                FPSGame.GetInstance().SetInfo(rot.Y * 180 / Math.PI + "");
                GetState().GetCharacter().SetRotation(rot);
                delay = 0;
            }
            base.Update(gameTime);
        }
    }
}
