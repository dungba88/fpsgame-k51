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

        public override void Update(GameTime gameTime)
        {
            EnemyCamera cam = GetCharacter().GetCamera();
            Vector3 pos = FPSGame.GetInstance().GetFPSCamera().GetPosition();
            cam.SetDirection(MathUtils.GetDiff(cam.GetPosition(), pos));
            GetCharacter().Shoot();
        }
    }
}
