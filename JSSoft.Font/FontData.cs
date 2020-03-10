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

using JSSoft.Font.Serializations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
                var itemPath = Path.Combine(path, $"{item.Name}_{item.Index}.png");
                item.Save(itemPath);
            }
        }

        public string Name => this.fontDescriptor.Name;

        public int Size => this.fontDescriptor.Size;

        public int LineHeight => this.fontDescriptor.Height;

        public int BaseLine => this.fontDescriptor.BaseLine;

        public int PageWidth => this.settings.Width;

        public int PageHeight => this.settings.Height;

        public FontPadding Padding => this.settings.Padding;

        public FontSpacing Spacing => this.settings.Spacing;

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
            var pageList = new List<FontPage>();
            var page = new FontPage(index++, settings);

            pageList.Add(page);
            foreach (var item in items)
            {
                var reservator = page.ReserveRegion(item);
                if (reservator == null)
                {
                    page = new FontPage(index++, settings);
                    pageList.Add(page);
                    reservator = page.ReserveRegion(item);
                }
                try
                {
                    reservator.Reserve();
                }
                catch
                {
                    reservator.Reject();
                }
            }

            return pageList.ToArray();
        }
    }
}
