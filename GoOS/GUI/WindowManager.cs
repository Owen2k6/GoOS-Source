using MOOS;
using MOOS.Graph;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MControl = System.Windows.Forms.Control;

namespace GoOS.GUI
{
    internal class WindowManager
    {
        public List<Window> Windows { get; private set; }
        public Window Focused { get; private set; }
        
        private int oldMouseX, oldMouseY;
        
        public WindowManager()
        {
            Windows = new List<Window>();
        }

        public void addWindow(Window window)
        {
            for (int i = 0; i < Windows.Count; i++) { if (Windows[i].WID == window.WID) return; }

            Windows.Add(window);

            Focused = window;
        }

        public bool[] getMouseOverWindow(Window window)
        {
            bool[] points = new bool[2];

            if (window.X <= oldMouseX && window.Y <= oldMouseY && window.X + window.Width >= oldMouseX && window.Y + window.Height >= oldMouseY) points[0] = true;
            else if (window.X <= oldMouseX + 11 && window.Y <= oldMouseY + 18 && window.X + window.Width >= oldMouseX + 11 && window.Y + window.Height >= oldMouseY + 18) points[1] = true; ;
            return points;
        }

        public void Render(Graphics g)
        {
            bool mouseMoved = oldMouseX != MControl.MousePosition.X || oldMouseY != MControl.MousePosition.Y; // Has mouse been moved?
            bool mouseHandled = !mouseMoved;

            bool[] isOverFocused = getMouseOverWindow(Focused);

            for (int i = 0; i < Windows.Count; i++)
            {
                Window window = Windows[i];

                bool[] isOveri = getMouseOverWindow(window);

                if (mouseMoved && (isOveri[0] || isOveri[1])) // Is mouse over window?
                {
                    if (isOveri[0] && isOveri[1])
                    {
                        if (window.WID == Focused.WID)
                        {
                            window.Update = true;
                            mouseHandled = true;
                        }
                        else if (!(isOverFocused[0] && isOverFocused[1]))
                        {
                            window.Update = true;
                            mouseHandled = true;
                        }
                    }
                    else if (isOveri[0] || isOveri[1])
                    {
                        window.Update = true;
                        for (int x = 0; x < 12; x++)
                        {
                            for (int y = 0; y < 19; y++)
                            {
                                uint colour = Program.ScafellPike.GetPixel(oldMouseX + x, oldMouseY + y);
                                g.DrawPoint(oldMouseX + x, oldMouseY + y, colour);
                            }
                        }
                        mouseHandled = true;
                    }
                }

                if (!window.Update)
                {
                    for (int ii = 0; ii < window.controls.Count; ii++)
                    {
                        if (window.controls[ii].Update)
                        {
                            window.controls[ii].Render();
                            window.controls[ii].Update = false;
                        }
                    }
                }
                else
                {
                    g.FillRectangle(window.X + 1, window.Y + 21, window.Width - 2, window.Height - 26, 0xFFFFFFFF);

                    for (int ii = 0; ii < window.controls.Count; ii++)
                    {
                        window.controls[ii].Render();
                        window.controls[ii].Update = false;
                    }

                    //g.FillRectangle(window.X, window.Y, window.Width, 21, 0xFF7F7F7F);

                    for (int ii = 0; ii < window.Width; ii++) g.DrawImage(window.X + ii, window.Y, Program.WindowbarGradient, false);

                    Program.lg12.DrawString(window.X + (window.Width / 2) - (Program.lg12.MeasureString(window.Title) / 2), window.Y + (26 / 2) - (Program.lg12.MeasureStringVert(window.Title) / 2), window.Title, 0xFFFFFFFF, g);

                    //g.DrawImage(window.X + 5, window.Y + (26 / 2) - (Program.WindowCloseButton.Height / 2), Program.WindowCloseButton);
                    //g.DrawImage(window.X + 20, window.Y + (26 / 2) - (Program.WindowMinButton.Height / 2), Program.WindowMinButton);
                    //g.DrawImage(window.X + 35, window.Y + (26 / 2) - (Program.WindowMaxButton.Height / 2), Program.WindowMaxButton);

                    window.Update = false;
                }

                if (window.Closing)
                {
                    Windows.Remove(window);
                }
            }

            if (!mouseHandled) 
            {
                int offset = 0;
                if (oldMouseY + 19 > Program.ScafellPike.Height) offset = oldMouseY + 19 - Program.ScafellPike.Height;

                for (int x = 0; x < 12; x++)
                {
                    for (int y = 0; y < 19 - offset; y++) 
                    {
                        uint colour = Program.ScafellPike.GetPixel(oldMouseX + x, oldMouseY + y);
                        g.DrawPoint(oldMouseX + x, oldMouseY + y, colour);
                    }
                }
            }

            g.DrawImage(Program.MouseX, Program.MouseY, Program.mouse);

            oldMouseX = Program.MouseX;
            oldMouseY = Program.MouseY;
        }
    }
}
