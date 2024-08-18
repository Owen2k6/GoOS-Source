using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GoOS.GUI
{
    internal class Window
    {
        public int X, Y;
        public int Width, Height;
        public bool WindowDecorations;
        public bool Closable = true;
        public bool Closing = false;
        public bool Update = true;

        public uint WID { get; private set; }
        public string Title { get; private set; }
        public int MaxWidth { get; private set; }
        public int MaxHeight { get; private set; }
        public int MinWidth { get; private set; }
        public int MinHeight { get; private set; }

        public List<Control> controls;

        public Window(int X, int Y, int Width, int Height, string Title)
        {
            this.X = X;
            this.Y = Y;
            this.Width = Width;
            this.Height = Height;
            this.Title = Title;

            MinWidth = 100;
            MinHeight = 150;

            controls = new List<Control>();

            Random random = new Random();
            WID = (uint)random.Next(0, 10000);
        }

        public Window(int X, int Y, int Width, int Height, string Title, int MinWidth, int MinHeight)
        {
            this.X = X;
            this.Y = Y;
            this.Width = Width;
            this.Height = Height;
            this.Title = Title;

            this.MinWidth = MinWidth;
            this.MinHeight = MinHeight;

            controls = new List<Control>();

            Random random = new Random();
            WID = (uint)random.Next(0, 10000);
        }

        public Window(int X, int Y, int Width, int Height, string Title, int MinWidth, int MinHeight, int MaxWidth, int MaxHeight)
        {
            this.X = X;
            this.Y = Y;
            this.Width = Width;
            this.Height = Height;
            this.Title = Title;

            this.MinWidth = MinWidth;
            this.MinHeight = MinHeight;
            this.MaxWidth = MaxWidth;
            this.MaxHeight = MaxHeight;

            controls = new List<Control>();

            Random random = new Random();
            WID = (uint)random.Next(0, 10000);
        }
    }
}
