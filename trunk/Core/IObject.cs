using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FPSGame.Core
{
    interface IObject
    {
        void Begin();

        void Update();

        void End();

        bool IsDead();
    }
}
