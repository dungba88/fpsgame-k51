using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FPSGame.Object;
using FPSGame.Core;
using FPSGame.Factory;

namespace FPSGame.Engine
{
    public class EnemyCamera : PersonalCamera
    {
        public const float MAX_ANGLE = (float)Math.PI/180*60;

        String visible = "visible";

        Ray ray;

        public EnemyCamera(Game game, Vector3 pos, Vector3 target, Vector3 up)
            : base(game, pos, target, up)
        {
            ray = new Ray();
        }

        public void ApplyToEnemy(SimpleCharacter character)
        {
            ApplyToObject(character);
        }

        public void SimpleUpdate()
        {
            IDisplayObject obj = GetObject();
            if (obj != null && obj is SimpleCharacter)
            {
                SimpleCharacter character = (SimpleCharacter)obj;
                this.pos = character.GetPosition();
                this.pos.Y = Camera.HEIGHT;
                this.dir = new Vector3((float)Math.Cos(character.GetRotation().Y), 0, (float)Math.Sin(character.GetRotation().Y));
            }
        }

        public override void Update()
        {
            SimpleUpdate();
            if (IsPositionVisible(FPSGame.GetInstance().GetFPSCamera().GetPosition()))
            {
                FPSGame.GetInstance().SetInfo("spotted player");
                FPSGame.GetInstance().AppendInfo("");
                GameEventGenerator.GenerateEvent(this.GetObject(), default(IObject), GameEventGenerator.EVENT_SPOT_PLAYER, "", "", true, false);
            }
            else
            {
                FPSGame.GetInstance().SetInfo("player is not spotted");
                GameEventGenerator.GenerateEvent(this.GetObject(), default(IObject), GameEventGenerator.EVENT_PLAYER_NOT_SPOTTED, "", "", true, false);
            }
            base.Update();
        }

        public void Draw()
        {
            //FPSGame.GetInstance().DrawLine3D(ray.Position, ray.Position + ray.Direction * fpos * 0.75f);
        }

        public override void SetPosition(Vector3 pos)
        {
            IDisplayObject obj = GetObject();
            if (obj != null && obj is SimpleCharacter)
            {
                //System.Windows.Forms.MessageBox.Show("hic"); 
                SimpleCharacter character = (SimpleCharacter)obj;
                pos.Y = character.GetPosition().Y;
                character.SetPosition(pos);
            }
            base.SetPosition(pos);
        }

        public override void SetDirection(Vector3 dir)
        {
            IDisplayObject obj = GetObject();
            if (obj != null && obj is SimpleCharacter)
            {
                SimpleCharacter character = (SimpleCharacter)obj;
                Vector3 rot = character.GetRotation();
                rot.Y = MathUtils.GetAngleFromVector(dir);
                character.SetRotation(rot);
            }
            base.SetDirection(dir);
        }

        public bool IsPositionVisible(Vector3 pos)
        {
            Vector3 diff = MathUtils.GetDiff(GetPosition(), pos);
            diff.Normalize();
            float a = MathUtils.GetAngleFromVector(diff);
            float f = MathUtils.GetAngleFromVector(GetDirection());

            //first, check to see if the player is in this unit's field of view
            if (!MathUtils.IsBound(f - MAX_ANGLE, f + MAX_ANGLE, a))
            {
                FPSGame.GetInstance().AppendInfo(" (not in field-of-view : " + MathHelper.ToDegrees(f - MAX_ANGLE) + " : " + MathHelper.ToDegrees(a) + " : " + MathHelper.ToDegrees(f + MAX_ANGLE) + ")");
                return false;
            }

            //if he is, do a ray-cast collision test
            //to see if there is any obstacle between him and this unit
            IDisplayObject[] objects = ObjectManager.GetInstance().GetObjects();
            FirstPersonCamera cam = FPSGame.GetInstance().GetFPSCamera();
            Vector3 dir = pos - GetPosition();
            ray = new Ray(GetPosition(), dir);

            //the closet intersection is the distance between player and this unit
            float closestIntersection = MathUtils.GetDistance(pos, GetPosition());
            //fpos = closestIntersection;
            float? intersection;
            Vector3 poss = new Vector3();
            for (int i = 0; i < objects.Length; i++)
            {
                IDisplayObject dobj = objects[i];
                if (dobj is Collidable && dobj is Wall3D && !(dobj is Brick))
                {
                    poss = ((Wall3D)dobj).GetBrick().GetPosition();
                    //FPSGame.GetInstance().AppendInfo(poss.X + "/" + poss.Z + " Pos: " + pos.X + "/" + pos.Z + " objPos: " + GetPosition().X + "/" + GetPosition().Z);
                    
                    //we only check for object that 'may' stand between the source and the destination
                    if (!CheckForPosibilityCollision(pos, dobj))
                        continue;

                    //finally, do a ray-cast collision test
                    intersection = ((Collidable)dobj).CollideWith(ray);
                    if (intersection.Value < closestIntersection)
                    {
                        //collide has been detected, so there must be an obstacle between two objects
                        FPSGame.GetInstance().AppendInfo(" (has an obstacle) "+intersection.Value + " : " + closestIntersection);
                        return false;
                    }
                }
            }
            //no obstacle found, the player is visible to this unit
            return true;
        }

        public void Face(Vector3 target)
        {
            SetDirection(MathUtils.GetDiff(GetPosition(), target));
        }

        public void FacePlayer()
        {
            Face(FPSGame.GetInstance().GetFPSCamera().GetPosition());
        }

        public bool CheckForPosibilityCollision(Vector3 src, IDisplayObject dobj)
        {
            if (!(dobj is IDisplayObject3D)) return false;
            float elemSize = MapLoader.GetInstance().GetMap().GetElemSize();
            IDisplayObject3D obj3d = (IDisplayObject3D)dobj;
            Vector3 pos = GetPosition();
            Vector3 dir = src - GetPosition();
            int signX = Math.Sign(dir.X) == 0 ? 1 : Math.Sign(dir.X);
            int signZ = Math.Sign(dir.Z) == 0 ? 1 : Math.Sign(dir.Z);

            if (dir.X < elemSize)
            {
                src.X = src.X + signX * elemSize;
                pos.X = pos.X - signX * elemSize;
            }
            if (dir.Z < elemSize)
            {
                src.Z = src.Z + signZ * elemSize;
                pos.Z = pos.Z - signZ * elemSize;
            }

            return (MathUtils.IsInRange(src, GetPosition(), obj3d.GetPosition()));
        }
    }
}
