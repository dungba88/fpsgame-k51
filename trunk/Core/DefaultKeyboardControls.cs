using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace FPSGame.Core
{
    class DefaultKeyboardControls : IKeyboardControls
    {
        public Keys MoveForwardKey()
        {
            return Keys.Up;
        }

        public Keys MoveLeftKey()
        {
            return Keys.Left;
        }

        public Keys MoveRightKey()
        {
            return Keys.Right;
        }

        public Keys MoveBackwardKey()
        {
            return Keys.Down;
        }

        public Keys CrouchKey()
        {
            return Keys.RightControl;
        }

        public Keys LayDownKey()
        {
            return Keys.Space;
        }

        public Keys ReloadWeaponKey()
        {
            return Keys.RightShift;
        }
    }
}
