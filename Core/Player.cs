using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPSGame.Object;

namespace FPSGame.Core
{
    public class Player
    {
        public const long MAX_SCORE = 1000000;

        private PlayerCharacter character;
        private IKeyboardControls controls;
        private long score;

        public Player()
        {
            score = 0;
            controls = new DefaultKeyboardControls();
        }

        public IKeyboardControls GetControls()
        {
            return controls;
        }

        public PlayerCharacter CreatePlayer()
        {
            if (character != null)
                character.End();
            character = new PlayerCharacter();
            character.Begin();
            return character;
        }

        public PlayerCharacter GetCharacter()
        {
            return this.character;
        }

        public long GetScore()
        {
            return score;
        }

        public void AddScore(long s)
        {
            SetScore(score + s);
        }

        public void SubstractScore(long s)
        {
            SetScore(score - s);
        }

        private void SetScore(long s)
        {
            if (s < 0) 
                s = 0;
            else if (s > MAX_SCORE) 
                s = MAX_SCORE;
            this.score = s;
        }
    }
}