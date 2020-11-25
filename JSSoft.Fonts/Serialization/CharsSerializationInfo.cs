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

using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace JSSoft.Fonts.Serializations
{
    public class CharsSerializationInfo
    {
        [XmlAttribute("count")]
        public int Count { get; set; }

        [XmlElement("char")]
        public CharSerializationInfo[] Items { get; set; }

        public static explicit operator CharsSerializationInfo(FontData fontData)
        {
            var charList = new List<FontGlyphData>();
            foreach (var item in fontData.Pages)
            {
                charList.AddRange(item.Glyphs);
            }
            charList.Sort((x, y) => x.ID.CompareTo(y.ID));

            return new CharsSerializationInfo()
            {
                Count = charList.Count,
                Items = charList.Select(item => (CharSerializationInfo)item).ToArray(),
            };
        }
    }
}
