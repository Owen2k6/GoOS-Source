using MOOS;
using MOOS.Graph;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GoOS.GUI
{
    internal class WindowManager
    {
        public List<Window> windows { get; private set; }
        //public List<(int, int, int, int)> dirtyRectangles;
        
        public WindowManager()
        {
            windows = new List<Window>();
        }

        public void addWindow(Window window)
        {
            for (int i = 0; i < windows.Count; i++) { if (windows[i].WID == window.WID) return; }

            windows.Add(window);
        }

        public void Render(Graphics g)
        {
            for (int i = 0; i < windows.Count; i++) 
            {
                Window window = windows[i];

                if (window.Closing)
                {
                    windows.Remove(window);
                    continue;
                }

                if (!window.Update)
                {
                    for (int ii = 0; ii < window.controls.Count; ii++)
                    {
                        Control control = window.controls[ii];

                        if (control.Update) control.Render();
                    }
                }
                else
                {
                    g.FillRectangle(window.X + 1, window.Y + 21, window.Width - 2, window.Height - 22, 0xFFFFFFFF);
                    
                    for (int ii = 0; ii < window.controls.Count; ii++) window.controls[ii].Render();

                    g.FillRectangle(window.X, window.Y, window.Width, 21, 0xFF7F7F7F);

                    Program.lg12.DrawString(window.X + (window.Width / 2) - (Program.lg12.MeasureString(window.Title) / 2), window.Y + (21 / 2) - (Program.lg12.MeasureStringVert(window.Title) / 2), window.Title, 0xFF000000, g);
                }
            }
        }
    }
}
