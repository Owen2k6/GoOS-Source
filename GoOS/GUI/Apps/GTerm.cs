﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoOS.GUI.Apps
{
    public class GTerm : Window
    {
        public GTerm()
        {
            Contents = BetterConsole.Canvas;
            X = 20;
            Y = 50;
            Title = "GTerm";
            Visible = true;
            Closable = true;
        }
    }
}
