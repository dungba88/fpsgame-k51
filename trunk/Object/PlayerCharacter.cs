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
        private int HP;
        private FirstPersonCamera camera;
        private Gun gun;

        public PlayerCharacter()
        {
            this.HP = 100;
            this.camera = FPSGame.GetInstance().GetFPSCamera();
            this.gun = new Gun();
        }

        public void Begin()
        {
            ObjectManager.GetInstance().RegisterObject(this);
            gun.Begin();
        }

        public void Update(GameTime gameTime)
        {
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
            Vector2 pos = new Vector2(bounds.Width - texture.Width, bounds.Height - texture.Height);
            FPSGame.GetInstance().DrawSprite(texture, pos);
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
    }
}
