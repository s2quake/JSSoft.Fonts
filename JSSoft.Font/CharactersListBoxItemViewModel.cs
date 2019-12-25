using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font
{
    internal class CharactersListBoxItemViewModel : Ntreev.ModernUI.Framework.ViewModels.ListBoxItemViewModel
    {
        private readonly IFontService fontService;
        private string name;
        private bool? isChecked;

        public CharactersListBoxItemViewModel(IFontService fontService, string name, uint min, uint max)
        {
            this.fontService = fontService ?? throw new ArgumentNullException(nameof(fontService));
            this.name = name;

            var i1 = min;
            var itemList = new List<CharacterRowItem>();
            while (i1 < max)
            {
                var i2 = Math.Min(i1 + 16, max);
                itemList.Add(new CharacterRowItem(this.fontService, i1, i2));
                i1 = i2 + 1;
            }
            this.Items = itemList.ToArray();
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

        public CharacterRowItem[] Items { get; }
    }
}
