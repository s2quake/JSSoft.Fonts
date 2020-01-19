using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JSSoft.Font.ApplicationHost
{
    [Export(typeof(IShell))]
    [Export]
    class ShellViewModel : ScreenBase, IShell
    {
        private readonly IEnumerable<IMenuItem> menuItems;
        private readonly IEnumerable<IToolBarItem> toolBarItems;
        private readonly ObservableCollection<CharacterGroup> groupList = new ObservableCollection<CharacterGroup>();
        private FontDescriptor fontDescriptor;
        private CharacterGroup selectedGroup;

        [ImportingConstructor]
        public ShellViewModel([ImportMany]IEnumerable<IMenuItem> menuItems, [ImportMany]IEnumerable<IToolBarItem> toolBarItems)
        {
            this.menuItems = menuItems;
            this.toolBarItems = toolBarItems;
            this.DisplayName = "JSFont";
        }

        public async Task OpenAsync(string fontPath)
        {
            await this.Dispatcher.InvokeAsync(() => this.IsProgressing = true);
            await Task.Run(() =>
            {
                this.fontDescriptor = new FontDescriptor(fontPath, 96, 22);
                foreach (var (name, min, max) in NamesList.Items)
                {
                    var item = new CharacterGroup(this.fontDescriptor, name, min, max);
                    this.groupList.Add(item);
                }
            });
            await this.Dispatcher.InvokeAsync(() =>
            {
                foreach (var item in this.groupList)
                {
                    this.SatisfyImportsOnce(item);
                    if (item.IsVisible == true)
                        this.Groups.Add(item);
                }
                this.DisplayName = this.fontDescriptor.Name;
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

        public IEnumerable<IToolBarItem> ToolBarItems => ToolBarItemUtility.GetToolBarItems(this, this.toolBarItems);

        public bool IsOpened { get; private set; }

        protected override void OnDeactivate(bool close)
        {
            if (close == true)
            {
                this.fontDescriptor?.Dispose();
                this.fontDescriptor = null;
            }
            base.OnDeactivate(close);
        }

        protected async override void OnInitialize()
        {
            base.OnInitialize();
            //await this.OpenAsync(@"SF-Mono-Semibold.otf");
            //await this.OpenAsync(@"C:\Users\s2quake\Desktop\AppleSDGothicNeo-Semibold.otf");
        }

        #region IShell

        async Task IShell.CloseAsync()
        {
            await this.Dispatcher.InvokeAsync(() =>
            {

            });
            await Task.Run(() => this.fontDescriptor.Dispose());
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
