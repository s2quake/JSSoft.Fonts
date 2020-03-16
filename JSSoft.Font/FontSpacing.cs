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

using JSSoft.Font.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace JSSoft.Font
{
    [TypeConverter(typeof(FontSpacingConverter))]
    public struct FontSpacing
    {
        public FontSpacing(int horizontal, int vertical)
        {
            this.Horizontal = horizontal;
            this.Vertical = vertical;
        }

        public static FontSpacing Parse(string s)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            var items = s.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            var values = items.Select(item => int.Parse(item)).ToArray();
            switch (values.Length)
            {
                case 1:
                    {
                        var value0 = values[0];
                        return new FontSpacing(value0, value0);
                    }
                case 2:
                    {
                        var value0 = values[0];
                        var value1 = values[1];
                        return new FontSpacing(value0, value1);
                    }
                default:
                    throw new FormatException($"{Resources.Exception_InvalidFormat}: \"{s}\"");
            }
        }

        public static bool TryParse(string s, out FontSpacing result)
        {
            if (s == null)
            {
                result = FontSpacing.Empty;
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
                    result = FontSpacing.Empty;
                    return false;
                }
            }
            switch (valueList.Count)
            {
                case 1:
                    {
                        var value0 = valueList[0];
                        result = new FontSpacing(value0, value0);
                        return true;
                    }
                case 2:
                    {
                        var value0 = valueList[0];
                        var value1 = valueList[1];
                        result = new FontSpacing(value0, value1);
                        return true;
                    }
                default:
                    {
                        result = FontSpacing.Empty;
                        return false;
                    }
            }
        }

        public int Horizontal { get; set; }

        public int Vertical { get; set; }

        public static readonly FontSpacing Empty = new FontSpacing();
    }
}
