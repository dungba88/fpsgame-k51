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
using FPSGame.Core.AI;

namespace FPSGame.Object
{
    public class SimpleCharacter : SimpleObject3D, IBoxShaped, Collidable , IObserver, Vulnerable
    {
        public const int MAX_GUN_DELAY = 10;
        public static int total;

        public IDictionary<String, Vector3> animFixRot;
        public IDictionary<String, Vector3> animFixPos;
        public float scale;
        //private IDictionary<String, SkinnedModel> models;
        //private IDictionary<String, AnimationController> animControllers;
        private float HPs;
        private int gunDelay;
        private IEnemyAI ai;
        private SkinnedModel currentModel;
        private AnimationController currentAnimation;
        private SimpleObject3D attachment;
        private String attachmentString;
        private Vector3 currentFixRot;
        private Vector3 currentFixPos;
        private bool guard;
        private EnemyCamera cam;
        private String currentAnim;
        private float spd = 0.15f;
        private IEnemyState initState;
        private int id;

        public void TakeDamage(float dmg)
        {
            SetHPs(HPs - dmg);
        }

        private void SetHPs(float hps)
        {
            this.HPs = hps;
            if (HPs < 0) End();
        }

        public override void Begin()
        {
            id = total++;
            ai.SetInitialState(initState);
            ai.Begin();
            base.Begin();
        }

        public int GetId()
        {
            return id;
        }

        public EnemyCamera GetCamera()
        {
            return cam;
        }

        public bool IsGuarding()
        {
            return guard;
        }

        public void Guard(bool guard)
        {
            this.guard = guard;
        }

        public void InitAI(IEnemyAI ai, IEnemyState initState)
        {
            this.ai = ai;
            this.initState = initState;
        }

        public void SetSpeed(float spd)
        {
            this.spd = spd;
        }

        public float GetSpeed()
        {
            return spd;
        }

        public SimpleCharacter(SkinnedModel model, float scale, Effect eff, float posMultiplicity)
            : base(model.Model, scale, posMultiplicity)
        {
            init(model, eff, new Dictionary<String, Vector3>(), new Dictionary<String, Vector3>());
        }

        public SimpleCharacter(SkinnedModel model, float scale, Vector3 fixPos, IDictionary<String, Vector3>animFixPos, IDictionary<String, Vector3>animFixRot, Effect eff, float posMultiplicity)
            : base(null, scale, fixPos, Vector3.Zero, posMultiplicity)
        {
            init(model, eff, animFixPos, animFixRot);
        }

        private void init(SkinnedModel model, Effect eff, IDictionary<String, Vector3> animFixPos, IDictionary<String, Vector3> animFixRot)
        {
            //init camera
            HPs = 100;
            gunDelay = 0;
            Vector3 pos = GetPosition();
            pos.Y = Camera.HEIGHT;
            Vector3 rot = new Vector3((float)Math.Cos(GetRotation().Y), 0, (float)Math.Sin(GetRotation().Y));
            Vector3 target = GetPosition() + rot;
            cam = new EnemyCamera(FPSGame.GetInstance(), pos, target, Vector3.Up);
            cam.ApplyToEnemy(this);
            
            //FirstPersonCamera camera = FPSGame.GetInstance().GetFPSCamera();
            
            //for (int i = 0; i < models.Count; i++)
            //{
            //    animControllers.Add(models.Keys.ElementAt(0), new AnimationController(models.Values.ElementAt(0).SkeletonBones));
            //}
            this.currentModel = model;
            this.currentAnimation = new AnimationController(model.SkeletonBones);
            this.animFixRot = animFixRot;
            this.animFixPos = animFixPos;
            Idle();
            //Shoot();
            //Go();
        }

        public void AttachObject(SimpleObject3D obj, String attachPos)
        {
            this.attachment = obj;
            this.attachmentString = attachPos;
            //obj.Begin();
        }

        public void Go()
        {
            RunAnimation("Run");
        }

        public void Idle()
        {
            RunAnimation("Idle");
        }

        public void Run()
        {
            RunAnimation("Run");
            Vector3 dir = cam.GetDirection();
            SetPosition(GetPosition() + new Vector3(dir.Z, 0, dir.X) * spd);
        }

