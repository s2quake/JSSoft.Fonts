using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font
{
    public sealed class FontPage
    {
        private readonly List<FontGlyphData> glyphList = new List<FontGlyphData>();
        private readonly bool[,] pixels;

        public FontPage(int index, string name, int width, int height)
        {
            this.Index = index;
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.Width = width;
            this.Height = height;
            this.pixels = new bool[width, height];

        }

        public bool Verify(FontGlyph glyph)
        {
            if (glyph.Bitmap == null)
                return false;

            var metrics = glyph.Metrics;
            var width = metrics.Width;
            var height = metrics.Height;
            //var x = 0;
            //var y = 0;

            for (var y = 0; y < this.Height - height; y++)
            {
                for (var x = 0; x < this.Width - width; x++)
                {
                    if (IsEmpty(new Rectangle(x, y, width, height)))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void Add(FontGlyph glyph)
        {
            if (glyph.Bitmap == null)
                return;

            var metrics = glyph.Metrics;
            var width = metrics.Width;
            var height = metrics.Height;
            for (var y = 0; y < this.Height - height; y++)
            {
                for (var x = 0; x < this.Width - width; x++)
                {
                    var rectangle = new Rectangle(x, y, width, height);
                    if (IsEmpty(rectangle))
                    {
                        this.FillRectangle(rectangle);
                        this.glyphList.Add(new FontGlyphData(this, glyph, rectangle));
                        return;
                    }
                }
            }

            //var x = this.x + width >= this.Width ? 0 : this.x;
            //    var y = this.x + width >= this.Width ? this.y + this.lineHeight : this.y;
            //    var rectangle = this.FindRectangle(x, y, width, height);
            //    this.FillRectangle(rectangle);
            //    this.dataList.Add(new FontGlyphData(this, glyph, rectangle));
            //    this.x = rectangle.Right;
            //    this.y = rectangle.Top;
            //    this.lineHeight = Math.Max(metrics.Height, this.lineHeight);
        }

        public void Save(string filename)
        {
            var bitmap = new Bitmap(this.Width, this.Height);
            var graphics = Graphics.FromImage(bitmap);
            graphics.FillRectangle(Brushes.Black, new Rectangle(0, 0, this.Width, this.Height));
            foreach (var item in this.glyphList)
            {
                graphics.DrawImage(item.Bitmap, item.Rectangle);
            }
            bitmap.Save(filename, ImageFormat.Png);
        }

        public int Index { get; }

        public string Name { get; }

        public int Width { get; }

        public int Height { get; }

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

        private Rectangle FindRectangle(int x, int y, int width, int height)
        {
            var rectangle = new Rectangle(x, y, width, height);
            while (y >= 0 && IsEmpty(rectangle))
            {
                rectangle = new Rectangle(x, y, width, height);
                y--;
            }
            return rectangle;
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
            for (var x = 0; x < rectangle.Width; x++)
            {
                for (var y = 0; y < rectangle.Height; y++)
                {
                    this.pixels[x + rectangle.X, y + rectangle.Y] = true;
                }
            }
        }
    }
}
