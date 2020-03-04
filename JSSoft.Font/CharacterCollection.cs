using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace JSSoft.Font
{
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
        public CharacterCollection(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));
            var items = text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var idList = new List<uint>();
            foreach (var item in items)
            {
                if (item.IndexOf('-') >= 0)
                {
                    var ss = item.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                    var min = Parse(ss[0]);
                    var max = Parse(ss[1]);
                    for (var i = min; i <= max; i++)
                    {
                        idList.Add(i);
                    }
                }
                else
                {
                    idList.Add(Parse(item));
                }
            }
            this.itemList = idList;
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
            foreach (var item in items)
            {
                if (item.IndexOf('-') >= 0)
                {
                    var ss = item.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                    var min = Parse(ss[0]);
                    var max = Parse(ss[1]);
                    if (min >= max)
                        throw new InvalidOperationException($"min must be less than max: '{item}'");
                }
                else
                {
                    Parse(item);
                }
            }
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

        public void Add(uint item) => this.itemList.Add(item);

        public void Insert(int index, uint item) => this.itemList.Insert(index, item);

        public bool Remove(uint item) => this.itemList.Remove(item);

        public void RemoveAt(int index) => this.itemList.RemoveAt(index);

        public int IndexOf(uint item) => this.itemList.IndexOf(item);

        public int Count => this.itemList.Count;

        public uint this[int index]
        {
            get => this.itemList[index];
            set => this.itemList[index] = value;
        }

        private static uint Parse(string text)
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
