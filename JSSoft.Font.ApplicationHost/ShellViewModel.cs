using JSSoft.Font.ApplicationHost.Serializations;
using Ntreev.Library;
using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
        private readonly IAppConfiguration configs;
        private readonly ObservableCollection<CharacterGroup> groupList = new ObservableCollection<CharacterGroup>();
        private CharacterGroup selectedGroup;
        private double zoomLevel = 1.0;
        private bool isOpened;
        private bool isModified;

        [ImportingConstructor]
        public ShellViewModel([ImportMany]IEnumerable<IMenuItem> menuItems, [ImportMany]IEnumerable<IToolBarItem> toolBarItems, IAppConfiguration configs)
        {
            this.menuItems = menuItems;
            this.toolBarItems = toolBarItems;
            this.configs = configs;
            this.DisplayName = "JSFont";
            this.Settings.PropertyChanged += ExportSettings_PropertyChanged;
            this.Dispatcher.InvokeAsync(this.ReadRecentSettings);
        }

        public async Task OpenAsync(string fontPath, int size, int dpi, int faceIndex)
        {
            await this.BeginTaskAsync(null, this.ValidateOpen);
            await this.OpenFontDescriptorAsync(fontPath, size, dpi, faceIndex);
            await this.EndTaskAsync(() =>
            {
                this.Groups.Clear();
                foreach (var item in this.groupList)
                {
                    this.SatisfyImportsOnce(item);
                    if (item.IsVisible == true)
                        this.Groups.Add(item);
                }
                this.DisplayName = this.FontDescriptor.Name;
                this.SelectedGroup = this.Groups.FirstOrDefault();
                this.IsProgressing = false;
                this.IsModified = false;
                this.IsOpened = true;
                this.OnOpened(EventArgs.Empty);
            });
        }

        public async Task ExportAsync(string filename)
        {
            await this.BeginTaskAsync(null, null);
            await Task.Run(() =>
            {
                var dataSettings = (FontDataSettings)this.Settings;
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
            var fullPath = Path.GetFullPath(filename);
            var info = await this.BeginTaskAsync(() => ExportSettingsInfo.Create(this));
            await WriteSettingsAsync(fullPath, info);
            await this.EndTaskAsync(() =>
            {
                this.RecentSettings.Remove(fullPath);
                this.RecentSettings.Insert(0, fullPath);
                this.configs[nameof(RecentSettings)] = this.RecentSettings.ToArray();
                this.IsModified = true;
            });
        }

        public async Task LoadSettingsAsync(string filename)
        {
            var fullPath = Path.GetFullPath(filename);
            var isOpened = await this.BeginTaskAsync(() => this.isOpened);
            var info = await ReadSettingsAsync(fullPath);
            if (isOpened == true)
                await this.CloseAsync();
            await this.OpenAsync(info.Font, info.Size, info.DPI, 0);
            await this.EndTaskAsync(() =>
            {
                this.UpdateCheckState(info.Characters);
                this.Settings.PropertyChanged -= ExportSettings_PropertyChanged;
                this.Settings.Update(info);
                this.Settings.PropertyChanged += ExportSettings_PropertyChanged;
                this.IsModified = false;
            });
        }

        public async Task<ImageSource[]> PreviewAsync()
        {
            await this.BeginTaskAsync(null, null);
            var images = await Task.Run(() =>
            {
                var dataSettings = (FontDataSettings)this.Settings;
                var data = new FontData(this.FontDescriptor, dataSettings);
                var query = from fontGroup in this.Groups
                            where fontGroup.IsChecked != false
                            from row in fontGroup.Items
                            where row.IsChecked != false
                            from item in row.Items
                            where item.IsChecked
                            select item.ID;
                var items = query.ToArray();
                data.Generate(items);

                var imageList = new List<ImageSource>(data.Pages.Length);
                for (var i = 0; i < data.Pages.Length; i++)
                {
                    var item = data.Pages[i];
                    var stream = new MemoryStream();
                    var bitmapImage = new BitmapImage();
                    item.Save(stream);
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.UriSource = null;
                    bitmapImage.StreamSource = stream;
                    bitmapImage.EndInit();
                    bitmapImage.Freeze();
                    imageList.Add(bitmapImage);
                    stream.Dispose();
                }
                return imageList.ToArray();
            });
            await this.EndTaskAsync(null);
            return images;
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

        public ObservableCollection<string> RecentSettings { get; } = new ObservableCollection<string>();

        public CharacterGroup SelectedGroup
        {
            get => this.selectedGroup;
            set
            {
                if (this.selectedGroup != value)
                {
                    this.selectedGroup = value;
                    this.NotifyOfPropertyChange(nameof(SelectedGroup));
                    this.NotifyOfPropertyChange(nameof(CharacterRows));
                }
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
                if (this.isOpened != value)
                {
                    this.isOpened = value;
                    this.NotifyOfPropertyChange(nameof(VerticalAdvance));
                    this.NotifyOfPropertyChange(nameof(IsOpened));
                }
            }
        }

        public bool IsModified
        {
            get => this.isModified;
            set
            {
                if (this.isModified != value)
                {
                    this.isModified = value;
                    this.NotifyOfPropertyChange(nameof(IsModified));
                    this.NotifyOfPropertyChange(nameof(DisplayName));
                }
            }
        }

        public ExportSettings Settings { get; } = new ExportSettings();

        public override string DisplayName
        {
            get
            {
                if (this.IsModified == true)
                    return $"{base.DisplayName}*";
                return base.DisplayName;
            }
            set => base.DisplayName = value;
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
            //await this.OpenAsync(@"..\..\..\Fonts\SF-Mono-Semibold.otf", 0);
            await this.OpenAsync(@"..\..\..\Fonts\gulim.ttc", 14, 72, 0);
            //await this.OpenAsync(@"C:\Users\s2quake\Desktop\AppleSDGothicNeo-Semibold.otf");
        }

        private void ExportSettings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.IsModified = true;
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

        private async Task BeginTaskAsync(Action action, Action validation)
        {
            await this.Dispatcher.InvokeAsync(() =>
            {
                validation?.Invoke();
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

        private Task OpenFontDescriptorAsync(string fontPath, int size, int dpi, int faceIndex)
        {
            return Task.Run(() =>
            {
                this.FontDescriptor = new FontDescriptor(fontPath, (uint)dpi, size, faceIndex);
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

        private void ReadRecentSettings()
        {
            if (this.configs.Contains(nameof(RecentSettings)) == true)
            {
                var settigns = this.configs[nameof(RecentSettings)] as string[];
                foreach (var item in settigns)
                {
                    try
                    {
                        var fullPath = Path.GetFullPath(item);
                        if (File.Exists(fullPath) == true)
                            this.RecentSettings.Add(fullPath);
                    }
                    catch
                    {

                    }
                }
            }
        }

        private void ValidateOpen()
        {
            if (this.IsOpened == true)
                throw new InvalidOperationException("font already open.");
        }

        private void ValidateClose()
        {
            if (this.IsOpened == false)
                throw new InvalidOperationException("font does not open.");
        }

        #region IShell

        async Task IShell.CloseAsync()
        {
            await this.BeginTaskAsync(null, this.ValidateClose);
            await Task.Run(() => this.FontDescriptor.Dispose());
            await this.EndTaskAsync(() =>
            {
                this.NotifyOfPropertyChange(nameof(this.VerticalAdvance));
                this.DisplayName = "JSFont";
                this.IsOpened = false;
                this.IsProgressing = false;
                this.OnClosed(EventArgs.Empty);
            });
        }

        IEnumerable<ICharacterGroup> IShell.Groups => this.Groups;

        IEnumerable<string> IShell.RecentSettings => this.RecentSettings;

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
