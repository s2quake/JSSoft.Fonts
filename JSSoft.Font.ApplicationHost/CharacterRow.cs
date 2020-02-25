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
        private readonly CharacterContext context;
        private bool? isChecked = false;

        internal CharacterRow(uint index)
        {
            this.Index = index;
            var itemList = new List<Character>(0x10);
            for (var i = 0u; i < itemList.Capacity; i++)
            {
                itemList.Add(new Character(i)); 
            }
            this.Items = itemList.ToArray();
            this.ActiveItems = itemList.Where(item => item.IsEnabled).ToArray();
            this.IsEnabled = this.ActiveItems.Length > 0;
        }

        public CharacterRow(CharacterContext context, uint min, uint max)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            if (min >= max)
                throw new ArgumentOutOfRangeException(nameof(min));

            this.Index = (min & 0xfffffff0) >> 4;
            var itemList = new List<Character>(0x10);
            for (var i = 0u; i < itemList.Capacity; i++)
            {
                var item = new Character(this.context, i + min);
                itemList.Add(item);
                item.PropertyChanged += Item_PropertyChanged;
            }
            this.Items = itemList.ToArray();
            this.ActiveItems = itemList.Where(item => item.IsEnabled).ToArray();
            this.IsEnabled = this.ActiveItems.Length > 0;
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

        public Character[] ActiveItems { get; }

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

        public bool IsEnabled { get; }

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
            if (e.PropertyName == nameof(Character.IsChecked) && sender is Character character)
            {
                var isChecked = GetChecked();
                if (this.isChecked != isChecked)
                {
                    this.isChecked = isChecked;
                    this.NotifyOfPropertyChange(nameof(IsChecked));
                }

                bool? GetChecked()
                {
                    var count = this.ActiveItems.Count(item => item.IsChecked == true);
                    if (count == this.ActiveItems.Length)
                        return true;
                    else if (count == 0)
                        return false;
                    return null;
                }
            }
        }

        #region ICharacterRow

        ICharacter[] ICharacterRow.Items => this.Items;

        #endregion
    }
}
