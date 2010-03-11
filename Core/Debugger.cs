using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FPSGame.Core
{
    public class Debugger
    {
        public bool verbose = false;

        public static Debugger instance = new Debugger();

        public static Debugger GetInstance()
        {
            return instance;
        }

        public void OutputDebugString(String s)
        {
            if (verbose)
                System.Console.Write(s);
        }

        public void OutputFatalError(String s)
        {
            if (verbose)
                throw new Exception("FATAL ERROR: " + s);
        }
    }
}
