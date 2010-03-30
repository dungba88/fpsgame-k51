using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPSGame.Factory;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using FPSGame.Object;
using FPSGame.Core;

namespace FPSGame.Engine.GameState
{
    public class NewGameState : SimpleGameState
    {
        MouseState prevMouseState;
        private bool rightBtnReleased = true;

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
            PlayerCharacter character = FPSGame.GetInstance().GetPlayer().CreatePlayer();
            FPSGame.GetInstance().GetFPSCamera().ApplyToPlayer(character);
            EffectUtils.GetInstance().PlaySound(ResourceManager.OPERA_THEME_SONG, false);
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
            MouseState ms = Mouse.GetState();
            IKeyboardControls controls = FPSGame.GetInstance().GetPlayer().GetControls();
            bool running = false;

            if (kb.IsKeyDown(Keys.I))
                FPSGame.INVISIBLE_MODE = !FPSGame.INVISIBLE_MODE;

            if (kb.IsKeyDown(controls.MoveForwardKey()))
            {
                camera.MoveForward();
                running = true;
            }
            if (kb.IsKeyDown(controls.MoveBackwardKey()))
            {
                camera.MoveBackward();
                running = true;
            }
            if (kb.IsKeyDown(controls.MoveLeftKey()))
            {
                camera.MoveLeft();
                running = true;
            }
            if (kb.IsKeyDown(controls.MoveRightKey()))
            {
                camera.MoveRight();
                running = true;
            }
            if (running)
                FPSGame.GetInstance().GetPlayer().GetCharacter().Running();
            else
                FPSGame.GetInstance().GetPlayer().GetCharacter().StopRunning();

            if (kb.IsKeyDown(controls.CrouchKey()))
            {
                camera.SetCameraState("crouch");
            }
            else
            {
                if (camera.IsStateActive("crouch"))
                    camera.UndoState();
            }
            if (ms.RightButton == ButtonState.Pressed && rightBtnReleased == true)
            {
                camera.SetCameraState("jump");
                rightBtnReleased = false;
            }
            else
            {
                rightBtnReleased = true;
            }
            if (ms.LeftButton == ButtonState.Pressed)
            {
                FPSGame.GetInstance().GetPlayer().GetCharacter().Shoot();
            }
            else
            {
                FPSGame.GetInstance().GetPlayer().GetCharacter().StopShooting();
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
            EffectUtils.GetInstance().StopSound(ResourceManager.OPERA_THEME_SONG);
        }
    }
}
