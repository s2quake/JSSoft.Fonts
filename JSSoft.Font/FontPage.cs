using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font
{
    public sealed class FontPage
    {
        private readonly List<FontGlyphData> glyphList = new List<FontGlyphData>();
        private readonly bool[,] pixels;
        private readonly FontDataSettings settings;

        public FontPage(int index, string name, FontDataSettings settings)
        {
            this.Index = index;
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.settings = settings;
            this.Width = settings.Width;
            this.Height = settings.Height;
            this.pixels = new bool[settings.Width, settings.Height];

        }

        public bool Verify(FontGlyph glyph)
        {
            if (glyph.Bitmap == null)
                return false;

            var metrics = glyph.Metrics;
            var width = metrics.Width;
            var height = metrics.Height;
            if (this.HitTest(width, height) is Point)
            {
                return true;
            }

            return false;
        }

        private Point? HitTest(int width, int height)
        {
            var padding = this.settings.Padding;
            var spacing = this.settings.Spacing;
            var rect = new Rectangle(0, 0, width, height);
            for (var y = 0; y < this.Height - height; y++)
            {
                for (var x = 0; x < this.Width - width; x++)
                {
                    rect.X = x;
                    rect.Y = y;
                    if (this.IsEmpty(rect) == true)
                    {
                        var right = rect.X + width + padding.Left + padding.Right;
                        var bottom = rect.Y + height + padding.Top + padding.Bottom;
                        if (right < this.Width && bottom < this.Height)
                            return rect.Location;
                    }
                }
            }
            return null;
        }

        public void Add(FontGlyph glyph)
        {
            if (glyph.Bitmap == null)
                return;

            var padding = this.settings.Padding;
            var spacing = this.settings.Spacing;
            var metrics = glyph.Metrics;
            var width = metrics.Width;
            var height = metrics.Height;

            if (this.HitTest(width, height) is Point location)
            {
                var right = location.X + width + padding.Left + padding.Right;
                var bottom = location.Y + height + padding.Top + padding.Bottom;
                var spacingRight = Math.Min(right + spacing.Horizontal, this.Width);
                var spacingBottom = Math.Min(bottom + spacing.Vertical, this.Height);
                var rectangle = new Rectangle(location.X, location.Y, right - location.X, bottom - location.Y);
                var spacingRectangle = new Rectangle(location.X, location.Y, spacingRight - location.X, spacingBottom - location.Y);
                this.FillRectangle(spacingRectangle);
                this.glyphList.Add(new FontGlyphData(this, glyph, rectangle));
            }
        }

        public void Save(string filename)
        {
            this.Save(bitmap => bitmap.Save(filename, ImageFormat.Png));
        }

        public void Save(Stream stream)
        {
            this.Save(bitmap => bitmap.Save(stream, ImageFormat.Png));
        }

        public int Index { get; }

        public string Name { get; }

        public int Width { get; }

        public int Height { get; }

        public Color BackgroundColor { get; set; } = Color.Transparent;

        public Color ForegroundColor { get; set; } = Color.White;

        public Color PaddingColor { get; set; } = Color.Red;

        public IEnumerable<FontGlyphData> Glyphs
        {
            get
            {
                foreach (var item in this.glyphList)
                {
                    yield return item;
                }
            }
        }

        private bool IsEmpty(Rectangle rectangle)
        {
            for (var x = rectangle.Left; x < rectangle.Right; x++)
            {
                for (var y = rectangle.Top; y < rectangle.Bottom; y++)
                {
                    var pixel = this.pixels[x, y];
                    if (pixel == true)
                        return false;
                }
            }
            return true;
        }

        private void FillRectangle(Rectangle rectangle)
        {
            for (var x = rectangle.Left; x < rectangle.Right; x++)
            {
                for (var y = rectangle.Top; y < rectangle.Bottom; y++)
                {
                    this.pixels[x, y] = true;
                }
            }
        }

        private void Save(Action<Bitmap> action)
        {
            var backgroundBrush = new SolidBrush(this.BackgroundColor);
            var foregroundBrush = new SolidBrush(this.ForegroundColor);
            var paddingBrush = new SolidBrush(this.PaddingColor);
            var bitmap = new Bitmap(this.Width, this.Height);
            var graphics = Graphics.FromImage(bitmap);
            var padding = this.settings.Padding;
            graphics.FillRectangle(backgroundBrush, new Rectangle(0, 0, this.Width, this.Height));
            int i = 0;
            foreach (var item in this.glyphList)
            {
                var metrics = item.Metrics;
                var rect = new Rectangle(item.Rectangle.Left + padding.Left, item.Rectangle.Top + padding.Top, metrics.Width, metrics.Height);

                if (i++ % 2 == 0)
                    graphics.FillRectangle(paddingBrush, item.Rectangle);
                else
                    graphics.FillRectangle(Brushes.Blue, item.Rectangle);
                //graphics.DrawImage(item.Bitmap, item.Rectangle);
            }
            action(bitmap);
        }
    }
}

