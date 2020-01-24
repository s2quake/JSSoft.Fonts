using Ntreev.ModernUI.Framework.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font.ApplicationHost
{
    class CharacterGroup : ListBoxItemViewModel, ICharacterGroup
    {
        private readonly FontDescriptor fontDescriptor;
        private string name;
        private bool? isChecked;

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
                this.isChecked = value;
                this.NotifyOfPropertyChange(nameof(IsChecked));
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
                itemList.Add(new CharacterRow(this, this.fontDescriptor, i1, i2));
                i1 = i2;
            }
            return itemList.ToArray();
        }

        #region ICharacterGroup

        string ICharacterGroup.Name => this.name;

        ICharacterRow[] ICharacterGroup.Items => this.Items;

        #endregion
    }
}
