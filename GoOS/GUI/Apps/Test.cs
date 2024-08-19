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
            int one = 1; int two = 0;
            int test = one / two;
            Programme.wm.addWindow(newWin);
        }

        public override void Execute()
        {
            base.Execute();
        }
    }
}
