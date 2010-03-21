using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FPSGame.Core
{
    public interface Collidable
    {
        bool CollideWith(IBoxShaped obj);

        bool CollideWith(ISphereShaped obj);
    }
}
