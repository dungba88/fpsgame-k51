﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FPSGame.Core
{
    public interface IObject
    {
        void Begin();

        void Update(GameTime gameTime);

        void End();

        bool IsDead();
    }
}
