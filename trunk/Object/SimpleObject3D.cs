using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FPSGame.Engine;
using FPSGame.Core;

namespace FPSGame.Object
{
    class SimpleObject3D : IDisplayObject
    {
        private Model model;
        private Matrix world;
        private Vector3 rotation;
        private Vector3 pos;
        private bool dead;

        public SimpleObject3D(Model model)
        {
            //initialize properties
            this.model = model;
            this.world = Matrix.Identity;
            this.rotation = Vector3.Zero;
            this.pos = Vector3.Zero;

            //register with ObjectMangager
            ObjectManager.GetInstance().RegisterObject(this);
        }

        public virtual void Update(GameTime gameTime) {}

        public virtual void Draw3D(GameTime gameTime)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            ICamera fpsCamera = FPSGame.GetInstance().GetFPSCamera();
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect be in mesh.Effects)
                {
                    be.EnableDefaultLighting();
                    be.Projection = fpsCamera.GetProjection();
                    be.View = fpsCamera.GetView();
                    be.World = GetWorld() * mesh.ParentBone.Transform;
                }
            }
        }

        public virtual void Draw(GameTime gameTime) 
        {
            
        }

        public Vector3 GetPosition()
        {
            return this.pos;
        }

        public void SetPosition(Vector3 pos)
        {
            this.pos = pos;
        }
        
        public Vector3 GetRotation()
        {
            return this.rotation;
        }

        public void SetRotation(Vector3 rotation)
        {
            this.rotation = rotation;
        }

        public Matrix GetWorld()
        {
            return world * MathUtils.CreateRotation(rotation) * MathUtils.CreateTranslation(pos);
        }

        public void SetWorld(Matrix world)
        {
            this.world = world;
        }

        public bool IsDead()
        {
            return dead;
        }

        public void Begin()
        {
        }

        public void End()
        {
            dead = true;
        }
    }
}
