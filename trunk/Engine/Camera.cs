using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace FPSGame.Engine
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Camera : Microsoft.Xna.Framework.GameComponent, ICamera
    {
        protected Matrix view;
        protected Matrix projection;
        protected Vector3 pos;
        protected Vector3 dir;
        protected Vector3 up;

        public Camera(Game game, Vector3 pos, Vector3 target, Vector3 up)
            : base(game)
        {
            // TODO: Construct any child components here
            this.pos = pos;
            this.dir = target-pos;
            this.dir.Normalize();
            this.up = up;
            Update();

            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                    (float)Game.Window.ClientBounds.Width /
                    (float)Game.Window.ClientBounds.Height,
                1, 1000);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            Begin();
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            Update();
            base.Update(gameTime);
        }

        public Matrix GetView()
        {
            return view;
        }

        public void SetView(Matrix view)
        {
            this.view = view;
        }

        public Matrix GetProjection()
        {
            return projection;
        }

        public void SetProjection(Matrix projection)
        {
            this.projection = projection;
        }

        public void Begin()
        {
        }

        public void Update()
        {
            view = Matrix.CreateLookAt(pos, pos+dir, up);
        }

        public void End()
        {
        }

        public bool IsDead()
        {
            return false;
        }

        public void SetPosition(Vector3 pos)
        {
            this.pos = pos;
        }

        public Vector3 GetPosition()
        {
            return pos;
        }

        public void SetDirection(Vector3 dir)
        {
            this.dir = dir;
        }

        public Vector3 GetDirection()
        {
            return dir;
        }

        public void SetUpVector(Vector3 up)
        {
            this.up = up;
        }

        public Vector3 GetUpVector()
        {
            return up;
        }
    }
}