using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPSGame.Factory;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace FPSGame.Engine.GameState
{
    public class NewGameState : SimpleGameState
    {
        MouseState prevMouseState;

        public override String GetName()
        {
            return "New Game";
        }

        public override void OnBegin()
        {
            MapLoader.GetInstance().BuildMap("Maps/map1.xml");
            Mouse.SetPosition(FPSGame.GetInstance().Window.ClientBounds.Width / 2,
                FPSGame.GetInstance().Window.ClientBounds.Height / 2);
            prevMouseState = Mouse.GetState();
            FPSGame.GetInstance().HideMouse();
        }

        public override void OnDraw(GameTime gameTime)
        {
            ObjectManager.GetInstance().Draw(gameTime);
        }

        public override void OnDraw3D(GameTime gameTime)
        {
            ObjectManager.GetInstance().Draw3D(gameTime);
        }

        public override void OnUpdate(GameTime gameTime)
        {
            ObjectManager.GetInstance().Update(gameTime);

            //check for user input
            FirstPersonCamera camera = FPSGame.GetInstance().GetFPSCamera();
            KeyboardState kb = Keyboard.GetState();

            if (kb.IsKeyDown(Keys.Up))
            {
                camera.MoveForward();
            }
            if (kb.IsKeyDown(Keys.Down))
            {
                camera.MoveBackward();
            }
            if (kb.IsKeyDown(Keys.Left))
            {
                camera.MoveLeft();
            }
            if (kb.IsKeyDown(Keys.Right))
            {
                camera.MoveRight();
            }

            //Yaw rotation
            float f = Mouse.GetState().X - prevMouseState.X;
            camera.Yaw(f);

            //Pitch rotation
            f = Mouse.GetState().Y - prevMouseState.Y;
            camera.Pitch(f);

            //always move the mouse to center of the screen
            Mouse.SetPosition(FPSGame.GetInstance().Window.ClientBounds.Width / 2,
                FPSGame.GetInstance().Window.ClientBounds.Height / 2);
            prevMouseState = Mouse.GetState();
        }

        public override void OnEnd()
        {
            FPSGame.GetInstance().ShowMouse();
        }
    }
}
