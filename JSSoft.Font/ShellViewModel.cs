using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font
{
    [Export(typeof(IShell))]
    class ShellViewModel : Screen
    {
        private ObservableCollection<CharactersListBoxItemViewModel> itemsSource = new ObservableCollection<CharactersListBoxItemViewModel>();
        private CharactersListBoxItemViewModel selectedItem;

        public ShellViewModel()
        {
            foreach (var (name, min, max) in NamesList.items)
            {
                this.itemsSource.Add(new CharactersListBoxItemViewModel(name, min, max));
            }
        }

        public ObservableCollection<CharactersListBoxItemViewModel> ItemsSource => this.itemsSource;

        public CharactersListBoxItemViewModel SelectedItem
        {
            get => this.selectedItem;
            set
            {
                this.selectedItem = value;
                this.NotifyOfPropertyChange(nameof(SelectedItem));
                this.NotifyOfPropertyChange(nameof(CharacterItems));
            }
        }

        public CharacterItem[] CharacterItems => this.selectedItem != null ? this.selectedItem.Items : new CharacterItem[] { };
    }
}
