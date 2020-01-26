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
        private readonly FontDescriptor fontDescriptor;
        private readonly string name;
        private bool? isChecked = false;

        public CharacterGroup(FontDescriptor fontDescriptor, string name, uint min, uint max)
        {
            this.fontDescriptor = fontDescriptor ?? throw new ArgumentNullException(nameof(fontDescriptor));
            this.name = name;
            this.Items = this.CreateItems(min, max);
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

        public override string DisplayName => this.name;

        public CharacterRow[] Items { get; }

        private CharacterRow[] CreateItems(uint min, uint max)
        {
            var i1 = min;
            var itemList = new List<CharacterRow>();
            while (i1 < max)
            {
                var i2 = Math.Min(i1 + 16, max);
                var item = new CharacterRow(this.fontDescriptor, i1, i2);
                itemList.Add(item);
                item.PropertyChanged += Item_PropertyChanged;
                i1 = i2;
            }
            return itemList.ToArray();
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Character.IsChecked))
            {
                this.isChecked = null;
                this.NotifyOfPropertyChange(nameof(IsChecked));
            }
        }

        #region ICharacterGroup

        string ICharacterGroup.Name => this.name;

        ICharacterRow[] ICharacterGroup.Items => this.Items;

        public bool ContainsListCollection => throw new NotImplementedException();

        #endregion
    }
}
