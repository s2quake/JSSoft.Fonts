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
    public sealed class FontData
    {
        private readonly FontDescriptor fontDescriptor;
        private readonly FontDataSettings settings;

        private FontData(FontDescriptor fontDescriptor, FontDataSettings settings)
        {
            this.fontDescriptor = fontDescriptor ?? throw new ArgumentNullException(nameof(fontDescriptor));
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        internal static FontData Create(FontDescriptor fontDescriptor, FontDataSettings settings)
        {
            var fontData = new FontData(fontDescriptor, settings)
            {
                Pages = GeneratePages(fontDescriptor, settings),
            };
            return fontData;
        }

        public void Save(string filename)
        {
            var info = (FontSerializationInfo)this;
            var serializer = new XmlSerializer(typeof(FontSerializationInfo));
            var writerSettings = new XmlWriterSettings()
            {
                Indent = true,
                Encoding = Encoding.UTF8
            };
            using (var writer = XmlWriter.Create(filename, writerSettings))
            {
                serializer.Serialize(writer, info);
            }
        }

        public void SavePages(string path)
        {
            foreach (var item in this.Pages)
            {
                var itemPath = Path.Combine(path, $"{item.Name}{item.Index}.png");
                item.Save(itemPath);
            }
        }

        public string Name => this.fontDescriptor.Name;

        public int Size => this.fontDescriptor.Size;

        public int LineHeight => this.fontDescriptor.Height;

        public int BaseLine => this.fontDescriptor.BaseLine;

        public int PageWidth => this.settings.Width;

        public int PageHeight => this.settings.Height;

        public FontPage[] Pages { get; private set; } = new FontPage[] { };

        private static FontPage[] GeneratePages(FontDescriptor fontDescriptor, FontDataSettings settings)
        {
            var characters = settings.Characters ?? new uint[] { };
            var query = from item in characters
                        where fontDescriptor.Glyphs.ContainsKey(item)
                        let glyph = fontDescriptor.Glyphs[item]
                        where glyph.Bitmap != null
                        let metrics = glyph.Metrics
                        orderby metrics.Width descending
                        orderby metrics.Height descending
                        select glyph;
            var items = query.ToArray();
            var index = 0;
            var name = fontDescriptor.Name;
            var pageList = new List<FontPage>();
            var page = new FontPage(index++, name, settings);

            pageList.Add(page);
            foreach (var item in items)
            {
                if (page.Verify(item) == false)
                {
                    page = new FontPage(index++, name, settings);
                    pageList.Add(page);
                }
                page.Add(item);
            }

            return pageList.ToArray();
        }
    }
}
