// MIT License
// 
// Copyright (c) 2019 Jeesu Choi
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
using System.Security;
using System.Xml.Serialization;

namespace JSSoft.Font.Serializations
{
    public struct CharSerializationInfo
    {
        [XmlAttribute("id")]
        public int ID { get; set; }

        [XmlAttribute("x")]
        public int X { get; set; }

        [XmlAttribute("y")]
        public int Y { get; set; }

        [XmlAttribute("width")]
        public int Width { get; set; }

        [XmlAttribute("height")]
        public int Height { get; set; }

        [XmlAttribute("xoffset")]
        public int XOffset { get; set; }

        [XmlAttribute("yoffset")]
        public int YOffset { get; set; }

        [XmlAttribute("xadvance")]
        public int XAdvance { get; set; }

        [XmlAttribute("page")]
        public int Page { get; set; }

        [XmlAttribute("chnl")]
        public int Chnl { get; set; }

        public static explicit operator CharSerializationInfo(FontGlyphData glyphData)
        {
            return new CharSerializationInfo()
            {
                ID = (int)glyphData.ID,
                X = glyphData.Rectangle.X,
                Y = glyphData.Rectangle.Y,
                Width = glyphData.Rectangle.Width,
                Height = glyphData.Rectangle.Height,
                XOffset = glyphData.Metrics.HorizontalBearingX,
                YOffset = glyphData.Metrics.BaseLine - glyphData.Metrics.HorizontalBearingY,
                XAdvance = glyphData.Metrics.HorizontalAdvance,
                Page = glyphData.Page.Index,
            };
        }
    }
}
