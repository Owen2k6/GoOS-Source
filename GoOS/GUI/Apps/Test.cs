using System;
using System.Collections.Generic;

namespace GoOS.GUI.Apps
{
    internal class Test : Process
    {
        public Test() : base(true) 
        {
            Window newWin = new Window(400, 400, 150, 100, "Test");
            windows.Add(newWin);

            Program.wm.addWindow(newWin);
        }

        public override void Execute()
        {
            base.Execute();
        }
    }
}
