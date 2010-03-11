using System;

namespace FPSGame
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (FPSGame game = FPSGame.GetInstance())
            {
                game.Run();
            }
        }
    }
}

