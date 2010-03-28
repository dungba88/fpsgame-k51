using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FPSGame.Core;
using FPSGame.Engine;
using FPSGame.Factory;

namespace FPSGame.Object
{
    public class Bullet : SimpleObject3D , ISphereShaped
    {
        private int life;
        private float speed;
        private float dmg;
        private Vector3 dir;

        public Bullet(Model model, float scale, Vector3 fixPos, Vector3 fixRot, float posMultiplicity, int life, float speed, float dmg, Vector3 dir)
            : base(model, scale, fixPos, fixRot, posMultiplicity)
        {
            this.life = life;
            this.speed = speed;
            this.dmg = dmg;
            this.dir = dir;
            this.dir.Normalize();
        }

        public override void Update(GameTime gameTime)
        {
            //CheckForCollision();
            Move();
            life--;
            if (life <= 0)
            {
                End();
            }
            base.Update(gameTime);
        }

        private void Move()
        {
            Vector3 pos = this.GetPosition();
            pos += dir * speed;
            SetPosition(pos);
        }

        private bool CheckForCollision()
        {
            IDisplayObject[] objects = ObjectManager.GetInstance().GetObjects();

            foreach (IDisplayObject obj in objects)
            {
                if (obj is Collidable && obj is IDisplayObject3D)
                {
                    if (obj is SimpleCharacter) return false;

                    //do a heuristic first
                    if (!CheckForPosibleCollision((IDisplayObject3D)obj)) return false;

                    //then do a complete collision test
                    if (CheckForCollision((IDisplayObject3D)obj))
                    {
                        if (obj is Vulnerable)
                            ((Vulnerable)obj).TakeDamage(dmg);
                        return true;
                    }
                }
            }

            return false;
        }

        public BoundingSphere GetBoundingSphere()
        {
            return GetModel().Meshes.ElementAt(0).BoundingSphere;
        }

        private bool CheckForCollision(IDisplayObject3D obj)
        {
            return ((Collidable)obj).CollideWith(this);
        }

        private bool CheckForPosibleCollision(IDisplayObject3D obj)
        {
            float elemSize = MapLoader.GetInstance().GetMap().GetElemSize();
            Vector3 pos = obj.GetPosition();
            float dist = MathUtils.GetDistance(pos, GetPosition());
            if (dist >= elemSize) return false;
            return true;
        }
    }
}
