using MOOS.Driver;
using System;

namespace MOOS.Misc
{
    public static class Panic
    {
        public static void Error(string msg,bool skippable = false)
        {
            //Kill all CPUs
            LocalAPIC.SendAllInterrupt(0xFD);
            IDT.Disable();
            Framebuffer.TripleBuffered = false;

            ConsoleColour colour = Console.ForegroundColour;

            Console.ForegroundColour = ConsoleColour.Red;
            Console.WriteLine(" GoOS has ran into a critical problem and has been forced to shut down...");
            Console.ForegroundColour = ConsoleColour.LightRed;
            Console.WriteLine(msg);
            Console.ForegroundColour = ConsoleColour.Grey;
            Console.WriteLine(" The system has disabled all CPU activity");
            Console.ForegroundColour = ConsoleColour.White;
            Console.WriteLine(" You will need to restart your computer.");

            Console.ForegroundColour = colour;

            if (!skippable)
            {
                Framebuffer.Update();
                for (; ; );
            }
        }
    }
}