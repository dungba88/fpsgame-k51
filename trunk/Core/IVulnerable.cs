using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FPSGame.Core
{
    public interface IVulnerable
    {
        void TakeDamage(int dmg);
    }
}
