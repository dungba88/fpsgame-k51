using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FPSGame.Core;
using System.Collections;

namespace FPSGame.Engine
{
    public class PersonalCamera : Camera , IBoxShaped
    {
        private IDisplayObject obj;

        public IDisplayObject GetObject()
        {
            return obj;
        }

        public PersonalCamera(Game game, Vector3 pos, Vector3 target, Vector3 up)
            : base(game, pos, target, up)
        {

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void ApplyToObject(IDisplayObject obj)
        {
            this.obj = obj;
        }

        public void Strafe(double speed)
        {
        }

        public void Yaw(float f)
        {
            SetDirection(Vector3.Transform(GetDirection(), Matrix.CreateFromAxisAngle(
                GetUpVector(), 
                (-MathHelper.PiOver4 / 600) * f)));
        }

        public void Pitch(float f)
        {
            Vector3 cross = Vector3.Cross(GetUpVector(), GetDirection());
            cross.Normalize();

            SetDirection(Vector3.Transform(GetDirection(), Matrix.CreateFromAxisAngle(
                cross, (MathHelper.PiOver4 / 900) * f)));
            //SetUpVector(Vector3.Transform(GetUpVector(), Matrix.CreateFromAxisAngle(
              //  cross, (MathHelper.PiOver4 / 600) * f)));
        }

        public Vector3 GetAbsoluteUpVector()
        {
            return new Vector3(0, 1, 0);
        }

        public Vector3 GetAbsoluteDirection()
        {
            return new Vector3(GetDirection().X, 0, GetDirection().Z);
        }

        public void MoveForward()
        {
            Vector3 oldPos = GetPosition();
            SetPosition(GetPosition() + GetAbsoluteDirection() * GetSpeed());
            if (CheckCollision())
            {
                SetPosition(oldPos);
            }
            else
            {
                MoveThrough();
            }
        }

        public void MoveBackward()
        {
            Vector3 oldPos = GetPosition();
            SetPosition(GetPosition() - GetAbsoluteDirection() * GetSpeed());
            if (CheckCollision())
            {
                SetPosition(oldPos);
            }
            else
            {
                MoveThrough();
            }
        }

        public void MoveLeft()
        {
            Vector3 oldPos = GetPosition();
            SetPosition(GetPosition() + Vector3.Cross(GetAbsoluteUpVector(), GetAbsoluteDirection()) * GetSpeed());
            if (CheckCollision())
            {
                SetPosition(oldPos);
            }
            else
            {
                MoveThrough();
            }
        }

        public void MoveRight()
        {
            Vector3 oldPos = GetPosition();
            SetPosition(GetPosition() - Vector3.Cross(GetAbsoluteUpVector(), GetAbsoluteDirection()) * GetSpeed());
            if (CheckCollision())
            {
                SetPosition(oldPos);
            }
            else
            {
                MoveThrough();
            }
        }

        public virtual void MoveThrough()
        {
        }

        public float GetSpeed()
        {
            return 0.1f;
        }

        public BoundingBox GetBoundingBox()
        {
            return MathUtils.GetBoundingBox(pos, new Vector3(1, 0, 0), GetAbsoluteUpVector(), 2, 2, 2.5f);
        }

        private bool CheckCollision()
        {
            IDisplayObject[] objects = ObjectManager.GetInstance().GetObjects();
            foreach (object obj in objects)
            {
                if (obj != null && obj is Collidable)
                {
                    Collidable col = (Collidable)obj;
                    if (col.CollideWith(this))
                        return true;
                }
            }

            return false;
        }
    }
}
