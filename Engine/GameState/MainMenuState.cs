using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FPSGame.Sprite;
using System.Windows.Forms;
using FPSGame.Core;

namespace FPSGame.Engine.GameState
{
    public class MainMenuState : SimpleGameState
    {
        public override String GetName()
        {
            return "MainMenu";
        }

        public override void OnBegin()
        {
            Texture2D newGame = ResourceManager.GetResource<Texture2D>(ResourceManager.NEW_GAME_BUTTON);
            Texture2D newGameOff = ResourceManager.GetResource<Texture2D>(ResourceManager.NEW_GAME_BUTTON_OFF);
            Texture2D option = ResourceManager.GetResource<Texture2D>(ResourceManager.OPTION_BUTTON);
            Texture2D optionOff = ResourceManager.GetResource<Texture2D>(ResourceManager.OPTION_BUTTON_OFF);
            Texture2D exit = ResourceManager.GetResource<Texture2D>(ResourceManager.EXIT_BUTTON);
            Texture2D exitOff = ResourceManager.GetResource<Texture2D>(ResourceManager.EXIT_BUTTON_OFF);

            int w = FPSGame.GetInstance().GraphicsDevice.Viewport.Width;
            int h = FPSGame.GetInstance().GraphicsDevice.Viewport.Height;

            //create new buttons
            IComponent compNewGame = new Component(newGame, newGameOff, w / 2, h / 2 + 100, true, true);
            IComponent compOption = new Component(option, optionOff, w / 2, h / 2 + 150, true, true);
            IComponent compExitGame = new Component(exit, exitOff, w / 2, h / 2 + 200, true, true);

            ActionListener ngAL = compNewGame.GetDefaultActionListener();
            ActionListener optAL = compOption.GetDefaultActionListener();
            ActionListener egAL = compExitGame.GetDefaultActionListener();

            egAL.SetActionPerformedMethod(new ActionListener.ActionPerformedMethod(ExitGame));

            //add event listener
            compExitGame.AddActionListener(egAL);

            AddComponent(compNewGame);
            AddComponent(compOption);
            AddComponent(compExitGame);

            FPSGame.GetInstance().ShowMouse();
        }

        public override void OnEnd()
        {
        }

        public void ExitGame(IActionEvent evt)
        {
            FPSGame.GetInstance().QuitGame();
        }
    }
}
