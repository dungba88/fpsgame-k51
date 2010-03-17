using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPSGame.Core;

namespace FPSGame.Object
{
    public interface ICharacter : IDisplayObject, IVulnerable
    {
        void MoveForward();

        void MoveBackward();

        void MoveLeft();

        void MoveRight();

        void TurnLeft();

        void TurnRight();

        void Shoot();
    }
}
