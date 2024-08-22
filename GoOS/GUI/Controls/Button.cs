using System;
using System.Collections.Generic;
using System.Drawing;

namespace GoOS.GUI.Controls
{
    internal class Button : Control
    {
        public bool pressed;

        private Image button;
        private Image hoveredButton;
        private Image pressedButton;

        public Button(Image Button, Image HoveredButton, Image PressedButton)
        {
            button = Button;
            hoveredButton = HoveredButton;
            pressedButton = PressedButton;
        }

        public override void Render()
        {
            
        }
    }
}
