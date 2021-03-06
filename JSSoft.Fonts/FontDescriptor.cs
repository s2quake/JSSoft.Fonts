﻿// MIT License
// 
// Copyright (c) 2020 Jeesu Choi
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using SharpFont;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace JSSoft.Fonts
{
    public sealed class FontDescriptor : IDisposable
    {
        public const int DefaultSize = 26;
        public const int DefaultDPI = 72;
        public const int DefaultFace = 0;

        private readonly Dictionary<uint, FontGlyph> glyphByID;
        private Library lib;
        private Face face;

        public FontDescriptor(string path, uint dpi, int size)
            : this(path, dpi, size, 0)
        {

        }

        public FontDescriptor(string path, uint dpi, int size, int faceIndex)
        {
            var pixelSize = (double)size * dpi / 72;
            var fullPath = Path.GetFullPath(path ?? throw new ArgumentNullException(nameof(path)));
            var lib = new Library();
            var face = new Face(lib, fullPath, faceIndex); face.SetCharSize(0, size, 0, dpi);
            var height = (int)Math.Round(face.Height * pixelSize / face.UnitsPerEM);
            var baseLine = height + (height * face.Descender / face.Height);

            this.glyphByID = CreateGlyphs(face, height, baseLine);
            this.lib = lib;
            this.face = face;
            this.Height = height;
            this.BaseLine = baseLine;
            this.Name = face.FamilyName;
            this.FaceIndex = faceIndex;
            this.DPI = dpi;
            this.Size = size;
            this.FontPath = path;
            this.BaseUri = new Uri(fullPath);
        }

        public static string[] GetFaces(string path)
        {
            var fullpath = Path.GetFullPath(path);
            using var lib = new Library();
            using var face = new Face(lib, fullpath, 0);
            var faceList = new List<string>() { face.FamilyName };
            for (var i = 1; i < face.FaceCount; i++)
            {
                using var childFace = new Face(lib, fullpath, i);
                faceList.Add(childFace.FamilyName);
            }
            return faceList.ToArray();
        }

        public FontData CreateData(FontDataSettings settings)
        {
            return FontData.Create(this, settings);
        }

        public void Dispose()
        {
            this.glyphByID.Clear();
            this.Height = 0;
            this.face?.Dispose();
            this.face = null;
            this.lib?.Dispose();
            this.lib = null;
        }

        public uint DPI { get; }

        public int Size { get; }

        public int Height { get; private set; }

        public int BaseLine { get; private set; }

        public string Name { get; } = string.Empty;

        public int FaceIndex { get; }

        public string FontPath { get; private set; }

        public Uri BaseUri { get; }

        public IReadOnlyDictionary<uint, FontGlyph> Glyphs => this.glyphByID;

        private static Dictionary<uint, FontGlyph> CreateGlyphs(Face face, int fontHeight, int baseLine)
        {
            var (min, max) = NamesList.Range;
            var glyphList = new List<FontGlyph>(100);
            for (var i = min; i <= max; i++)
            {
                var glyph = RegisterItem(face, i, fontHeight, baseLine);
                if (glyph != null)
                {
                    if (glyphList.Count + 1 == glyphList.Capacity)
                        glyphList.Capacity += 100;
                    glyphList.Add(glyph);
                }
            }
            return glyphList.ToDictionary(item => item.ID);
        }

        private static FontGlyph RegisterItem(Face face, uint charCode, int fontHeight, int baseLine)
        {
            var glyph = CreateGlyph(face, charCode);
            if (glyph == null)
                return null;

            var ftbmp = glyph.Bitmap;
            var metrics = glyph.Metrics;
            var glyphMetrics = new GlyphMetrics()
            {
                ID = charCode,
                Width = (int)metrics.Width,
                Height = (int)metrics.Height,
                HorizontalBearingX = (int)metrics.HorizontalBearingX,
                HorizontalBearingY = (int)metrics.HorizontalBearingY,
                HorizontalAdvance = (int)metrics.HorizontalAdvance,
                VerticalBearingX = (int)metrics.VerticalBearingX,
                VerticalBearingY = (int)metrics.VerticalBearingY,
                VerticalAdvance = (int)metrics.VerticalAdvance,
                FontHeight = fontHeight,
                BaseLine = baseLine,
            };
            return new FontGlyph()
            {
                ID = charCode,
                Bitmap = CreateBitmap(ftbmp, charCode),
                Metrics = glyphMetrics,
            };
        }

        private static Bitmap CreateBitmap(FTBitmap ftbmp, uint charCode)
        {
            if (ftbmp.Rows > 0 && ftbmp.Width > 0)
            {
                return FontBitmapConverter.Convert(ftbmp, Color.White, charCode);
            }
            return null;
        }

        private static GlyphSlot CreateGlyph(Face face, uint charCode)
        {
            var index = face.GetCharIndex(charCode);
            if (index == 0)
                return null;
            try
            {
                face.LoadGlyph(index, LoadFlags.Default, LoadTarget.Normal);
            }
            catch
            {
                return null;
            }
            face.Glyph.RenderGlyph(RenderMode.Normal);
            return face.Glyph;
        }
    }
}
