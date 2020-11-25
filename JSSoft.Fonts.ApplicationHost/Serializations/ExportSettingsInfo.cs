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

using System.Linq;
using System.Xml.Serialization;

namespace JSSoft.Fonts.ApplicationHost.Serializations
{
    [XmlRoot]
    public struct ExportSettingsInfo
    {
        [XmlElement]
        public string Font { get; set; }

        [XmlElement]
        public int Face { get; set; }

        [XmlElement]
        public int Size { get; set; }

        [XmlElement]
        public int DPI { get; set; }

        [XmlElement]
        public int TextureWidth { get; set; }

        [XmlElement]
        public int TextureHeight { get; set; }

        [XmlIgnore]
        public FontPadding Padding { get; set; }

        [XmlIgnore]
        public FontSpacing Spacing { get; set; }

        [XmlIgnore]
        public uint[] Characters { get; set; }

        [XmlElement]
        public string PaddingValue
        {
            get => $"{this.Padding.Top},{this.Padding.Right},{this.Padding.Bottom},{this.Padding.Left}";
            set
            {
                var items = value.Split(',');
                this.Padding = new FontPadding(int.Parse(items[0]), int.Parse(items[1]), int.Parse(items[2]), int.Parse(items[3]));
            }
        }

        [XmlElement]
        public string SpacingValue
        {
            get => $"{this.Spacing.Vertical},{this.Spacing.Horizontal}";
            set
            {
                var items = value.Split(',');
                this.Spacing = new FontSpacing(int.Parse(items[0]), int.Parse(items[1]));
            }
        }

        [XmlElement]
        public string CharactersValue
        {
            get => $"{new CharacterCollection(this.Characters):X}";
            set => this.Characters = CharacterCollection.Parse(value).ToArray();
        }

        internal static ExportSettingsInfo Create(ShellViewModel shell)
        {
            var settings = shell.Settings;
            var fontDescriptor = shell.FontDescriptor;
            var query = from characterGroup in shell.Groups
                        where characterGroup.IsChecked != false
                        from row in characterGroup.Items
                        where row.IsChecked != false
                        from item in row.Items
                        where item.IsChecked && item.IsEnabled
                        select item.ID;

            return new ExportSettingsInfo()
            {
                Font = fontDescriptor.FontPath,
                Face = fontDescriptor.FaceIndex,
                Size = fontDescriptor.Size,
                DPI = (int)fontDescriptor.DPI,
                TextureWidth = settings.TextureWidth,
                TextureHeight = settings.TextureHeight,
                Padding = settings.PaddingValue,
                Spacing = settings.SpacingValue,
                Characters = query.ToArray(),
            };
        }
    }
}
