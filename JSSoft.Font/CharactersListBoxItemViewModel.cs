using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font
{
    class CharactersListBoxItemViewModel : Ntreev.ModernUI.Framework.ViewModels.ListBoxItemViewModel
    {
        private string name;
        private bool? isChecked;

        public CharactersListBoxItemViewModel(string name, uint min, uint max)
        {
            this.name = name;
            this.Items = new CharacterItem[max - min + 1];

            for (var i = 0; i < this.Items.Length; i++)
            {
                this.Items[i] = new CharacterItem((uint)(i + min));
            }
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

        public CharacterItem[] Items { get; }
    }
}