        public void Shoot()
        {
            if (gunDelay < MAX_GUN_DELAY) return;
            gunDelay = 0;
            Model m = ResourceManager.GetResource<Model>(ResourceManager.BULLET_BALL);
            Vector3 dir = GetCamera().GetDirection();
            Vector3 pos = attachment.GetPosition();
            Bullet b = new Bullet(m, 0.05f, Vector3.Zero, Vector3.Zero, 18f, 100, 0.5f, 10, new Vector3(dir.Z, dir.Y, dir.X));
            b.SetPosition(new Vector3(pos.X, EnemyCamera.HEIGHT*0.75f, pos.Z));
            b.Begin();
            RunAnimation("Shoot");
        }

        public override void Update(GameTime gameTime)
        {
            if (gunDelay < MAX_GUN_DELAY)
                gunDelay++;
            currentAnimation.Update(gameTime.ElapsedGameTime, GetWorld());
            cam.Update(gameTime);
            ai.Update(gameTime);
            Vector3 dir = cam.GetDirection();
            base.Update(gameTime);
        }

        public void RunAnimation(String anim)
        {
            //animController.StartClip(skinnedModel.AnimationClips[anim]);
            if (currentAnim == anim) return;
            currentAnimation.StartClip(currentModel.AnimationClips[anim]);
            currentFixRot = animFixRot[anim];
            currentFixPos = animFixPos[anim];
            currentAnim = anim;
        }

        public override void Draw(GameTime gameTime)
        {
            cam.Draw();
            base.Draw(gameTime);
        }

        public override void Draw3D(GameTime gameTime)
        {
            
            FirstPersonCamera camera = FPSGame.GetInstance().GetFPSCamera();
            foreach (ModelMesh modelMesh in currentModel.Model.Meshes)
            {
                foreach (SkinnedModelBasicEffect effect in modelMesh.Effects)
                {
                    // Setup camera
                    effect.View = camera.GetView();
                    effect.Projection = camera.GetProjection();

                    // Set the animated bones to the model
                    effect.Bones = currentAnimation.SkinnedBoneTransforms;

                    // OPTIONAL - Configure material
                    effect.Material.DiffuseColor = new Vector3(0.8f);
                    effect.Material.SpecularColor = new Vector3(0.3f);
                    effect.Material.SpecularPower = 8;

                    // OPTIONAL - Configure lights
                    effect.AmbientLightColor = new Vector3(1f);
                    effect.LightEnabled = true;
                    effect.EnabledLights = EnabledLights.One;
                    effect.PointLights[0].Color = Vector3.One;
                    effect.PointLights[0].Position = new Vector3(100);
                }

                // Draw a model mesh
                modelMesh.Draw();
            }

            if (attachment != null && attachmentString != null)
            {
                Vector3 scale, trans;
                Quaternion rot;
                Matrix world;
                world = currentAnimation.GetBoneAbsoluteTransform(attachmentString);
                world.Decompose(out scale, out rot, out trans);
                Vector3 pos = trans;
                attachment.SetPosition(pos);
                attachment.SetRotation(rot);
                attachment.SetRotation(currentFixRot);
                attachment.Draw3D(gameTime);
            }
        }

        public SkinnedModel GetSkinnedModel()
        {
            return currentModel;
        }

        public BoundingBox GetBoundingBox()
        {
            Dictionary<string, object> tagData = (Dictionary<string, object>)GetSkinnedModel().Model.Tag;
            return (BoundingBox)tagData["BoundingBox"];
        }

        public float? CollideWith(Ray ray)
        {
            Matrix[] absTrans = new Matrix[currentModel.Model.Bones.Count];
            currentModel.Model.CopyAbsoluteBoneTransformsTo(absTrans);
            return RayCollisionDetector.RayIntersectsModel(ray, currentModel.Model, GetWorld(), absTrans);
        }

        public bool CollideWith(ISphereShaped box)
        {
            return false;
        }

        public bool CollideWith(IBoxShaped sphere)
        {
            return false;
        }

        public override void SetPosition(Vector3 pos)
        {
            base.SetPosition(pos);
            cam.SimpleUpdate();
        }

        public override void SetRotation(Vector3 rotation)
        {
            base.SetRotation(rotation);
            cam.SimpleUpdate();
        }

        #region IObserver Members

        public void Notify(global::FPSGame.Engine.GameEvent.IGameEvent evt)
        {
            ai.Notify(evt);
        }

        #endregion
    }
}