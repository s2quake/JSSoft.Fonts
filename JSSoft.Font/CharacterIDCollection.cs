using Ntreev.Library;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font
{
    public class CharacterIDCollection : IEnumerable<uint>
    {
        private readonly List<uint> itemList;

        public CharacterIDCollection()
        {
            this.itemList = new List<uint>();
        }

        public CharacterIDCollection(int capacity)
        {
            this.itemList = new List<uint>(capacity);
        }

        public CharacterIDCollection(IEnumerable<uint> items)
        {
            this.itemList = new List<uint>(items);
        }

        /// <summary>
        /// 0-255,256
        /// </summary>
        public CharacterIDCollection(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));
            var items = text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var idList = new List<uint>();
            foreach (var item in items)
            {
                if (item.IndexOf('-') >= 0)
                {
                    var ss = StringUtility.Split(item, '-');
                    var min = uint.Parse(ss[0]);
                    var max = uint.Parse(ss[1]);
                    for (var i = min; i <= max; i++)
                    {
                        idList.Add(i);
                    }
                }
                else
                {
                    idList.Add(uint.Parse(item));
                }
            }
            this.itemList = idList;
        }

        public override string ToString()
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
                    itemList.Add(ToString(idList));
                    idList.Clear();
                    idList.Add(item);
                }
            }
            itemList.Add(ToString(idList));
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

        private static string ToString(IEnumerable<uint> items)
        {
            if (items.Any())
            {
                if (items.Count() == 1)
                    return $"{items.First()}";
                return $"{items.First()}-{items.Last()}";
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
