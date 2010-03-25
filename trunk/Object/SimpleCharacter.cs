using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FPSGame.Engine;
using System.Collections;
using XNAnimation.Controllers;
using XNAnimation;
using XNAnimation.Effects;
using FPSGame.Core;

namespace FPSGame.Object
{
    public class SimpleCharacter : SimpleObject3D, IBoxShaped
    {
        private int index;
        public Vector3 fixPos;
        public float fixRotX, fixRotY, fixRotZ;
        public float scale;
        private SkinnedModel skinnedModel;
        private AnimationController animController;        

        public SimpleCharacter(SkinnedModel model, float scale, Effect eff, float posMultiplicity)
            : base(model.Model, scale, posMultiplicity)
        {
            init(model, eff);
        }

        public SimpleCharacter(SkinnedModel model, float scale, Vector3 fixPos, Effect eff, float posMultiplicity)
            : base(model.Model, scale, fixPos, posMultiplicity)
        {
            init(model, eff);
        }

        private void init(SkinnedModel model, Effect eff)
        {
            FirstPersonCamera cam = FPSGame.GetInstance().GetFPSCamera();
            this.animController = new AnimationController(model.SkeletonBones);
            this.skinnedModel = model;
            Idle();
        }

        public void Idle()
        {
            RunAnimation("Idle");
        }

        public void Run()
        {
            RunAnimation("Run");
        }

        public void Shoot()
        {
            RunAnimation("Shoot");
        }

        public override void Update(GameTime gameTime)
        {
            animController.Update(gameTime.ElapsedGameTime, GetWorld());
            base.Update(gameTime);
        }

        public void RunAnimation(String anim)
        {
            //animController.StartClip(skinnedModel.AnimationClips[anim]);
            animController.PlayClip(skinnedModel.AnimationClips[anim]);
        }

        public override void Draw3D(GameTime gameTime)
        {
            
            FirstPersonCamera camera = FPSGame.GetInstance().GetFPSCamera();
            foreach (ModelMesh modelMesh in skinnedModel.Model.Meshes)
            {
                foreach (SkinnedModelBasicEffect effect in modelMesh.Effects)
                {
                    // Setup camera
                    effect.View = camera.GetView();
                    effect.Projection = camera.GetProjection();

                    // Set the animated bones to the model
                    effect.Bones = animController.SkinnedBoneTransforms;

                    // OPTIONAL - Configure material
                    effect.Material.DiffuseColor = new Vector3(0.8f);
                    effect.Material.SpecularColor = new Vector3(0.3f);
                    effect.Material.SpecularPower = 8;

                    // OPTIONAL - Configure lights
                    effect.AmbientLightColor = new Vector3(0.1f);
                    effect.LightEnabled = true;
                    effect.EnabledLights = EnabledLights.One;
                    effect.PointLights[0].Color = Vector3.One;
                    effect.PointLights[0].Position = new Vector3(100);
                }

                // Draw a model mesh
                modelMesh.Draw();
            }
        }

        public SkinnedModel GetSkinnedModel()
        {
            return skinnedModel;
        }

        public BoundingBox GetBoundingBox()
        {
            Dictionary<string, object> tagData = (Dictionary<string, object>)GetModel().Tag;
            return (BoundingBox)tagData["BoundingBox"];
        }
    }
}
