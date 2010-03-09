using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace FPSGame.Core
{
    interface IKeyboardControls
    {
        Keys MoveForwardKey();

        Keys MoveLeftKey();

        Keys MoveRightKey();

        Keys MoveBackwardKey();

        Keys CrouchKey();

        Keys LayDownKey();

        Keys ReloadWeaponKey();
    }
}
