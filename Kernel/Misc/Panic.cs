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

            Console.ForegroundColour = System.ConsoleColour.Red;
            Console.Write("PANIC: ");
            Console.WriteLine(msg);
            Console.WriteLine("All CPU Halted Now!");

            Console.ForegroundColour = colour;

            if (!skippable)
            {
                Framebuffer.Update();
                for (; ; );
            }
        }
    }
}