using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPSGame.Object;
using Microsoft.Xna.Framework;
using FPSGame.Engine;

namespace FPSGame.Core.AI
{
    public class ShootState : SimpleState
    {
        public ShootState(SimpleCharacter character)
            : base (character)
        {
        }

        public override void Begin()
        {
            DefEnemyAI.currentShootingNumber++;
            base.Begin();
        }

        public override void Update(GameTime gameTime)
        {
            EnemyCamera cam = GetCharacter().GetCamera();
            Vector3 pos = FPSGame.GetInstance().GetFPSCamera().GetPosition();
            cam.SetDirection(MathUtils.GetDiff(cam.GetPosition(), pos));

            //rotate a bit
            float f = MathUtils.Random(-2, 2) * (float)Math.PI / 180;
            Vector3 rot = GetCharacter().GetRotation();
            rot.Y += f;
            GetCharacter().SetRotation(rot);

            GetCharacter().Shoot();

            base.Update(gameTime);
        }

        public override void End()
        {
            DefEnemyAI.currentShootingNumber--;
            base.End();
        }
    }
}
