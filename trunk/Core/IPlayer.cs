using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPSGame.Object;

namespace FPSGame.Core
{
    interface IPlayer
    {
        String GetName();

        void SetName(String name);

        long GetScore();

        void SetScore(long score);

        void AddScore(long score);

        IKeyboardControls GetKbdControls();

        void SetKbdControls(IKeyboardControls kbpControls);

        IPlayerCharacter GetCharacter();

        void SetCharacter(IPlayerCharacter character);
    }
}
