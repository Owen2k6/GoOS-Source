using GoOS.GUI;
using MOOS.Driver;
using System;
using System.Collections.Generic;

namespace GoOS
{
    internal class Process
    {
        public uint PID;

        public List<Window> windows { get; private set; } // Can't have no random voids or classes tampering with our **WINDOWS**.

        public Process(bool window = false)
        {
            if (window) windows = new List<Window>();

            Random random = new Random();
            PID = (uint)random.Next(0, 10000);
        }

        public virtual void Execute()
        {
            for (int i = 0; i < windows.Count; i++)
            {
                if (windows[i].Closing)
                {
                    windows.Remove(windows[i]);
                    continue;
                }
            }
        }
    }
}
