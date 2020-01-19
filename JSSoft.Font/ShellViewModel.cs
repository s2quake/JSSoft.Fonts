using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JSSoft.Font
{
    [Export(typeof(IShell))]
    [Export]
    class ShellViewModel : ScreenBase, IShell
    {
        private readonly IFontService fontService;
        private readonly IEnumerable<IMenuItem> menuItems;
        private readonly ObservableCollection<CharacterGroup> groupList = new ObservableCollection<CharacterGroup>();
        private CharacterGroup selectedGroup;
        private CancellationTokenSource cancellation;

        [ImportingConstructor]
        public ShellViewModel(IFontService fontService, [ImportMany]IEnumerable<IMenuItem> menuItems)
        {
            this.fontService = fontService;
            this.menuItems = menuItems;
            this.DisplayName = "JSFont";
        }

        public async Task OpenAsync(string fontPath)
        {
            await this.Dispatcher.InvokeAsync(() => this.IsProgressing = true);
            this.cancellation = new CancellationTokenSource();
            await this.fontService.OpenAsync(fontPath, this.cancellation.Token);
            foreach (var (name, min, max) in NamesList.Items)
            {
                var item = new CharacterGroup(this.fontService, name, min, max);
                this.SatisfyImportsOnce(item);
                this.groupList.Add(item);
            }
            foreach (var item in this.groupList)
            {
                if (item.IsVisible == true)
                    this.Groups.Add(item);
            }
            await this.Dispatcher.InvokeAsync(() =>
            {
                this.DisplayName = this.fontService.Name;
                this.IsOpened = true;
                this.IsProgressing = false;
            });
        }

        public ObservableCollection<CharacterGroup> Groups { get; } = new ObservableCollection<CharacterGroup>();

        public CharacterGroup SelectedGroup
        {
            get => this.selectedGroup;
            set
            {
                this.selectedGroup = value;
                this.NotifyOfPropertyChange(nameof(SelectedGroup));
                this.NotifyOfPropertyChange(nameof(CharacterRows));
            }
        }

        public CharacterRow[] CharacterRows => this.selectedGroup != null ? this.selectedGroup.Items : new CharacterRow[] { };

        public IEnumerable<IMenuItem> MenuItems => MenuItemUtility.GetMenuItems(this, this.menuItems);

        public bool IsOpened { get; private set; }

        protected override Task<bool> CloseAsync()
        {
            this.cancellation?.Cancel();
            return Task.Run(() => true);
        }

        protected override async void OnDeactivate(bool close)
        {
            if (close == true && this.fontService.IsOpened == true)
            {
                await this.fontService.CloseAsync();
            }
            base.OnDeactivate(close);
        }

        protected async override void OnInitialize()
        {
            base.OnInitialize();
            //this.Open(@"SF-Mono-Semibold.otf");
            await this.OpenAsync(@"C:\Users\s2quake\Desktop\AppleSDGothicNeo-Semibold.otf");
        }

        #region IShell

        async Task IShell.CloseAsync()
        {
            await this.Dispatcher.InvokeAsync(() =>
            {

            });
            await this.fontService.CloseAsync();
            await this.Dispatcher.InvokeAsync(() =>
            {
                this.DisplayName = "JSFont";
                this.IsOpened = false;
                this.IsProgressing = false;
            });
        }


        IEnumerable<ICharacterGroup> IShell.Groups => this.Groups;

        ICharacterGroup IShell.SelectedGroup
        {
            get => this.SelectedGroup;
            set
            {
                if (value is CharacterGroup group && this.Groups.Contains(group) == false)
                    throw new ArgumentException(nameof(value));
                this.SelectedGroup = value as CharacterGroup;
            }
        }

        #endregion
    }
}
