using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font
{
    public class FontWriter
    {
        private readonly FontDescriptor fontDescriptor;
        private readonly FontWriterSettings settings;

        public FontWriter(FontDescriptor fontDescriptor, FontWriterSettings settings)
        {
            this.fontDescriptor = fontDescriptor ?? throw new ArgumentNullException(nameof(fontDescriptor));
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public void Generate( params uint[] characters)
        {
            var query = from item in characters
                        where this.fontDescriptor.Metrics.ContainsKey(item)
                        let metric = this.fontDescriptor.Metrics[item]
                        orderby metric.Width descending
                        orderby metric.Height descending
                        select metric;

            var items = query.ToArray();

            var bitmap = new Bitmap(settings.Width, settings.Height);
            var graphics = Graphics.FromImage(bitmap);
            var x1 = 0;
            var y1 = 0;
            var x2 = 0;
            var y2 = 0;
            var emptyList = new List<Rectangle>();
            foreach (var item in items)
            {
                if (y1 == y2)
                {
                    y2 = y1 + item.Height;
                }
                if (x1+ item.Width >= settings.Width)
                {
                    var emptyRectangle1 = new Rectangle(x1, y1, settings.Width - x1, y2 - y1);
                    emptyList.Add(emptyRectangle1);
                    x1 = 0;
                    y1 = y2;
                }
                if (this.fontDescriptor.Bitmaps.ContainsKey(item.ID) == false)
                    continue;

                var glyphBitmap = this.fontDescriptor.Bitmaps[item.ID];
                var glyphRect = new Rectangle(x1, y1, item.Width, item.Height);
                graphics.DrawImage(glyphBitmap, new Point(x1, y1));
                var emptyRectangle = new Rectangle(x1, y1 + item.Height, item.Width, y2 - y1 - item.Height);
                x1 += item.Width;
            }
            bitmap.Save(@"C:\Users\s2quake\Desktop\glyph.png", ImageFormat.Png);
        }
    }
}
