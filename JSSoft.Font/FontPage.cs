// MIT License
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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace JSSoft.Font
{
    public sealed class FontPage
    {
        private readonly FontDataSettings settings;
        private readonly FontNode node;

        public FontPage(int index, FontDataSettings settings)
        {
            this.Index = index;
            this.settings = settings;
            this.Name = settings.Name;
            this.Width = settings.Width;
            this.Height = settings.Height;
            this.node = FontNode.Create(this, settings);
        }

        public FontGlyphData HitTest(Point point)
        {
            return this.node.HitTest(point);
        }

        public IReservator ReserveRegion(FontGlyph glyph)
        {
            return this.node.ReserveRegion(glyph);
        }

        public void Save(string filename)
        {
            this.Save(bitmap => bitmap.Save(filename, ImageFormat.Png));
        }

        public void Save(Stream stream)
        {
            this.Save(bitmap => bitmap.Save(stream, ImageFormat.Png));
        }

        public static Color DefaultBackgroundColor { get; } = Color.Transparent;

        public static Color DefaultForegroundColor { get; } = Color.White;

        public static Color DefaultPaddingColor { get; } = Color.Transparent;

        public int Index { get; }

        public string Name { get; }

        public int Width { get; }

        public int Height { get; }

        public Color BackgroundColor { get; set; } = DefaultBackgroundColor;

        public Color ForegroundColor { get; set; } = DefaultForegroundColor;

        public Color PaddingColor { get; set; } = DefaultPaddingColor;

        public IEnumerable<FontGlyphData> Glyphs
        {
            get
            {
                foreach (var item in this.node.GlyphList)
                {
                    yield return item;
                }
            }
        }

        internal Rectangle GeneratePaddingRectangle(Rectangle rectangle)
        {
            var padding = this.settings.Padding;
            var l = rectangle.Left - padding.Left;
            var t = rectangle.Top - padding.Top;
            var r = rectangle.Right + padding.Right;
            var b = rectangle.Bottom + padding.Bottom;
            return Rectangle.FromLTRB(l, t, r, b);
        }

        internal Rectangle GenerateSpacingRectangle(Rectangle rectangle)
        {
            var padding = this.settings.Padding;
            var spacing = this.settings.Spacing;
            var l = rectangle.Left - padding.Left;
            var t = rectangle.Top - padding.Top;
            var r = rectangle.Right + padding.Right;
            var b = rectangle.Bottom + padding.Bottom;
            if (r + spacing.Horizontal < this.Width)
                r += spacing.Horizontal;
            if (b + spacing.Vertical < this.Height)
                b += spacing.Vertical;
            return Rectangle.FromLTRB(l, t, r, b);
        }

        private void Save(Action<Bitmap> action)
        {
            var backgroundBrush = new SolidBrush(this.BackgroundColor);
            var paddingBrush = new SolidBrush(this.PaddingColor);
            var bitmap = new Bitmap(this.Width, this.Height);
            var graphics = Graphics.FromImage(bitmap);

            graphics.CompositingMode = CompositingMode.SourceCopy;
            foreach (var item in this.node.GlyphList)
            {
                var glyphBitmap = this.CloneBitmap(item.Bitmap, this.ForegroundColor);
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.FillRectangle(paddingBrush, item.PaddingRectangle);
                graphics.FillRectangle(backgroundBrush, item.Rectangle);
                graphics.CompositingMode = CompositingMode.SourceOver;
                graphics.DrawImage(glyphBitmap, item.Rectangle);
                glyphBitmap.Dispose();
            }
            action(bitmap);
        }

        private Bitmap CloneBitmap(Bitmap bitmap, Color color)
        {
            lock (bitmap)
            {
                var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                var bitmap2 = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb);
                var data1 = bitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                var data2 = bitmap2.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                var bytes1 = new byte[Math.Abs(data1.Stride) * data1.Height];
                var bytes2 = new byte[Math.Abs(data2.Stride) * data2.Height];

                Marshal.Copy(data1.Scan0, bytes1, 0, bytes1.Length);
                for (var y = 0; y < bitmap.Height; y++)
                {
                    for (var x = 0; x < bitmap.Width; x++)
                    {
                        var b = bytes1[x * 4 + y * data1.Stride + 0];
                        var g = bytes1[x * 4 + y * data1.Stride + 1];
                        var r = bytes1[x * 4 + y * data1.Stride + 2];
                        var a = bytes1[x * 4 + y * data1.Stride + 3];

                        var bf = ((float)b / 255) * ((float)color.B / 255);
                        var gf = ((float)g / 255) * ((float)color.G / 255);
                        var rf = ((float)r / 255) * ((float)color.R / 255);
                        var af = ((float)a / 255) * ((float)color.A / 255);

                        bytes2[x * 4 + y * data2.Stride + 0] = (byte)(bf * 255.0f);
                        bytes2[x * 4 + y * data2.Stride + 1] = (byte)(gf * 255.0f);
                        bytes2[x * 4 + y * data2.Stride + 2] = (byte)(rf * 255.0f);
                        bytes2[x * 4 + y * data2.Stride + 3] = (byte)(af * 255.0f);
                    }
                }
                Marshal.Copy(bytes2, 0, data2.Scan0, bytes2.Length);
                bitmap2.UnlockBits(data2);
                bitmap.UnlockBits(data1);
                return bitmap2;
            }
        }
    }
}
