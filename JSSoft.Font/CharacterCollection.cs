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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace JSSoft.Font
{
    [TypeConverter(typeof(CharacterCollectionConverter))]
    public class CharacterCollection : IEnumerable<uint>, IFormattable
    {
        private readonly List<uint> itemList;

        public CharacterCollection()
        {
            this.itemList = new List<uint>();
        }

        public CharacterCollection(int capacity)
        {
            this.itemList = new List<uint>(capacity);
        }

        public CharacterCollection(IEnumerable<uint> items)
        {
            this.itemList = new List<uint>(items);
        }

        /// <summary>
        /// 0-255,256
        /// </summary>
        public static CharacterCollection Parse(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));
            var items = text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var capacity = GetCapacity(items);
            var characters = new CharacterCollection(capacity);
            Enumerate(items, (item) =>
            {
                var min = item.min;
                var max = item.max;
                for (var i = min; i <= max; i++)
                {
                    characters.Add(i);
                }
            });
            return characters;
        }

        public static string ToString(uint[] characters)
        {
            return $"{new CharacterCollection(characters):X}";
        }

        public static void Validate(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));
            var items = text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            Enumerate(items, (item) =>
            {
                var min = item.min;
                var max = item.max;
                if (min > max)
                    throw new InvalidOperationException($"{Resources.Exception_MinLessMax}: '{min} < {max}'");

            });
        }

        public override string ToString()
        {
            return this.ToString(string.Empty, null);
        }

        public string ToString(string format, IFormatProvider provider)
        {
            var itemList = new List<string>();
            var items = this.OrderBy(item => item).ToArray();
            var idList = new List<uint>(this.Count);
            for (var i = 0; i < items.Length; i++)
            {
                var item = items[i];
                if (idList.Any() == false || idList.Last() + 1 == item)
                {
                    idList.Add(item);
                }
                else
                {
                    itemList.Add(ToString(idList, format, provider));
                    idList.Clear();
                    idList.Add(item);
                }
            }
            itemList.Add(ToString(idList, format, provider));
            return string.Join(",", itemList);
        }

        public void Add(uint item)
        {
            this.itemList.Add(item);
        }

        public void Insert(int index, uint item)
        {
            this.itemList.Insert(index, item);
        }

        public bool Remove(uint item)
        {
            return this.itemList.Remove(item);
        }

        public void RemoveAt(int index)
        {
            this.itemList.RemoveAt(index);
        }

        public int IndexOf(uint item)
        {
            return this.itemList.IndexOf(item);
        }

        public int Count => this.itemList.Count;

        public uint this[int index]
        {
            get => this.itemList[index];
            set => this.itemList[index] = value;
        }

        public static readonly CharacterCollection Empty = new CharacterCollection();

        private static void Enumerate(string[] items, Action<(uint min, uint max)> action)
        {
            foreach (var item in items)
            {
                if (item.IndexOf('-') >= 0)
                {
                    var ss = item.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                    var min = ParseText(ss[0]);
                    var max = ParseText(ss[1]);
                    action((min, max));
                }
                else
                {
                    var v = ParseText(item);
                    action((v, v));
                }
            }
        }

        private static int GetCapacity(string[] items)
        {
            var capacity = 0u;
            Enumerate(items, (item) =>
            {
                capacity += (item.max - item.min + 1);
            });
            return (int)capacity;
        }

        private static uint ParseText(string text)
        {
            var match = Regex.Match(text, "^0x([0-9a-fA-F]+)");
            if (match.Success == true)
            {
                return uint.Parse(match.Groups[1].Value, NumberStyles.HexNumber);
            }
            return uint.Parse(text);
        }

        private static string ToString(IEnumerable<uint> items, string format, IFormatProvider provider)
        {
            if (items.Any())
            {
                var prefix = format == "x" || format == "X" ? "0x" : string.Empty;
                if (items.Count() == 1)
                    return $"{prefix}{items.First().ToString(format, provider)}";
                return $"{prefix}{items.First().ToString(format, provider)}-{prefix}{items.Last().ToString(format, provider)}";
            }
            return string.Empty;
        }

        #region IEnumerable

        IEnumerator<uint> IEnumerable<uint>.GetEnumerator()
        {
            foreach (var item in this.itemList)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var item in this.itemList)
            {
                yield return item;
            }
        }

        #endregion
    }
}
