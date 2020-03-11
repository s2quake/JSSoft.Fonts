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
        private bool? isChecked = false;

        internal CharacterRow(uint index)
        {
            var itemList = new List<Character>(0x10);
            for (var i = 0u; i < itemList.Capacity; i++)
            {
                itemList.Add(new Character(i));
            }
            this.Index = index;
            this.Items = itemList.ToArray();
            this.ActiveItems = itemList.Where(item => item.IsEnabled).ToArray();
            this.IsEnabled = this.ActiveItems.Length > 0;
        }

        public CharacterRow(CharacterGroup group, CharacterContext context, uint min, uint max)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (min >= max)
                throw new ArgumentOutOfRangeException(nameof(min), $"min must be less than max: '{min} < {max}'");

            var itemList = new List<Character>(0x10);
            for (var i = 0u; i < itemList.Capacity; i++)
            {
                var item = new Character(this, context, i + min);
                itemList.Add(item);
                item.PropertyChanged += Item_PropertyChanged;
            }
            this.Group = group ?? throw new ArgumentOutOfRangeException(nameof(group));
            this.Index = (min & 0xfffffff0) >> 4;
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
                        item.IsChecked = this.isChecked.Value;
                        item.PropertyChanged += Item_PropertyChanged;
                    }
                }
            }
        }

        public bool IsEnabled { get; }

        public CharacterGroup Group { get; }

        public IEnumerable<IMenuItem> MenuItems => MenuItemUtility.GetMenuItems(this, AppBootstrapperBase.Current);

        public void SetChecked(bool value)
        {
            if (this.isChecked != value)
            {
                this.isChecked = value;
                this.NotifyOfPropertyChange(nameof(IsChecked));
                foreach (var item in this.Items)
                {
                    item.PropertyChanged -= Item_PropertyChanged;
                    item.IsChecked = this.isChecked.Value;
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

        ICharacterGroup ICharacterRow.Group => this.Group;

        #endregion
    }
}
