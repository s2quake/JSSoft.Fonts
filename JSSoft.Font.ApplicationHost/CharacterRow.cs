using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font.ApplicationHost
{
    class CharacterRow : PropertyChangedBase, ICharacterRow
    {
        private readonly FontDescriptor fontDescriptor;
        private bool? isChecked;

        internal CharacterRow(uint index)
        {
            this.Index = index;
            var itemList = new List<Character>(0x10);
            for (var i = 0u; i < itemList.Capacity; i++)
            {
                itemList.Add(new Character(i));
            }
            this.Items = itemList.ToArray();
        }

        public CharacterRow(FontDescriptor fontDescriptor, uint min, uint max)
        {
            this.fontDescriptor = fontDescriptor ?? throw new ArgumentNullException(nameof(fontDescriptor));
            if (min >= max)
                throw new ArgumentOutOfRangeException(nameof(min));

            this.Index = (min & 0xfffffff0) >> 4;
            var itemList = new List<Character>(0x10);
            for (var i = 0u; i < itemList.Capacity; i++)
            {
                var item = new Character(this.fontDescriptor, i + min);
                itemList.Add(item);
                item.PropertyChanged += Item_PropertyChanged;
            }
            this.Items = itemList.ToArray();
        }

        public bool TestVisible()
        {
            foreach (var item in this.Items)
            {
                if (item.IsEnabled == true)
                    return true;
            }
            return false;
        }

        public Character[] Items { get; }

        public uint Index { get; }

        public bool? IsChecked
        {
            get => this.isChecked;
            set
            {
                if (this.isChecked != value)
                {
                    this.isChecked = value ?? false;
                    this.NotifyOfPropertyChange(nameof(IsChecked));
                    foreach (var item in this.Items)
                    {
                        item.PropertyChanged -= Item_PropertyChanged;
                        item.SetChecked(this.isChecked.Value);
                        item.PropertyChanged += Item_PropertyChanged;
                    }
                }
            }
        }

        public void SetChecked(bool value)
        {
            if (this.isChecked != value)
            {
                this.isChecked = value;
                this.NotifyOfPropertyChange(nameof(IsChecked));
                foreach (var item in this.Items)
                {
                    item.PropertyChanged -= Item_PropertyChanged;
                    item.SetChecked(this.isChecked.Value);
                    item.PropertyChanged += Item_PropertyChanged;
                }
            }
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Character.IsChecked))
            {
                this.isChecked = null;
                this.NotifyOfPropertyChange(nameof(IsChecked));
            }
        }

        #region ICharacterRow

        ICharacter[] ICharacterRow.Items => this.Items;

        #endregion
    }
}
