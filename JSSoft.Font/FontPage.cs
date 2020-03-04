using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public IReservator ReserveRegion(FontGlyph glyph) => this.node.ReserveRegion(glyph);

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

        private void Save(Action<Bitmap> action)
        {
            var backgroundBrush = new SolidBrush(this.BackgroundColor);
            var paddingBrush = new SolidBrush(this.PaddingColor);
            var bitmap = new Bitmap(this.Width, this.Height);
            var graphics = Graphics.FromImage(bitmap);
            var padding = this.settings.Padding;

            graphics.CompositingMode = CompositingMode.SourceCopy;
            foreach (var item in this.node.GlyphList)
            {
                var metrics = item.Metrics;
                var glyphBitmap = this.CloneBitmap(item.Bitmap, this.ForegroundColor);
                var rect = new Rectangle(item.Rectangle.Left, item.Rectangle.Top, metrics.Width, metrics.Height);
                var paddingRect = Rectangle.FromLTRB(item.Rectangle.Left - padding.Left, item.Rectangle.Top - padding.Top, item.Rectangle.Right + padding.Right, item.Rectangle.Bottom + padding.Bottom);
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.FillRectangle(paddingBrush, paddingRect);
                graphics.FillRectangle(backgroundBrush, rect);
                graphics.CompositingMode = CompositingMode.SourceOver;
                graphics.DrawImage(glyphBitmap, rect);
                glyphBitmap.Dispose();
            }
            action(bitmap);
        }

        private Bitmap CloneBitmap(Bitmap bitmap, Color color)
        {
            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            var newBitmap = bitmap.Clone(rect, PixelFormat.Format32bppArgb) as Bitmap;
            var data = newBitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            var ptr = data.Scan0;
            var length = Math.Abs(data.Stride) * data.Height;
            var bytes = new byte[length];

            System.Runtime.InteropServices.Marshal.Copy(ptr, bytes, 0, length);
            for (var y = 0; y < bitmap.Height; y++)
            {
                for (var x = 0; x < bitmap.Width; x++)
                {
                    byte b = bytes[x * 4 + y * data.Stride + 0];
                    byte g = bytes[x * 4 + y * data.Stride + 1];
                    byte r = bytes[x * 4 + y * data.Stride + 2];
                    byte a = bytes[x * 4 + y * data.Stride + 3];

                    float bf = ((float)b / 255) * ((float)color.B / 255);
                    float gf = ((float)g / 255) * ((float)color.G / 255);
                    float rf = ((float)r / 255) * ((float)color.R / 255);
                    float af = ((float)a / 255) * ((float)color.A / 255);

                    bytes[x * 4 + y * data.Stride + 0] = (byte)(bf * 255.0f);
                    bytes[x * 4 + y * data.Stride + 1] = (byte)(gf * 255.0f);
                    bytes[x * 4 + y * data.Stride + 2] = (byte)(rf * 255.0f);
                    bytes[x * 4 + y * data.Stride + 3] = (byte)(af * 255.0f);
                }
            }
            System.Runtime.InteropServices.Marshal.Copy(bytes, 0, ptr, length);
            newBitmap.UnlockBits(data);
            return newBitmap;
        }
    }
}

