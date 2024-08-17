using MOOS.Graph;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections.Generic;

namespace MOOS.Misc
{
    internal unsafe class ACFFont
    {
        /// <summary>
        /// Initializes a new ACF (Advanced Cosmos Font Format) font face.
        /// </summary>
        /// <param name="stream">The data of the ACF font file.</param>
        public ACFFont(byte[] byteArray)
        {
            _byteArray = byteArray;

            ParseMagic();
            ParseVersion();
            ParsePixelFormat();
            ParseHeight();
            ParseKerning();
            ParseMetadata();
            ParseGlyphs(); // ITS THIS ONE!
            ParseEndingMagic();
        }

        /// <summary>
        /// Check if a set of magic bytes are valid for the ACF format.
        /// </summary>
        /// <param name="magic">The magic bytes.</param>
        /// <returns>If the magic bytes are valid.</returns>
        private static bool AreMagicBytesValid(byte[] magic)
        {
            return magic[0] == 0x41 && magic[1] == 0x43 && magic[2] == 0x46;
        }

        /// <summary>
        /// Check if a set of ending magic bytes are valid for the ACF format.
        /// </summary>
        /// <param name="magic">The ending magic bytes.</param>
        /// <returns>If the ending magic bytes are valid.</returns>
        private static bool AreEndingMagicBytesValid(byte[] magic)
        {
            return magic[0] == 0x46 && magic[1] == 0x43 && magic[2] == 0x41;
        }

        /// <summary>
        /// Parse the magic bytes at the beginning of the ACF font face's stream.
        /// </summary>
        /// <exception cref="InvalidDataException">Thrown when the magic bytes are not valid.</exception>
        private void ParseMagic()
        {
            byte[] magicBuf = new byte[3];
            int length = 3;
            for (int i = _byteArrayIndex; i < _byteArrayIndex + length; i++) magicBuf[i - _byteArrayIndex] = _byteArray[i];
            _byteArrayIndex += length;

            if (!AreMagicBytesValid(magicBuf)) { } // I dunno what to do here
        }

        /// <summary>
        /// Parse the magic bytes at the end of the ACF font face's stream
        /// </summary>
        /// <exception cref="InvalidDataException">Thrown when the ending magic bytes are not valid</exception>
        private void ParseEndingMagic()
        {
            byte[] endingMagicBuf = new byte[3];
            int length = 3;
            for (int i = _byteArrayIndex; i < _byteArrayIndex + length; i++) endingMagicBuf[i - _byteArrayIndex] = _byteArray[i];
            _byteArrayIndex += length;

            if (!AreEndingMagicBytesValid(endingMagicBuf)) { } // I dunno what to do here
        }

        /// <summary>
        /// Parse the format version of the stream
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown when the version is not supported by the parser</exception>
        private void ParseVersion()
        {
            _version = _byteArray[_byteArrayIndex];
            _byteArrayIndex++;
            if (_version != 0) { } // I dunno what to do here
        }

        /// <summary>
        /// Parse the pixel format of the ACF font face
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown when the pixel format is not supported by the parser</exception>
        private void ParsePixelFormat()
        {
            _pixelFormat = _byteArray[_byteArrayIndex];
            _byteArrayIndex++;
            if (_pixelFormat != 0) { } // I dunno what to do here
        }

        /// <summary>
        /// Parse the line height of the ACF font face
        /// </summary>
        /// <exception cref="InvalidDataException">Thrown when the line height is zero</exception>
        private void ParseHeight()
        {
            _height = _byteArray[_byteArrayIndex];
            _byteArrayIndex++;
            if (_height == 0) { } // I dunno what to do here
        }

        /// <summary>
        /// Parse the kerning information of the ACF font face
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown when kerning information is in the stream,
        /// as the parser does not currently support this feature</exception>
        private void ParseKerning()
        {
            byte a = _byteArray[_byteArrayIndex];
            _byteArrayIndex++;
            byte b = _byteArray[_byteArrayIndex];
            _byteArrayIndex++;
            ushort kerningPairCount = (ushort)(a | (b << 8));
            if (kerningPairCount > 0) { } // I dunno what to do here
        }

