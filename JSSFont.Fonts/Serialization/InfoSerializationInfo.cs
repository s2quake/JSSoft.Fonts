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

using System.Xml.Serialization;

namespace JSSoft.Font.Serializations
{
    public struct InfoSerializationInfo
    {
        [XmlAttribute("face")]
        public string Face { get; set; }

        [XmlAttribute("size")]
        public int Size { get; set; }

        [XmlAttribute("bold")]
        public int Bold { get; set; }

        [XmlAttribute("italic")]
        public int Italic { get; set; }

        [XmlAttribute("charset")]
        public string Charset { get; set; }

        [XmlAttribute("unicode")]
        public int Unicode { get; set; }

        [XmlAttribute("stretchH")]
        public int StretchH { get; set; }

        [XmlAttribute("smooth")]
        public int Smooth { get; set; }

        [XmlAttribute("aa")]
        public int Aa { get; set; }

        [XmlAttribute("padding")]
        public string Padding
        {
            get => $"{this.PaddingValue}";
            set => this.PaddingValue = FontPadding.Parse(value);
        }

        [XmlAttribute("spacing")]
        public string Spacing
        {
            get => $"{this.SpacingValue}";
            set => this.SpacingValue = FontSpacing.Parse(value);
        }

        [XmlAttribute("outline")]
        public int Outline { get; set; }

        public static explicit operator InfoSerializationInfo(FontData fontData)
        {
            return new InfoSerializationInfo()
            {
                Face = fontData.Name,
                Size = fontData.Size,
                Bold = 0,
                Italic = 0,
                Charset = string.Empty,
                Unicode = 1,
                StretchH = 100,
                Smooth = 1,
                Aa = 1,
                PaddingValue = fontData.Padding,
                SpacingValue = fontData.Spacing,
                Outline = 0,
            };
        }

        [XmlIgnore]
        public FontPadding PaddingValue { get; set; }

        [XmlIgnore]
        public FontSpacing SpacingValue { get; set; }
    }
}
