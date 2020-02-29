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
    class CharacterGroup : ListBoxItemViewModel, ICharacterGroup
    {
        private readonly CharacterContext context;
        private bool? isChecked = false;

        public CharacterGroup(CharacterContext context, string name, uint min, uint max)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.Name = name;
            this.Min = min;
            this.Max = max;
            this.Items = this.CreateItems(min, max);
            this.ActiveItems = this.Items.Where(item => item.IsEnabled).ToArray();
            this.IsVisible = this.Items.Any(item => item.TestVisible());
        }

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

        public string Name { get; }

        public override string DisplayName => $"{this.Name}";

        public uint Min { get; }

        public uint Max { get; }

        public CharacterRow[] Items { get; }

        public CharacterRow[] ActiveItems { get; }

        private CharacterRow[] CreateItems(uint min, uint max)
        {
            var i1 = min;
            var itemList = new List<CharacterRow>();
            while (i1 < max)
            {
                var i2 = Math.Min(i1 + 16, max);
                var item = new CharacterRow(this.context, i1, i2);
                itemList.Add(item);
                item.PropertyChanged += Item_PropertyChanged;
                i1 = i2;
            }
            return itemList.ToArray();
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CharacterRow.IsChecked) && sender is CharacterRow row)
            {
                var isChcked = GetChecked();
                if (this.isChecked != isChcked)
                {
                    this.isChecked = isChcked;
                    this.NotifyOfPropertyChange(nameof(IsChecked));
                }
            }

            bool? GetChecked()
            {
                var count1 = this.ActiveItems.Count(item => item.IsChecked == true);
                var count2 = this.ActiveItems.Count(item => item.IsChecked == null);
                if (count1 == this.ActiveItems.Length)
                    return true;
                else if (count1 + count2 > 0)
                    return null;
                return false;
            }
        }

        #region ICharacterGroup

        string ICharacterGroup.Name => this.Name;

        ICharacterRow[] ICharacterGroup.Items => this.Items;

        public bool ContainsListCollection => false;

        #endregion
    }
}