        /*
        /// <summary>
        /// Parse a Pascal (length-prefixed) string from the data stream.
        /// </summary>
        /// <returns>The parsed string.</returns>
        private string ParsePascalString()
        {
            byte length = _byteArray[_byteArrayIndex];
            _byteArrayIndex++;
            byte[] buffer = new byte[length];
            for (int i = _byteArrayIndex; i < _byteArrayIndex + length; i++) buffer[i - _byteArrayIndex] = _byteArray[i];
            _byteArrayIndex += length;
            return Encoding.UTF8.GetString(buffer);
        }*/

        private void ParsePascalString()
        {
            byte length = _byteArray[_byteArrayIndex];
            _byteArrayIndex += length + 1;
        }

        /*
        /// <summary>
        /// Parse the ACF metadata (family and style names) from the stream.
        /// </summary>
        private void ParseMetadata()
        {
            _familyName = ParsePascalString();
            _styleName = ParsePascalString();
        }*/

        private void ParseMetadata()
        {
            ParsePascalString();
            ParsePascalString();
        }

        /// <summary>
        /// Parses the glyphs in the ACF font face from the stream.
        /// </summary>
        private void ParseGlyphs()
        {
            for (int i = 0; i < 256; i++)
                _glyphs[i] = ParseGlyph();
        }

        /// <summary>
        /// Parses a single glyph in the ACF font face from the stream.
        /// </summary>
        private Glyph ParseGlyph()
        {
            byte width = _byteArray[_byteArrayIndex];
            _byteArrayIndex++;
            byte height = _byteArray[_byteArrayIndex]; 
            _byteArrayIndex++;
            sbyte left = unchecked((sbyte)_byteArray[_byteArrayIndex]);
            _byteArrayIndex++;
            sbyte top = unchecked((sbyte)_byteArray[_byteArrayIndex]);
            _byteArrayIndex++;
            byte advanceX = _byteArray[_byteArrayIndex];
            _byteArrayIndex++;
            byte[] bitmap = new byte[width * height];

            if (bitmap.Length > 0)
            {
                int length = width * height;
                for (int i = _byteArrayIndex; i < _byteArrayIndex + length; i++) bitmap[i - _byteArrayIndex] = _byteArray[i];
                _byteArrayIndex += length;
            }

            Glyph glyph = new Glyph(left, top, width, height, bitmap);
            return glyph;
            //return null;
        }

        public string GetFamilyName() => _familyName;

        public string GetStyleName() => _styleName;

        public int GetHeight() => _height;

        public  Glyph? GetGlyph(char c)
        {
            if (c < 0 || c > 255)
            {
                return null;
            }

            return _glyphs[c];
        }

        public ushort MeasureString(string s)
        {
            ushort returnVal = 0;

            for (int i = 0; i < s.Length; i++) returnVal = (ushort)(returnVal + (GetGlyph(s[i])!.Width + 2));

            return (ushort)(returnVal + (s.Length * SpacingModifier()));
        }

        public int SpacingModifier() => 0;

        /// <summary>
        /// The array to read the ACF font face's data from.
        /// </summary>
        private readonly byte[] _byteArray;

        /// <summary>
        /// The array to read the ACF font face's data from's index.
        /// </summary>
        private int _byteArrayIndex;

        /// <summary>
        /// The format version of the ACF file.
        /// </summary>
        private byte _version;

        /// <summary>
        /// The pixel format ID of the ACF file.
        /// </summary>
        private byte _pixelFormat;

        /// <summary>
        /// The line height of the font face.
        /// </summary>
        private byte _height;

        /// <summary>
        /// The name of the font family.
        /// </summary>
        private string _familyName = string.Empty;

        /// <summary>
        /// The name of the font style.
        /// </summary>
        private string _styleName = string.Empty;

        /// <summary>
        /// The glyphs of the font face in ASCII.
        /// </summary>
        private readonly Glyph[] _glyphs = new Glyph[256];

