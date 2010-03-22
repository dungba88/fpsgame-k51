using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FPSGame.Core;
using FPSGame.Factory;
using System.Collections;
using FPSGame.Object;

namespace FPSGame.Engine
{
    public class FirstPersonCamera : Camera, IBoxShaped
    {
        private PlayerCharacter character;
        private float normalY;
        private float delta;
        private CameraState state;
        private bool running;

        public FirstPersonCamera(Game game, Vector3 pos, Vector3 target, Vector3 up)
            : base(game, pos, target, up)
        {
            normalY = pos.Y;
            delta = pos.Y / 10;
        }

        private void ChangeState(CameraState cs)
        {
            if (state == null)
            {
                this.state = cs;
                state.Begin();
            }
        }

        public void SetCameraState(String s)
        {
            if (s == "crouch")
            {
                ChangeState(new CrouchCameraState(this, normalY, delta));
            }
            else if (s == "jump")
            {
                ChangeState(new JumpCameraState(this, normalY, delta));
            }
        }

        public bool IsStateActive(String s)
        {
            if (state == null) return false;
            if (s == "crouch" && state is CrouchCameraState) return true;
            if (s == "jump" && state is JumpCameraState) return true;
            return false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (state != null)
            {
                if (!state.IsDead())
                    state.Update(gameTime);
                else
                    state = null;
            }
        }

        public void ApplyToPlayer(PlayerCharacter character)
        {
            this.character = character;
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
                AddShock();
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
                AddShock();
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
                AddShock();
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
                AddShock();
            }
        }

        public float GetSpeed()
        {
            return 0.1f;
        }

        public BoundingBox GetBoundingBox()
        {
            return MathUtils.GetBoundingBox(pos, new Vector3(1, 0, 0), GetAbsoluteUpVector(), 2, 2, 2.5f);
        }

        public void DoState()
        {
            if (state != null)
                state.Do();
        }

        public void UndoState()
        {
            if (state != null)
                state.Undo();
        }

        public void AddShock()
        {
            if (character != null)
                character.GetGun().AddShock();
        }

        private bool CheckCollision()
        {
            ArrayList objects = ObjectManager.GetInstance().GetObjects();
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

    abstract class CameraState : IObject
    {
        protected bool condition;
        private bool dead;

        public abstract void DoState();

        public abstract void UndoState();

        public virtual bool CheckCondition()
        {
            return condition;
        }

        public virtual void Begin()
        {
            condition = true;
            dead = false;
        }

        public virtual void End()
        {
            condition = false;
            dead = true;
        }

        public void Do()
        {
            condition = true;
        }

        public void Undo()
        {
            condition = false;
        }

        public void Update(GameTime gameTime)
        {
            if (CheckCondition())
                DoState();
            else
                UndoState();
        }

        public bool IsDead()
        {
            return dead;
        }
    }

    class CrouchCameraState : CameraState
    {
        private FirstPersonCamera camera;
        private float normalY;
        private float delta;

        public CrouchCameraState(FirstPersonCamera camera, float normalY, float delta)
        {
            this.camera = camera;
            this.normalY = normalY;
            this.delta = delta;
        }

        public override void DoState()
        {
            Vector3 pos = camera.GetPosition();
            if (pos.Y > normalY / 2)
            {
                pos.Y = MathUtils.ClampFloat(pos.Y - delta, normalY / 2, normalY);
                camera.SetPosition(pos);
                camera.AddShock();
            }
        }

        public override void UndoState()
        {
            Vector3 pos = camera.GetPosition();
            if (pos.Y < normalY)
            {
                pos.Y = MathUtils.ClampFloat(pos.Y + delta, normalY / 2, normalY);
                camera.SetPosition(pos);
                camera.AddShock();
                if (pos.Y == normalY)
                    End();
            }
        }

        public override void End()
        {
            Vector3 pos = camera.GetPosition();
            pos.Y = normalY;
            camera.SetPosition(pos);
            base.End();
        }
    }

    class JumpCameraState : CameraState
    {
        private FirstPersonCamera camera;
        private float normalY;
        private float delta;
        private float maxFactor = 2.0f;
        private int maxLat = 4;
        private int latency;

        public JumpCameraState(FirstPersonCamera camera, float normalY, float delta)
        {
            this.camera = camera;
            this.normalY = normalY;
            this.delta = delta;
            this.latency = maxLat;
        }

        public override bool CheckCondition()
        {
            Vector3 pos = camera.GetPosition();
            bool b = base.CheckCondition();
            return (b && (pos.Y < normalY * maxFactor || latency < maxLat));
        }

        public override void DoState()
        {
            Vector3 pos = camera.GetPosition();
            if (pos.Y < normalY * maxFactor || latency < maxLat)
            {
                pos.Y = MathUtils.ClampFloat(pos.Y + delta, normalY, normalY * maxFactor);
                camera.SetPosition(pos);
                camera.AddShock();
                if (pos.Y == normalY * maxFactor)
                {
                    if (latency == 0)
                        condition = false;
                    else
                        latency--;
                }
            }
        }

        public override void UndoState()
        {
            Vector3 pos = camera.GetPosition();
            if (pos.Y > normalY)
            {
                pos.Y = MathUtils.ClampFloat(pos.Y - delta / 1.05f, normalY, normalY * maxFactor);
                camera.SetPosition(pos);
                camera.AddShock();
                if (pos.Y == normalY)
                    End();
            }
        }

        public override void End()
        {
            Vector3 pos = camera.GetPosition();
            pos.Y = normalY;
            camera.SetPosition(pos);
            base.End();
        }
    }
}
