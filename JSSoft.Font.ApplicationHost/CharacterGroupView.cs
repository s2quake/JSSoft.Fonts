using Ntreev.ModernUI.Framework.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font.ApplicationHost
{
    class CharacterGroupView : IBindingList, ITypedList
    {
        private readonly ICharacterGroup group;
        private readonly ICharacterRow[] rows;
        private readonly PropertyDescriptorCollection properties;

        public CharacterGroupView(ICharacterGroup group)
        {
            this.group = group;
            this.rows = group.Items;

            var propList = new List<PropertyDescriptor>(0x10);
            for (var i = 0u; i < propList.Capacity; i++)
            {
                propList.Add(new RowPropertyDescriptor(i));
            }
            this.properties = new PropertyDescriptorCollection(propList.ToArray());
        }

        public object this[int index]
        {
            get => this.rows[index];
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

        public int Count => this.rows.Length;

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
            this.rows.CopyTo(array, index);
        }

        public int Find(PropertyDescriptor property, object key)
        {
            throw new NotImplementedException();
        }

        public IEnumerator GetEnumerator()
        {
            foreach (var item in this.rows)
            {
                yield return item;
            }
        }
        public int IndexOf(object value)
        {
            for (var i = 0; i < this.rows.Length; i++)
            {
                var item = this.rows[i];
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
                if (component is ICharacterRow row)
                {
                    return row.Items[this.index];
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
