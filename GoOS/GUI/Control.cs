using System;
using System.Collections.Generic;

namespace GoOS.GUI
{
    internal class Control
    {
        public int X, Y;
        public bool Update = false;

        public virtual void Render()
        {

        }
    }
}
