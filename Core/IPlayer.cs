using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPSGame.Object;

namespace FPSGame.Core
{
    interface IPlayer
    {
        public String GetName();

        public void SetName(String name);

        public long GetScore();

        public void SetScore(long score);

        public void AddScore(long score);

        public IKeyboardControls GetKbdControls();

        public void SetKbdControls(IKeyboardControls kbpControls);

        public IPlayerCharacter GetCharacter();

        public void SetCharacter(IPlayerCharacter character);
    }
}
