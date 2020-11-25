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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace JSSoft.Font.ApplicationHost
{
    class CharacterGroupInfoView : IBindingList, ITypedList
    {
        private readonly PropertyDescriptorCollection properties;
        private readonly List<CharacterRowView> rowList = new List<CharacterRowView>(0x10);
        private ICharacterGroup group;

        public CharacterGroupInfoView()
        {
            var propList = new List<PropertyDescriptor>(0x10);
            for (var i = 0u; i < propList.Capacity; i++)
            {
                propList.Add(new RowPropertyDescriptor(i));
            }
            this.properties = new PropertyDescriptorCollection(propList.ToArray());
            for (var i = 0u; i < this.rowList.Capacity; i++)
            {
                this.rowList.Add(new CharacterRowView() { Row = new CharacterRow(i) });
            }
        }

        public ICharacterGroup CharacterGroup
        {
            get => this.group;
            set
            {
                this.group = value;
                this.rowList.Clear();
                this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, null));
                if (this.group != null)
                {
                    for (var i = 0; i < this.group.Items.Length; i++)
                    {
                        this.rowList.Add(new CharacterRowView() { Row = this.group.Items[i] });
                        this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, i));
                    }
                }
                else
                {
                    for (var i = 0u; i < 0x10; i++)
                    {
                        this.rowList.Add(new CharacterRowView() { Row = new CharacterRow(i) });
                        this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, (int)i));
                    }
                }
            }
        }

        public object this[int index]
        {
            get => this.rowList[index];
            set => throw new NotImplementedException();
        }

        public bool AllowNew => throw new NotImplementedException();

        public bool AllowEdit => throw new NotImplementedException();

        public bool AllowRemove => throw new NotImplementedException();

        public bool SupportsChangeNotification => true;

        public bool SupportsSearching => throw new NotImplementedException();

        public bool SupportsSorting => false;

        public bool IsSorted => throw new NotImplementedException();

        public PropertyDescriptor SortProperty => throw new NotImplementedException();

        public ListSortDirection SortDirection => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        public bool IsFixedSize => throw new NotImplementedException();

        public int Count => this.rowList.Count;

        public object SyncRoot => throw new NotImplementedException();

        public bool IsSynchronized => throw new NotImplementedException();

        public event ListChangedEventHandler ListChanged;

        public int Add(object value)
        {
            throw new NotImplementedException();
        }

        public void AddIndex(PropertyDescriptor property)
        {
            throw new NotImplementedException();
        }

        public object AddNew()
        {
            throw new NotImplementedException();
        }

        public void ApplySort(PropertyDescriptor property, ListSortDirection direction)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Array array, int index)
        {
            this.rowList.CopyTo(this.rowList.ToArray(), index);
        }

        public int Find(PropertyDescriptor property, object key)
        {
            throw new NotImplementedException();
        }

        public IEnumerator GetEnumerator()
        {
            foreach (var item in this.rowList)
            {
                yield return item;
            }
        }
        public int IndexOf(object value)
        {
            for (var i = 0; i < this.rowList.Count; i++)
            {
                var item = this.rowList[i];
                if (object.Equals(item, value) == true)
                    return i;
            }
            return -1;
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public void RemoveIndex(PropertyDescriptor property)
        {
            throw new NotImplementedException();
        }

        public void RemoveSort()
        {
            throw new NotImplementedException();
        }

        protected virtual void OnListChanged(ListChangedEventArgs e)
        {
            this.ListChanged?.Invoke(this, e);
        }

        #region ITypedList

        PropertyDescriptorCollection ITypedList.GetItemProperties(PropertyDescriptor[] listAccessors)
        {
            return this.properties;
        }

        string ITypedList.GetListName(PropertyDescriptor[] listAccessors)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region RowPropertyDescriptor

        class RowPropertyDescriptor : PropertyDescriptor
        {
            private readonly uint index;

            public RowPropertyDescriptor(uint index)
                : base($"Item{index:X}", null)
            {
                this.index = index;
            }

            public override Type ComponentType => throw new NotImplementedException();

            public override bool IsReadOnly => true;

            public override Type PropertyType => typeof(ICharacter);

            public override bool CanResetValue(object component)
            {
                throw new NotImplementedException();
            }

            public override object GetValue(object component)
            {
                if (component is CharacterRowView row)
                {
                    return row[(int)this.index];
                }
                throw new NotImplementedException();
            }

            public override void ResetValue(object component)
            {
                throw new NotImplementedException();
            }

            public override void SetValue(object component, object value)
            {
                throw new NotImplementedException();
            }

            public override bool ShouldSerializeValue(object component)
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
