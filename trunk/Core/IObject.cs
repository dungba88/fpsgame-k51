using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FPSGame.Core
{
    interface IObject
    {
        public void Begin();

        public void Update();

        public void End();

        public void IsDead();
    }
}
