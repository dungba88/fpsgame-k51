using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPSGame.Core;
using Microsoft.Xna.Framework;
using FPSGame.Engine;
using Microsoft.Xna.Framework.Graphics;

namespace FPSGame.Object
{
    public class PlayerCharacter : IDisplayObject
    {
        public const float MAX_DELTA = 5;
        private int HP;
        private FirstPersonCamera camera;
        private Gun gun;
        private bool running;
        private float delta;
        private int sign;
        private FirstPersonCamera cam;

        public PlayerCharacter()
        {
            this.HP = 100;
            this.camera = FPSGame.GetInstance().GetFPSCamera();
            running = false;
            sign = 1;
            this.gun = new Gun();
        }

        public void Begin()
        {
            ObjectManager.GetInstance().RegisterObject(this);
            gun.Begin();
        }

        public void Update(GameTime gameTime)
        {
            cam = FPSGame.GetInstance().GetFPSCamera();
            if (HP < 0) End();
            gun.Update(gameTime);
        }

        public void Draw3D(GameTime gameTime)
        {
            gun.Draw3D(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            Rectangle bounds = FPSGame.GetInstance().Window.ClientBounds;
            Texture2D texture = ResourceManager.GetResource<Texture2D>(ResourceManager.PLAYER_DEFAULT_GUN);
            Vector2 pos = new Vector2(bounds.Width - texture.Width + 1 , bounds.Height - texture.Height + 5);
            if (gun.IsShooting())
            {
                pos.X += 10;
                pos.Y += 10;
            }
            if (running)
            {
                delta += sign;
                if (delta >= MAX_DELTA) sign = -1;
                else if (delta <= -MAX_DELTA) sign = 1;
                pos.Y += delta;
            }
            FPSGame.GetInstance().DrawSprite(texture, pos, 1);
            gun.Draw(gameTime);
        }

        public void End()
        {
            gun.End();
            ObjectManager.GetInstance().UnregisterObject(this);
        }

        public bool IsDead()
        {
            return (HP <= 0);
        }

        public Gun GetGun()
        {
            return gun;
        }

        public void Running()
        {
            running = true;
        }

        public void StopRunning()
        {
            running = false;
            delta = 0;
        }

        public bool IsRunning()
        {
            return running;
        }

        public void Shoot()
        {
            gun.Shoot();
        }

        public void StopShooting()
        {
            gun.StopShoot();
        }
    }
}
