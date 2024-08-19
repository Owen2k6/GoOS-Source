namespace System.Drawing
{
    public struct Colour
    {
        public static Colour AliceBlue { get { return FromArgb(240, 248, 255); } }
        public static Colour LightSalmon { get { return FromArgb(255, 160, 122); } }
        public static Colour AntiqueWhite { get { return FromArgb(250, 235, 215); } }
        public static Colour LightSeaGreen { get { return FromArgb(32, 178, 170); } }
        public static Colour Aqua { get { return FromArgb(0, 255, 255); } }
        public static Colour LightSkyBlue { get { return FromArgb(135, 206, 250); } }
        public static Colour Aquamarine { get { return FromArgb(127, 255, 212); } }
        public static Colour LightSlateGrey { get { return FromArgb(119, 136, 153); } }
        public static Colour Azure { get { return FromArgb(240, 255, 255); } }
        public static Colour LightSteelBlue { get { return FromArgb(176, 196, 222); } }
        public static Colour Beige { get { return FromArgb(245, 245, 220); } }
        public static Colour LightYellow { get { return FromArgb(255, 255, 224); } }
        public static Colour Bisque { get { return FromArgb(255, 228, 196); } }
        public static Colour Lime { get { return FromArgb(0, 255, 0); } }
        public static Colour Black { get { return FromArgb(0, 0, 0); } }
        public static Colour LimeGreen { get { return FromArgb(50, 205, 50); } }
        public static Colour BlanchedAlmond { get { return FromArgb(255, 255, 205); } }
        public static Colour Linen { get { return FromArgb(250, 240, 230); } }
        public static Colour Blue { get { return FromArgb(0, 0, 255); } }
        public static Colour Magenta { get { return FromArgb(255, 0, 255); } }
        public static Colour BlueViolet { get { return FromArgb(138, 43, 226); } }
        public static Colour Maroon { get { return FromArgb(128, 0, 0); } }
        public static Colour Brown { get { return FromArgb(165, 42, 42); } }
        public static Colour MediumAquamarine { get { return FromArgb(102, 205, 170); } }
        public static Colour BurlyWood { get { return FromArgb(222, 184, 135); } }
        public static Colour MediumBlue { get { return FromArgb(0, 0, 205); } }
        public static Colour CadetBlue { get { return FromArgb(95, 158, 160); } }
        public static Colour MediumOrchid { get { return FromArgb(186, 85, 211); } }
        public static Colour Chartreuse { get { return FromArgb(127, 255, 0); } }
        public static Colour MediumPurple { get { return FromArgb(147, 112, 219); } }
        public static Colour Chocolate { get { return FromArgb(210, 105, 30); } }
        public static Colour MediumSeaGreen { get { return FromArgb(60, 179, 113); } }
        public static Colour Coral { get { return FromArgb(255, 127, 80); } }
        public static Colour MediumSlateBlue { get { return FromArgb(123, 104, 238); } }
        public static Colour CornflowerBlue { get { return FromArgb(100, 149, 237); } }
        public static Colour MediumSpringGreen { get { return FromArgb(0, 250, 154); } }
        public static Colour Cornsilk { get { return FromArgb(255, 248, 220); } }
        public static Colour MediumTurquoise { get { return FromArgb(72, 209, 204); } }
        public static Colour Crimson { get { return FromArgb(220, 20, 60); } }
        public static Colour MediumVioletRed { get { return FromArgb(199, 21, 112); } }
        public static Colour Cyan { get { return FromArgb(0, 255, 255); } }
        public static Colour MidnightBlue { get { return FromArgb(25, 25, 112); } }
        public static Colour DarkBlue { get { return FromArgb(0, 0, 139); } }
        public static Colour MintCream { get { return FromArgb(245, 255, 250); } }
        public static Colour DarkCyan { get { return FromArgb(0, 139, 139); } }
        public static Colour MistyRose { get { return FromArgb(255, 228, 225); } }
        public static Colour DarkGoldenrod { get { return FromArgb(184, 134, 11); } }
        public static Colour Moccasin { get { return FromArgb(255, 228, 181); } }
        public static Colour DarkGrey { get { return FromArgb(169, 169, 169); } }
        public static Colour NavajoWhite { get { return FromArgb(255, 222, 173); } }
        public static Colour DarkGreen { get { return FromArgb(0, 100, 0); } }
        public static Colour Navy { get { return FromArgb(0, 0, 128); } }
        public static Colour DarkKhaki { get { return FromArgb(189, 183, 107); } }
        public static Colour OldLace { get { return FromArgb(253, 245, 230); } }
        public static Colour DarkMagena { get { return FromArgb(139, 0, 139); } }
        public static Colour Olive { get { return FromArgb(128, 128, 0); } }
        public static Colour DarkOliveGreen { get { return FromArgb(85, 107, 47); } }
        public static Colour OliveDrab { get { return FromArgb(107, 142, 45); } }
        public static Colour DarkOrange { get { return FromArgb(255, 140, 0); } }
        public static Colour Orange { get { return FromArgb(255, 165, 0); } }
        public static Colour DarkOrchid { get { return FromArgb(153, 50, 204); } }
        public static Colour OrangeRed { get { return FromArgb(255, 69, 0); } }
        public static Colour DarkRed { get { return FromArgb(139, 0, 0); } }
        public static Colour Orchid { get { return FromArgb(218, 112, 214); } }
        public static Colour DarkSalmon { get { return FromArgb(233, 150, 122); } }
        public static Colour PaleGoldenrod { get { return FromArgb(238, 232, 170); } }
        public static Colour DarkSeaGreen { get { return FromArgb(143, 188, 143); } }
        public static Colour PaleGreen { get { return FromArgb(152, 251, 152); } }
        public static Colour DarkSlateBlue { get { return FromArgb(72, 61, 139); } }
        public static Colour PaleTurquoise { get { return FromArgb(175, 238, 238); } }
        public static Colour DarkSlateGrey { get { return FromArgb(40, 79, 79); } }
        public static Colour PaleVioletRed { get { return FromArgb(219, 112, 147); } }
        public static Colour DarkTurquoise { get { return FromArgb(0, 206, 209); } }
        public static Colour PapayaWhip { get { return FromArgb(255, 239, 213); } }
        public static Colour DarkViolet { get { return FromArgb(148, 0, 211); } }
        public static Colour PeachPuff { get { return FromArgb(255, 218, 155); } }
        public static Colour DeepPink { get { return FromArgb(255, 20, 147); } }
        public static Colour Peru { get { return FromArgb(205, 133, 63); } }
        public static Colour DeepSkyBlue { get { return FromArgb(0, 191, 255); } }
        public static Colour Pink { get { return FromArgb(255, 192, 203); } }
        public static Colour DimGrey { get { return FromArgb(105, 105, 105); } }
        public static Colour Plum { get { return FromArgb(221, 160, 221); } }
        public static Colour DodgerBlue { get { return FromArgb(30, 144, 255); } }
        public static Colour PowderBlue { get { return FromArgb(176, 224, 230); } }
        public static Colour Firebrick { get { return FromArgb(178, 34, 34); } }
        public static Colour Purple { get { return FromArgb(128, 0, 128); } }
        public static Colour FloralWhite { get { return FromArgb(255, 250, 240); } }
        public static Colour Red { get { return FromArgb(255, 0, 0); } }
        public static Colour ForestGreen { get { return FromArgb(34, 139, 34); } }
        public static Colour RosyBrown { get { return FromArgb(188, 143, 143); } }
        public static Colour Fuschia { get { return FromArgb(255, 0, 255); } }
        public static Colour RoyalBlue { get { return FromArgb(65, 105, 225); } }
        public static Colour Gainsboro { get { return FromArgb(220, 220, 220); } }
        public static Colour SaddleBrown { get { return FromArgb(139, 69, 19); } }
        public static Colour GhostWhite { get { return FromArgb(248, 248, 255); } }
        public static Colour Salmon { get { return FromArgb(250, 128, 114); } }
        public static Colour Gold { get { return FromArgb(255, 215, 0); } }
        public static Colour SandyBrown { get { return FromArgb(244, 164, 96); } }
        public static Colour Goldenrod { get { return FromArgb(218, 165, 32); } }
        public static Colour SeaGreen { get { return FromArgb(46, 139, 87); } }
        public static Colour Grey { get { return FromArgb(128, 128, 128); } }
        public static Colour Seashell { get { return FromArgb(255, 245, 238); } }
        public static Colour Green { get { return FromArgb(0, 128, 0); } }
        public static Colour Sienna { get { return FromArgb(160, 82, 45); } }
        public static Colour GreenYellow { get { return FromArgb(173, 255, 47); } }
        public static Colour Silver { get { return FromArgb(192, 192, 192); } }
        public static Colour Honeydew { get { return FromArgb(240, 255, 240); } }
        public static Colour SkyBlue { get { return FromArgb(135, 206, 235); } }
        public static Colour HotPink { get { return FromArgb(255, 105, 180); } }
        public static Colour SlateBlue { get { return FromArgb(106, 90, 205); } }
        public static Colour IndianRed { get { return FromArgb(205, 92, 92); } }
        public static Colour SlateGrey { get { return FromArgb(112, 128, 144); } }
        public static Colour Indigo { get { return FromArgb(75, 0, 130); } }
        public static Colour Snow { get { return FromArgb(255, 250, 250); } }
        public static Colour Ivory { get { return FromArgb(255, 240, 240); } }
        public static Colour SpringGreen { get { return FromArgb(0, 255, 127); } }
        public static Colour Khaki { get { return FromArgb(240, 230, 140); } }
        public static Colour SteelBlue { get { return FromArgb(70, 130, 180); } }
        public static Colour Lavender { get { return FromArgb(230, 230, 250); } }
        public static Colour Tan { get { return FromArgb(210, 180, 140); } }
        public static Colour LavenderBlush { get { return FromArgb(255, 240, 245); } }
        public static Colour Teal { get { return FromArgb(0, 128, 128); } }
        public static Colour LawnGreen { get { return FromArgb(124, 252, 0); } }
        public static Colour Thistle { get { return FromArgb(216, 191, 216); } }
        public static Colour LemonChiffon { get { return FromArgb(255, 250, 205); } }
        public static Colour Tomato { get { return FromArgb(253, 99, 71); } }
        public static Colour LightBlue { get { return FromArgb(173, 216, 230); } }
        public static Colour Turquoise { get { return FromArgb(64, 224, 208); } }
        public static Colour LightCoral { get { return FromArgb(240, 128, 128); } }
        public static Colour Violet { get { return FromArgb(238, 130, 238); } }
        public static Colour LightCyan { get { return FromArgb(224, 255, 255); } }
        public static Colour Wheat { get { return FromArgb(245, 222, 179); } }
        public static Colour LightGoldenrodYellow { get { return FromArgb(250, 250, 210); } }
        public static Colour White { get { return FromArgb(255, 255, 255); } }
        public static Colour LightGreen { get { return FromArgb(144, 238, 144); } }
        public static Colour WhiteSmoke { get { return FromArgb(245, 245, 245); } }
        public static Colour LightGrey { get { return FromArgb(211, 211, 211); } }
        public static Colour Yellow { get { return FromArgb(255, 255, 0); } }
        public static Colour LightPink { get { return FromArgb(255, 182, 193); } }
        public static Colour YellowGreen { get { return FromArgb(154, 205, 50); } }

        public uint ARGB;

        public byte A
        {
            get
            {
                return ((byte)((ARGB >> 24) & 0xFF));
            }
            set
            {
                ARGB &= ~0xFF000000;
                ARGB |= (uint)(value << 24);
            }
        }

        public byte R
        {
            get
            {
                return ((byte)((ARGB >> 16) & 0xFF));
            }
            set
            {
                ARGB &= ~0x00FF0000U;
                ARGB |= (uint)(value << 16);
            }
        }

        public byte G
        {
            get
            {
                return ((byte)((ARGB >> 8) & 0xFF));
            }
            set
            {
                ARGB &= ~0x0000FF00U;
                ARGB |= (uint)(value << 8);
            }
        }

        public byte B
        {
            get
            {
                return ((byte)((ARGB >> 0) & 0xFF));
            }
            set
            {
                ARGB &= ~0x000000FFU;
                ARGB |= (uint)(value << 0);
            }
        }

        public static bool operator ==(Colour a, Colour b)
        {
            return Equals(a, b);
        }

        public static bool operator !=(Colour a, Colour b)
        {
            return !Equals(a, b);
        }

        public static bool Equals(Colour a, Colour b)
        {
            return a.ARGB == b.ARGB;
        }

        public uint ToArgb()
        {
            return ARGB;
        }

        public static uint ToArgb(byte r, byte g, byte b)
        {
            return (uint)(255 << 24 | r << 16 | g << 8 | b);
        }

        public static uint ToArgb(byte a, byte r, byte g, byte b)
        {
            return (uint)(a << 24 | r << 16 | g << 8 | b);
        }

        public static Colour FromArgb(byte red, byte green, byte blue)
        {
            return new Colour() { ARGB = ToArgb(red, green, blue) };
        }

        public static Colour FromArgb(byte alpha, byte red, byte green, byte blue)
        {
            return new Colour() { ARGB = ToArgb(alpha, red, green, blue) };
        }

        public static Colour FromArgb(uint argb)
        {
            return new Colour() { ARGB = argb };
        }
    }
}