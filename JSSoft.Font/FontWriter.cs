using JSSoft.Font.Serializations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

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

        public void Generate(params uint[] characters)
        {
            var query = from item in characters
                        where this.fontDescriptor.Glyphs.ContainsKey(item)
                        let glyph = this.fontDescriptor.Glyphs[item]
                        let metrics = glyph.Metrics
                        orderby metrics.Width descending
                        orderby metrics.Height descending
                        select metrics;

            var items = query.ToArray();
            var bitmap = new Bitmap(this.settings.Width, this.settings.Height);
            var graphics = Graphics.FromImage(bitmap);
            var x1 = 0;
            var y1 = 0;
            var x2 = 0;
            var y2 = 0;
            var rectangleByBitmap = new Dictionary<uint, FontGlyph>();

            graphics.FillRectangle(Brushes.Transparent, new Rectangle(0, 0, settings.Width, settings.Height));
            var i = 0;
            foreach (var item in items)
            {
                if (y1 == y2)
                {
                    y2 = y1 + item.Height;
                }
                if (x1 + item.Width >= settings.Width)
                {
                    x1 = 0;
                    y1 = y2;
                }
                var glyph = this.fontDescriptor.Glyphs[item.ID];
                if (glyph.Bitmap != null)
                    continue;

                var glyphBitmap = glyph.Bitmap;
                var glyphRect = new Rectangle(x1, y1, item.Width, item.Height);
                while (IsEmpty(bitmap, glyphRect))
                {
                    glyphRect.Y--;
                    if (glyphRect.Y < 0)
                        break;
                }
                glyphRect.Y++;
                //graphics.DrawImage(glyphBitmap, new Point(x1, y1));

                if (i % 2 == 0)
                    graphics.FillRectangle(Brushes.Red, glyphRect);
                else
                    graphics.FillRectangle(Brushes.Blue, glyphRect);
                var fontChar = new FontGlyph()
                {
                    ID = item.ID,
                    Bitmap = glyphBitmap,
                    Rectangle = glyphRect,
                    Metrics = item,
                };
                rectangleByBitmap.Add(item.ID, fontChar);
                x1 += item.Width;
                i++;
            }

            //graphics.Clear(Color.Transparent);
            foreach (var item in rectangleByBitmap)
            {
                var glyphBitmap = item.Key;
                var fontChar = item.Value;
                graphics.DrawImage(fontChar.Bitmap, fontChar.Rectangle);
            }

            bitmap.Save(@"C:\Users\s2quake\Desktop\glyph.png", ImageFormat.Png);
            var fontcharList = new List<CharSerializationInfo>(rectangleByBitmap.Count);

            foreach (var item in rectangleByBitmap)
            {
                var fontChar = item.Value;
                fontcharList.Add(new CharSerializationInfo()
                {
                    ID = (int)fontChar.ID,
                    X = fontChar.Rectangle.X,
                    Y = fontChar.Rectangle.Y,
                    Width = fontChar.Rectangle.Width,
                    Height = fontChar.Rectangle.Height,
                    XOffset = fontChar.Metrics.HorizontalBearingX,
                    YOffset = fontChar.Metrics.VerticalBearingX,
                    Chnl = 15,
                });
            }

            var f = new FontSerializationInfo()
            {
                Info = new InfoSerializationInfo()
                {
                    Face = this.fontDescriptor.Name,
                    Size = this.fontDescriptor.ItemHeight,
                    PaddingValue = (0, 0, 0, 0),
                },
                Common = new CommonSerializationInfo()
                {
                    LineHeight = this.fontDescriptor.ItemHeight,
                    ScaleW = this.settings.Width,
                    ScaleH = this.settings.Height
                },
                CharInfo = new CharsSerializationInfo()
                {
                    Count = rectangleByBitmap.Count,
                    Items = fontcharList.ToArray(),
                },
            };

            var s = new XmlSerializer(typeof(FontSerializationInfo));
            var ws = new XmlWriterSettings()
            {
                Indent = true,
                Encoding = Encoding.UTF8
            };
            using (var w = XmlWriter.Create(@"C:\Users\s2quake\Desktop\glyph.fnt", ws))
            {
                s.Serialize(w, f);
            }
        }

        private bool IsEmpty(Bitmap bitmap, Rectangle rectangle)
        {
            for (var x = 0; x < rectangle.Width; x++)
            {
                for (var y = 0; y < rectangle.Height; y++)
                {
                    var pixel = bitmap.GetPixel(x + rectangle.X, y + rectangle.Y);
                    if (pixel.A != 0 || pixel.R != 0 || pixel.G != 0 || pixel.G != 0)
                        return false;
                }
            }
            return true;
        }
    }
}
