using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPSGame.Core;

namespace FPSGame.Object.Simulation
{
    public interface ICharacter2D:IDisplayObject2D, Vulnerable
    {
        void MoveLeft();

        void MoveRight();

        void MoveBackward();

        void MoveForward();

        void Shoot();

        void Rotate(double angle);
    }
}