        public void DrawChar(int x, int y, Glyph glyph, Color colour, Graphics graphics)
        {
            for (int yy = 0; yy < glyph.Height; yy++)
            {
                for (int xx = 0; xx < glyph.Width; xx++)
                {
                    int Width = Framebuffer.Graphics.Width;
                    int Height = Framebuffer.Graphics.Height;

                    uint* fb = Framebuffer.FirstBuffer;

                    // Get the alpha value of the glyph's pixel and the inverted value.
                    uint alpha = glyph.Bitmap[yy * glyph.Width + xx];
                    uint invAlpha = (uint)-alpha;

                    // Get the index of the framebuffer of where to draw the point at.
                    int canvasIdx = (y + yy - glyph.Top) * Width + x + xx;

                    // Get the background ARGB value and the glyph color's ARGB value.
                    uint backgroundArgb = graphics.GetPoint(x + xx, y + yy - glyph.Top);
                    uint glyphColorArgb = colour.ToArgb();

                    // Store the individual background color's R, G and B values.
                    byte backgroundR = (byte)((backgroundArgb >> 16) & 0xFF);
                    byte backgroundG = (byte)((backgroundArgb >> 8) & 0xFF);
                    byte backgroundB = (byte)(backgroundArgb & 0xFF);

                    // Store the individual glyph foreground color's R, G and B values.
                    byte foregroundR = (byte)((glyphColorArgb >> 16) & 0xFF);
                    byte foregroundG = (byte)((glyphColorArgb >> 8) & 0xFF);
                    byte foregroundB = (byte)((glyphColorArgb) & 0xFF);

                    // Get the individual R, G and B values for the blended color.
                    byte r = (byte)((alpha * foregroundR + invAlpha * backgroundR) >> 8);
                    byte g = (byte)((alpha * foregroundG + invAlpha * backgroundG) >> 8);
                    byte b = (byte)((alpha * foregroundB + invAlpha * backgroundB) >> 8);

                    // Store the blended color in an unsigned integer.
                    uint _colour = ((uint)255 << 24) | ((uint)r << 16) | ((uint)g << 8) | b;

                    // Set the pixel to the blended color.
                    graphics.DrawPoint(x + xx, y + yy - glyph.Top, _colour);
                }
            }
        }

        public void DrawString(int x, int y, string text, Color colour, Graphics graphics, bool centre = false)
        {
            if (string.IsNullOrEmpty(text)) return;

            if (x >= Framebuffer.Width || y >= Framebuffer.Height) return;

            string[] lines = text.Split('\n');

            int[] bx = new int[lines.Length];
            int[] by = new int[lines.Length];

            // Set temporary values.
            for (int i = 0; i < bx.Length; i++) bx[i] = x;
            for (int i = 0; i < by.Length; i++) by[i] = y + GetHeight() * i;

            // Loop though the split lines.
            for (int i = 0; i < lines.Length; i++)
            {
                // Precalculate the string's size.
                ushort TextWidth = MeasureString(lines[i]);

                // Check if the text needs to be centered.
                if (centre)
                {
                    by[i] -= GetHeight() * (lines.Length + 1) / 2;
                    bx[i] -= TextWidth / 2;
                }

                // Loop through each character in the line.
                for (int I = 0; I < lines[i].Length; I++)
                {
                    switch (lines[i][I])
                    {
                        case '\0':
                            continue;
                        case ' ':
                            bx[i] += GetHeight() / 2;
                            continue;
                        case '\t':
                            bx[i] += GetHeight() * 4;
                            continue;
                    }

                    // Get the glyph for this char.
                    Glyph? Temp = GetGlyph(lines[i][I]);

                    // Continue if the glyph for this char is null.
                    if (Temp == null)
                    {
                        continue;
                    }


                    // Draw all pixels.
                    // Draw the ACF glyph.
                    for (int yy = 0; yy < Temp.Height; yy++)
                    {
                        for (int xx = 0; xx < Temp.Width; xx++)
                        {
                            // Get the alpha value of the glyph's pixel and the inverted value.
                            uint alpha = Temp.Bitmap[yy * Temp.Width + xx];
                            uint invAlpha = alpha - 2 - (alpha * 2);

                            // Get the background ARGB value and the glyph color's ARGB value.
                            uint backgroundArgb = graphics.GetPoint(bx[i] + x + xx, by[i] + y + yy - Temp.Top);
                            uint glyphColorArgb = colour.ToArgb();

                            // Store the individual background color's R, G and B values.
                            byte backgroundR = (byte)((backgroundArgb >> 16) & 0xFF);
                            byte backgroundG = (byte)((backgroundArgb >> 8) & 0xFF);
                            byte backgroundB = (byte)(backgroundArgb & 0xFF);

                            // Store the individual glyph foreground color's R, G and B values.
                            byte foregroundR = (byte)((glyphColorArgb >> 16) & 0xFF);
                            byte foregroundG = (byte)((glyphColorArgb >> 8) & 0xFF);
                            byte foregroundB = (byte)((glyphColorArgb) & 0xFF);

                            // Get the individual R, G and B values for the blended color.
                            byte r = (byte)((alpha * foregroundR + invAlpha * backgroundR) >> 8);
                            byte g = (byte)((alpha * foregroundG + invAlpha * backgroundG) >> 8);
                            byte b = (byte)((alpha * foregroundB + invAlpha * backgroundB) >> 8);

                            // Store the blended color in an unsigned integer.
                            uint _colour = ((uint)255 << 24) | ((uint)r << 16) | ((uint)g << 8) | b;

                            // Set the pixel to the blended color.
                            graphics.DrawPoint(bx[i] + x + xx, by[i] + y + yy - Temp.Top, _colour);
                        }
                    }

                    // Offset the X position by the glyph's length.
                    bx[i] += Temp.Width + 2;
                }
            }
        }
    }

