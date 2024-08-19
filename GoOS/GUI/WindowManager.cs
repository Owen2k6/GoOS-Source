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
            bool[] points = new bool[4];

            if (window.X <= oldMouseX && window.Y <= oldMouseY && window.X + window.Width >= oldMouseX && window.Y + window.Height >= oldMouseY) points[0] = true;
            else if (window.X <= oldMouseX + 11 && window.Y <= oldMouseY + 18 && window.X + window.Width >= oldMouseX + 11 && window.Y + window.Height >= oldMouseY + 18) points[1] = true;
            else if (window.X <= oldMouseX + 11 && window.Y <= oldMouseY && window.X + window.Width >= oldMouseX + 11 && window.Y + window.Height >= oldMouseY) points[2] = true;
            else if (window.X <= oldMouseX && window.Y <= oldMouseY + 18 && window.X + window.Width >= oldMouseX && window.Y + window.Height >= oldMouseY + 18) points[3] = true;
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

                if (mouseMoved && (isOveri[0] || isOveri[1] || isOveri[2] || isOveri[3])) // Is mouse over window?
                {
                    if (isOveri[0] && isOveri[1] && isOveri[2] && isOveri[3])
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
                    else if (isOveri[0] || isOveri[1] || isOveri[2] || isOveri[3])
                    {
                        window.Update = true;
                        for (int x = 0; x < 12; x++)
                        {
                            for (int y = 0; y < 19; y++)
                            {
                                uint colour = Programme.ScafellPike.GetPixel(oldMouseX + x, oldMouseY + y);
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

                    for (int ii = 0; ii < window.Width; ii++) g.DrawImage(window.X + ii, window.Y, Programme.WindowbarGradient, false);

                    Programme.lg12.DrawString(window.X + (window.Width / 2) - (Programme.lg12.MeasureString(window.Title) / 2), window.Y + (26 / 2) - (Programme.lg12.MeasureStringVert(window.Title) / 2), window.Title, 0xFFFFFFFF, g);

                    //g.DrawImage(window.X + 5, window.Y + (26 / 2) - (Programme.WindowCloseButton.Height / 2), Programme.WindowCloseButton);
                    //g.DrawImage(window.X + 20, window.Y + (26 / 2) - (Programme.WindowMinButton.Height / 2), Programme.WindowMinButton);
                    //g.DrawImage(window.X + 35, window.Y + (26 / 2) - (Programme.WindowMaxButton.Height / 2), Programme.WindowMaxButton);

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
                if (oldMouseY + 19 > Programme.ScafellPike.Height) offset = oldMouseY + 19 - Programme.ScafellPike.Height;

                for (int x = 0; x < 12; x++)
                {
                    for (int y = 0; y < 19 - offset; y++) 
                    {
                        uint colour = Programme.ScafellPike.GetPixel(oldMouseX + x, oldMouseY + y);
                        g.DrawPoint(oldMouseX + x, oldMouseY + y, colour);
                    }
                }
            }

            g.DrawImage(Programme.MouseX, Programme.MouseY, Programme.mouse);

            oldMouseX = Programme.MouseX;
            oldMouseY = Programme.MouseY;
        }
    }
}
