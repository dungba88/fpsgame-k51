﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FPSGame.Object
{
    interface IPlayerCharacter : ICharacter
    {
        void Yaw(double angle);

        void Pitch(double angle);
    }
}
