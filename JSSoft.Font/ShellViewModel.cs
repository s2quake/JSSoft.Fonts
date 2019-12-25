using Caliburn.Micro;
using Ntreev.ModernUI.Framework;
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
    class ShellViewModel : ScreenBase
    {
        private readonly IFontService fontService;
        private ObservableCollection<CharactersListBoxItemViewModel> itemList = new ObservableCollection<CharactersListBoxItemViewModel>();
        private ObservableCollection<CharactersListBoxItemViewModel> visibleList = new ObservableCollection<CharactersListBoxItemViewModel>();
        private CharactersListBoxItemViewModel selectedItem;

        [ImportingConstructor]
        public ShellViewModel(IFontService fontService)
        {
            this.fontService = fontService;
        }

        public void Open()
        {

        }

        public ObservableCollection<CharactersListBoxItemViewModel> ItemsSource => this.visibleList;

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

        public CharacterRowItem[] CharacterItems => this.selectedItem != null ? this.selectedItem.Items : new CharacterRowItem[] { };

        protected override async void OnDeactivate(bool close)
        {
            if (close == true)
            {
                await this.fontService.CloseAsync();
            }
            base.OnDeactivate(close);
        }

        protected override async void OnInitialize()
        {
            base.OnInitialize();
            await this.fontService.OpenAsync(@"SF-Mono-Semibold.otf");
            foreach (var (name, min, max) in NamesList.Items)
            {
                var item = new CharactersListBoxItemViewModel(this.fontService, name, min, max);
                this.SatisfyImportsOnce(item);
                this.itemList.Add(item);
            }
            foreach (var item in this.itemList)
            {
                if (item.IsVisible == true)
                    this.visibleList.Add(item);
            }
        }
    }
}
