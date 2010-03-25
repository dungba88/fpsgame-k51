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
    public class SimpleObject3D : IDisplayObject3D
    {
        private Model model;
        private Matrix world;
        private Vector3 rotation;
        private Vector3 pos;
        private bool dead;
        private float scale;
        private Vector3 fixPos;
        private float posMultiplicity;

        public SimpleObject3D(Model model, float scale, float posMultiplicity)
        {
            //initialize properties
            init(model, scale, Vector3.Zero, posMultiplicity);
        }

        public SimpleObject3D(Model model, float scale, Vector3 fixPos, float posMultiplicity)
        {
            //initialize properties
            init(model, scale, fixPos, posMultiplicity);
        }

        private void init(Model model, float scale, Vector3 fixPos, float posMultiplicity)
        {
            this.model = model;
            this.posMultiplicity = posMultiplicity;
            this.world = Matrix.Identity * scale;
            this.rotation = Vector3.Zero;
            this.scale = scale;
            this.pos = Vector3.Zero;
            this.fixPos = fixPos;
        }

        public virtual void Update(GameTime gameTime)
        {
            
        }

        public virtual void Draw3D(GameTime gameTime)
        {
            FirstPersonCamera camera = FPSGame.GetInstance().GetFPSCamera();

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect be in mesh.Effects)
                {
                    be.EnableDefaultLighting();
                    be.Projection = camera.GetProjection();
                    be.View = camera.GetView();
                    be.World = GetWorld() * mesh.ParentBone.Transform;
                }
                mesh.Draw();
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
            //System.Windows.Forms.MessageBox.Show(pos.X / scale + "/" + pos.Y / scale + "/" + pos.Z / scale);
            return world * Matrix.CreateScale(scale) * MathUtils.CreateTranslation(fixPos * posMultiplicity) * MathUtils.CreateRotation(rotation) * MathUtils.CreateTranslation(pos * posMultiplicity);
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
            ObjectManager.GetInstance().RegisterObject(this);
        }

        public void End()
        {
            ObjectManager.GetInstance().UnregisterObject(this);
            dead = true;
        }

        public Model GetModel()
        {
            return model;
        }
    }
}
