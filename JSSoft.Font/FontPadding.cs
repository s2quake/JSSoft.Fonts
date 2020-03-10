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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font
{
    [TypeConverter(typeof(FontPaddingConverter))]
    public struct FontPadding
    {
        public FontPadding(int left, int top, int right, int bottom)
        {
            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
        }

        public FontPadding(int thickness)
        {
            this.Left = thickness;
            this.Top = thickness;
            this.Right = thickness;
            this.Bottom = thickness;
        }

        public override string ToString()
        {
            return $"{this.Left},{this.Top},{this.Right},{this.Bottom}";
        }

        public static FontPadding Parse(string s)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            var items = s.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            var values = items.Select(item => int.Parse(item)).ToArray();
            switch (values.Length)
            {
                case 1:
                    {
                        return new FontPadding(values.First());
                    }
                case 2:
                    {
                        var value0 = values[0];
                        var value1 = values[1];
                        return new FontPadding(value0, value1, value0, value1);
                    }
                case 4:
                    {
                        var value0 = values[0];
                        var value1 = values[1];
                        var value2 = values[2];
                        var value3 = values[3];
                        return new FontPadding(value0, value1, value2, value3);
                    }
                default:
                    throw new FormatException($"invalid format: \"{s}\"");
            }
        }

        public static bool TryParse(string s, out FontPadding result)
        {
            if (s == null)
            {
                result = FontPadding.Empty;
                return false;
            }
            var items = s.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            var valueList = new List<int>(items.Length);
            for (var i = 0; i < items.Length; i++)
            {
                if (int.TryParse(items[i], out var v) == true)
                {
                    valueList.Add(v);
                }
                else
                {
                    result = FontPadding.Empty;
                    return false;
                }
            }
            switch (valueList.Count)
            {
                case 1:
                    {
                        result = new FontPadding(valueList[0]);
                        return true;
                    }
                case 2:
                    {
                        var value0 = valueList[0];
                        var value1 = valueList[1];
                        result = new FontPadding(value0, value1, value0, value1);
                        return true;
                    }
                case 4:
                    {
                        var value0 = valueList[0];
                        var value1 = valueList[1];
                        var value2 = valueList[2];
                        var value3 = valueList[3];
                        result = new FontPadding(value0, value1, value2, value3);
                        return true;
                    }
                default:
                    {
                        result = FontPadding.Empty;
                        return false;
                    }
            }
        }

        public int Left { get; set; }

        public int Top { get; set; }

        public int Right { get; set; }

        public int Bottom { get; set; }

        public static readonly FontPadding Empty = new FontPadding();

        internal int Horizontal => this.Left + this.Right;

        internal int Vertical => this.Top + this.Bottom;
    }
}
