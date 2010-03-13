using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FPSGame.Sprite;
using System.Windows.Forms;

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

            AddComponent(new Component(newGame, newGameOff, 0, 50, true));
            AddComponent(new Component(option, optionOff, 0, 100, true));
            AddComponent(new Component(exit, exitOff, 0, 150, true));
        }

        public override void OnEnd()
        {
        }
    }
}
