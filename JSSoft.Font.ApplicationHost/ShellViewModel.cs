using JSSoft.Font.ApplicationHost.Serializations;
using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace JSSoft.Font.ApplicationHost
{
    [Export(typeof(IShell))]
    [Export]
    class ShellViewModel : ScreenBase, IShell
    {
        private readonly IEnumerable<IMenuItem> menuItems;
        private readonly IEnumerable<IToolBarItem> toolBarItems;
        private readonly ObservableCollection<CharacterGroup> groupList = new ObservableCollection<CharacterGroup>();
        private CharacterGroup selectedGroup;
        private double zoomLevel = 1.0;
        private ExportSettings exportSettings = new ExportSettings();
        private bool isOpened;

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
            await this.OpenFontDescriptorAsync(fontPath);
            await this.Dispatcher.InvokeAsync(() =>
            {
                foreach (var item in this.groupList)
                {
                    this.SatisfyImportsOnce(item);
                    if (item.IsVisible == true)
                        this.Groups.Add(item);
                }
                this.DisplayName = this.FontDescriptor.Name;
                this.IsOpened = true;
                this.IsProgressing = false;
                this.SelectedGroup = this.Groups.FirstOrDefault();
                this.OnOpened(EventArgs.Empty);
            });
        }

        public async Task ExportAsync(string filename)
        {
            await this.BeginTaskAsync(null);
            await Task.Run(() =>
            {
                var dataSettings = new FontDataSettings()
                {

                };
                var data = new FontData(this.FontDescriptor, dataSettings);
                var fullPath = Path.GetFullPath(filename);
                var directory = Path.GetDirectoryName(fullPath);
                var query = from fontGroup in this.Groups
                            where fontGroup.IsChecked != false
                            from row in fontGroup.Items
                            where row.IsChecked != false
                            from item in row.Items
                            where item.IsChecked
                            select item.ID;
                var items = query.ToArray();
                data.Generate(items);
                data.Save(filename);
                data.SavePages(directory);
            });
            await this.EndTaskAsync(null);
        }

        public async Task SaveSettingsAsync(string filename)
        {
            var info = await this.BeginTaskAsync(() => ExportSettingsInfo.Create(this));
            await WriteSettingsAsync(filename, info);
            await this.EndTaskAsync(null);
        }

        public async Task LoadSettingsAsync(string filename)
        {
            var isOpened = await this.BeginTaskAsync(() => this.isOpened);
            var info = await ReadSettingsAsync(filename);
            if (isOpened == true)
                await this.CloseAsync();
            await this.OpenAsync(info.Font);
            await this.Dispatcher.InvokeAsync(() => this.UpdateCheckState(info.Characters));
        }

        private static CharacterGroup[] CreateGroups(FontDescriptor fontDescriptor, string name, uint min, uint max)
        {
            if (max - min <= 0xff)
            {
                return new CharacterGroup[] { new CharacterGroup(fontDescriptor, name, min, max) };
            }
            else
            {
                var groupList = new List<CharacterGroup>();
                while (max - min > 0xff)
                {
                    groupList.Add(new CharacterGroup(fontDescriptor, name, min, min + 0xff));
                    min += 0xff + 1;
                }
                if (max - min <= 0xff)
                {
                    groupList.Add(new CharacterGroup(fontDescriptor, name, min, max));
                }
                return groupList.ToArray();
            }
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

        public int VerticalAdvance => (this.FontDescriptor != null ? this.FontDescriptor.ItemHeight : 22);

        public double ZoomLevel
        {
            get => this.zoomLevel;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value));
                this.zoomLevel = value;
                this.NotifyOfPropertyChange(nameof(this.ZoomLevel));
            }
        }

        public bool IsOpened
        {
            get => this.isOpened;
            set
            {
                this.isOpened = value;
                this.NotifyOfPropertyChange(nameof(VerticalAdvance));
                this.NotifyOfPropertyChange(nameof(IsOpened));
            }
        }

        public ExportSettings ExportSettings
        {
            get => this.exportSettings;
            set
            {
                this.exportSettings = value ?? throw new ArgumentNullException(nameof(value));
                this.NotifyOfPropertyChange(nameof(ExportSettings));
            }
        }

        public FontDescriptor FontDescriptor { get; private set; }

        public event EventHandler Opened;

        public event EventHandler Closed;

        protected virtual void OnOpened(EventArgs e) => this.Opened?.Invoke(this, e);

        protected virtual void OnClosed(EventArgs e) => this.Closed?.Invoke(this, e);

        protected override void OnDeactivate(bool close)
        {
            if (close == true)
            {
                this.FontDescriptor?.Dispose();
                this.FontDescriptor = null;
            }
            base.OnDeactivate(close);
        }

        protected async override void OnInitialize()
        {
            base.OnInitialize();
            await this.OpenAsync(@"..\..\..\Fonts\SF-Mono-Semibold.otf");
            //await this.OpenAsync(@"..\..\..\Fonts\gulim.ttc");
            //await this.OpenAsync(@"C:\Users\s2quake\Desktop\AppleSDGothicNeo-Semibold.otf");
        }

        private async Task<T> BeginTaskAsync<T>(Func<T> func)
        {
            return await this.Dispatcher.InvokeAsync(() =>
            {
                if (this.IsProgressing == true)
                    throw new InvalidOperationException();
                this.IsProgressing = true;
                return func();
            });
        }

        private async Task BeginTaskAsync(Action action)
        {
            await this.Dispatcher.InvokeAsync(() =>
            {
                if (this.IsProgressing == true)
                    throw new InvalidOperationException();
                this.IsProgressing = true;
                action?.Invoke();
            });
        }

        private async Task EndTaskAsync(Action action)
        {
            await this.Dispatcher.InvokeAsync(() =>
            {
                action?.Invoke();
                this.IsProgressing = false;
            });
        }

        private Task OpenFontDescriptorAsync(string fontPath)
        {
            return Task.Run(() =>
            {
                this.FontDescriptor = new FontDescriptor(fontPath, 96, 16);
                this.groupList.Clear();
                foreach (var (name, min, max) in NamesList.Items)
                {
                    var items = CreateGroups(this.FontDescriptor, name, min, max);
                    Array.ForEach(items, item => this.groupList.Add(item));
                }
            });
        }

        private void UpdateCheckState(IEnumerable<uint> items)
        {
            this.Dispatcher.VerifyAccess();
            var query = from characterGroup in this.Groups
                        where characterGroup.IsVisible
                        from row in characterGroup.Items
                        from item in row.Items
                        where item.IsEnabled
                        select item;
            var characterByID = query.ToDictionary(item => item.ID);
            foreach (var item in items)
            {
                characterByID[item].SetChecked(true);
            }
        }

        private static Task<ExportSettingsInfo> ReadSettingsAsync(string filename)
        {
            return Task.Run(() =>
            {
                var serializer = new XmlSerializer(typeof(ExportSettingsInfo));
                using (var reader = XmlReader.Create(filename))
                {
                    return (ExportSettingsInfo)serializer.Deserialize(reader);
                }
            });
        }

        private static Task WriteSettingsAsync(string filename, ExportSettingsInfo info)
        {
            return Task.Run(() =>
            {
                var serializer = new XmlSerializer(typeof(ExportSettingsInfo));
                var writerSettings = new XmlWriterSettings()
                {
                    Encoding = Encoding.UTF8,
                    Indent = true,
                };
                using (var writer = XmlWriter.Create(filename, writerSettings))
                {
                    serializer.Serialize(writer, info);
                }
            });
        }

        #region IShell

        async Task IShell.CloseAsync()
        {
            await this.Dispatcher.InvokeAsync(() =>
            {

            });
            await Task.Run(() => this.FontDescriptor.Dispose());
            await this.Dispatcher.InvokeAsync(() =>
            {
                this.NotifyOfPropertyChange(nameof(this.VerticalAdvance));
                this.DisplayName = "JSFont";
                this.IsOpened = false;
                this.IsProgressing = false;
                this.OnClosed(EventArgs.Empty);
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
