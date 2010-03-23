using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPSGame.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FPSGame.Engine;

namespace FPSGame.Object
{
    public class Gun : IDisplayObject
    {
        public const int MIN_SHOCK = 5;
        public const int MAX_SHOCK = 20;    //crosshair shock
        public const int MAX_LATENCY = 2;    //crosshair shock
        public const float MAX_GUN_SHOCK = 20;     //gun shock
        public const float MAX_GUN_SHOCK_RADIUS = 2;
        public const int MAX_GUNFIRE_LATENCY = 3;

        private bool dead;
        private float shock;
        private Line line;
        private bool shockAdd;
        private bool shooting;
        private int latency;
        private float gunshock;
        private bool drawFire;
        private bool lastDrawFire;
        private int gunfireLat;

        public Gun()
        {

        }

        private void SetShock(float f)
        {
            if (f <= MIN_SHOCK)
                shock = MIN_SHOCK;
            else if (f >= GetMaxShock())
                shock = GetMaxShock();
            else
                shock = f;
        }

        private void SetGunShock(float f)
        {
            if (f <= MIN_SHOCK)
                shock = MIN_SHOCK;
            else if (f >= GetMaxShock())
                shock = GetMaxShock();
            else
                shock = f;
        }

        public void ResetShock()
        {
            SetShock(MIN_SHOCK);
        }

        public void AddShock()
        {
            shockAdd = true;
        }

        public void Shoot()
        {
            if (latency >= MAX_LATENCY)
            {
                drawFire = true;
                if (!shooting)
                {
                    shooting = true;
                    latency = 0;
                }
                AddShock();
                EffectUtils.PlaySound(ResourceManager.PLAYER_GUN_SND);
                if (gunshock < MAX_GUN_SHOCK)
                {
                    gunshock += 1;
                    FPSGame.GetInstance().GetFPSCamera().Pitch(-1);
                }
                else
                {
                    float max = GetMaxGunShockRadius();
                    float p = max * MathUtils.Random(-100, 100) / 100;
                    float y = max * MathUtils.Random(-100, 100) / 100;
                    FPSGame.GetInstance().GetFPSCamera().Pitch(p);
                    FPSGame.GetInstance().GetFPSCamera().Yaw(y);
                }
            }
            else
            {
                drawFire = false;
            }
        }

        public void StopShoot()
        {
            shooting = false;
            drawFire = false;
            gunfireLat = MAX_GUNFIRE_LATENCY;
            if (gunshock > 0)
            {
                gunshock -= 1;
                FPSGame.GetInstance().GetFPSCamera().Pitch(1);
            }
        }

        public bool IsShooting()
        {
            return shooting;
        }

        public void Begin()
        {
            dead = false;
            shockAdd = false;
            lastDrawFire = false;
            drawFire = false;
            shock = MIN_SHOCK;
            gunshock = 0;
            latency = MAX_LATENCY;
            line = new Line(new Color[] { Color.Aqua });
            gunfireLat = MAX_GUNFIRE_LATENCY;
        }

        public void Draw(GameTime gameTime)
        {
            DrawCrosshair();
            if (drawFire)
            {
                if (gunfireLat >= MAX_GUNFIRE_LATENCY)
                {
                    gunfireLat = 0;
                    DrawFire();
                }
                else
                {
                    gunfireLat++;
                }
            }
        }

        public void Draw3D(GameTime gameTime)
        {
        }

        public void Update(GameTime gameTime)
        {
            if (shockAdd)
            {
                SetShock(shock + 0.5f);
                shockAdd = false;
            }
            else
                SetShock(shock - 0.5f);
            if (latency < MAX_LATENCY)
                latency++;
        }

        public void End()
        {
            dead = true;
        }

        public bool IsDead()
        {
            return dead;
        }

        public int GetMaxShock()
        {
            //shock is reduced when crouching
            if (FPSGame.GetInstance().GetFPSCamera().IsStateActive("crouch"))
                return (int)(MAX_SHOCK / 1.5);
            //shock is increases when running
            if (FPSGame.GetInstance().GetPlayer().GetCharacter().IsRunning())
                return (int)(MAX_SHOCK * 1.2);
            return MAX_SHOCK;
        }

        public float GetMaxGunShockRadius()
        {
            if (FPSGame.GetInstance().GetFPSCamera().IsStateActive("crouch"))
                return MAX_GUN_SHOCK_RADIUS / 3;
            if (FPSGame.GetInstance().GetPlayer().GetCharacter().IsRunning())
                return (int)(MAX_GUN_SHOCK_RADIUS * 1.5);
            return MAX_GUN_SHOCK_RADIUS;
        }

        private void DrawCrosshair()
        {
            float centerX = FPSGame.GetInstance().Window.ClientBounds.Width / 2;
            float centerY = FPSGame.GetInstance().Window.ClientBounds.Height / 2;
            line.Render(new Vector2(centerX - shock, centerY), new Vector2(centerX - shock * 1.5f - 15, centerY), Color.Aqua, 0);
            line.Render(new Vector2(centerX + shock * 1.5f + 15, centerY), new Vector2(centerX + shock, centerY), Color.Aqua, 0);
            line.Render(new Vector2(centerX, centerY - shock), new Vector2(centerX, centerY - shock * 1.5f - 15), Color.Aqua, 0);
            line.Render(new Vector2(centerX, centerY + shock * 1.5f + 15), new Vector2(centerX, centerY + shock), Color.Aqua, 0);
        }

        private void DrawFire()
        {
            Texture2D texture = ResourceManager.GetResource<Texture2D>(ResourceManager.GUNFIRE);
            FPSGame.GetInstance().DrawSprite(texture, new Vector2(530, 370));
        }
    }
}