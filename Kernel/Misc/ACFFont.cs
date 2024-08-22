﻿using MOOS.Graph;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections.Generic;

namespace MOOS.Misc
{
    internal unsafe class ACFFont
    {
        /// <summary>
        /// Initialises a new ACF (Advanced Cosmos Font Format) font face.
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
            ParseGlyphs();
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

        public int MeasureStringVert(string s)
        {
            int topHeight = 0;
            for (int I = 0; I < s.Length; I++)
            {
                Glyph? Temp = GetGlyph(s[I]);
                if (Temp != null && Temp.Height > topHeight)
                {
                    topHeight = Temp.Height;
                }
            }

            return topHeight;
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

        public void DrawString(int x, int y, string text, uint colour, Graphics graphics, bool centre = false)
        {
            if (string.IsNullOrEmpty(text) || x >= Framebuffer.Width || y >= Framebuffer.Height) return;

            string[] lines = text.Split('\n');
            int lineHeight = GetHeight();
            int totalHeight = lineHeight * lines.Length;

            for (int i = 0; i < lines.Length; i++)
            {
                int bx = 0;
                int by = lineHeight * i - (centre ? totalHeight / 2 : 0);
                ushort textWidth = MeasureString(lines[i]);

                if (centre) bx -= textWidth / 2;

                int topHeight = 0;
                for (int j = 0; j < lines[i].Length; j++)
                {
                    Glyph? glyph = GetGlyph(lines[i][j]);
                    if (glyph != null && glyph.Height > topHeight) topHeight = glyph.Height;
                }

                for (int j = 0; j < lines[i].Length; j++)
                {
                    char c = lines[i][j];
                    if (c == '\0') continue;
                    if (c == ' ') { bx += lineHeight / 2; continue; }
                    if (c == '\t') { bx += lineHeight * 4; continue; }

                    Glyph? glyph = GetGlyph(c);
                    if (glyph == null) continue;

                    for (int yy = 0; yy < glyph.Height; yy++)
                    {
                        for (int xx = 0; xx < glyph.Width; xx++)
                        {
                            uint alpha = glyph.Bitmap[yy * glyph.Width + xx];
                            uint _colour = (alpha << 24) | (colour & 0xFFFFFF);
                            graphics.DrawPoint(bx + x + xx, by + y + yy + topHeight - glyph.Top, _colour, true);
                        }
                    }

                    bx += glyph.Width + 2;
                }
            }
        }
    }

    internal class Glyph
    {
        /// <summary>
        /// Initialises a new glyph.
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
        /// Initialises a new glyph.
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
