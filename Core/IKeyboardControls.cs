using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace FPSGame.Core
{
    interface IKeyboardControls
    {
        public Keys MoveForwardKey();

        public Keys MoveLeftKey();

        public Keys MoveRightKey();

        public Keys MoveBackwardKey();

        public Keys CrouchKey();

        public Keys LayDownKey();

        public Keys ReloadWeaponKey();
    }
}