    internal class Glyph
    {
        /// <summary>
        /// Initializes a new glyph.
        /// </summary>
        /// <param name="left">The horizontal offset of the glyph's bitmap.</param>
        /// <param name="top">The vertical offset of the glyph's bitmap.</param>
        /// <param name="width">The width of the glyph's bitmap in pixels.</param>
        /// <param name="height">The height of the glyph's bitmap in pixels.</param>
        /// <param name="bitmap">The buffer of the glyph's bitmap, as an array of alpha values.</param>
        public Glyph(int left, int top, int width, int height, byte[] bitmap)
        {
            Left = left;
            Top = top;
            Width = width;
            Height = height;
            Bitmap = bitmap;
            Points = new List<(int X, int Y)>();
        }

        /// <summary>
        /// Initializes a new glyph.
        /// </summary>
        /// <param name="left">The horizontal offset of the glyph's bitmap.</param>
        /// <param name="top">The vertical offset of the glyph's bitmap.</param>
        /// <param name="width">The width of the glyph's bitmap in pixels.</param>
        /// <param name="height">The height of the glyph's bitmap in pixels.</param>
        /// <param name="points">The buffer of the glyph's bitmap, as a list of points.</param>
        public Glyph(int left, int top, int width, int height, List<(int X, int Y)> points)
        {
            Left = left;
            Top = top;
            Width = width;
            Height = height;
            Bitmap = Array.Empty<byte>();
            Points = points;
        }

        /// <summary>
        /// The horizontal offset of the glyph's bitmap.
        /// </summary>
        public readonly int Left;

        /// <summary>
        /// The vertical offset of the glyph's bitmap. Should be subtracted from the baseline.
        /// </summary>
        public readonly int Top;

        /// <summary>
        /// The width of the glyph's bitmap in pixels.
        /// </summary>
        public readonly int Width;

        /// <summary>
        /// The height of the glyph's bitmap in pixels.
        /// </summary>
        public readonly int Height;

        /// <summary>
        /// The buffer of the glyph's bitmap, as an array of alpha values.
        /// </summary>
        public readonly byte[] Bitmap;

        /// <summary>
        /// The buffer of the glyph's bitmap, as a list of points.
        /// </summary>
        public readonly List<(int X, int Y)> Points;
    }
}
