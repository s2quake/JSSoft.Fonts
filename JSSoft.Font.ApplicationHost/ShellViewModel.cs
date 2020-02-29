using JSSoft.Font.ApplicationHost.Serializations;
using Ntreev.Library;
using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
        private readonly IServiceProvider serviceProvider;
        private readonly IAppConfiguration configs;
        private readonly ObservableCollection<CharacterGroup> groupList = new ObservableCollection<CharacterGroup>();
        private ObservableCollection<uint> selectedCharacters;
        private CharacterGroup selectedGroup;
        private double zoomLevel = 1.0;
        private bool isOpened;
        private bool isModified;
        private CharacterContext context;

        [ImportingConstructor]
        public ShellViewModel(IServiceProvider serviceProvider, IAppConfiguration configs)
            : base(serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.configs = configs;
            this.DisplayName = "JSFont";
            this.Settings.PropertyChanged += ExportSettings_PropertyChanged;
            this.Dispatcher.InvokeAsync(this.ReadRecentSettings);
        }

        public async Task OpenAsync(string fontPath, int size, int dpi, int faceIndex)
        {
            try
            {
                await this.BeginProgressAsync();
                await this.BeginOpenAsync();
                await this.OpenFontDescriptorAsync(fontPath, size, dpi, faceIndex);
                await this.EndOpenAsync();
            }
            finally
            {
                await this.EndProgressAsync();
            }
        }

        public new async Task CloseAsync()
        {
            try
            {
                await this.BeginProgressAsync();
                await this.BeginCloseAsync();
                await Task.Run(this.FontDescriptor.Dispose);
                await this.EndCloseAsync();
            }
            finally
            {
                await this.EndProgressAsync();
            }
        }

        public async Task ExportAsync(string filename)
        {
            try
            {
                await this.BeginProgressAsync();
                await Task.Run(() =>
                {
                    var dataSettings = this.Settings.Convert(this.selectedCharacters.ToArray());
                    var data = this.FontDescriptor.CreateData(dataSettings);
                    var fullPath = Path.GetFullPath(filename);
                    var directory = Path.GetDirectoryName(fullPath);
                    data.Save(filename);
                    data.SavePages(directory);
                });
            }
            finally
            {
                await this.EndProgressAsync();
            }
        }

        public async Task SaveSettingsAsync(string filename)
        {
            try
            {
                await this.BeginProgressAsync();
                var fullPath = Path.GetFullPath(filename);
                var info = await this.Dispatcher.InvokeAsync(() => ExportSettingsInfo.Create(this));
                await WriteSettingsAsync(fullPath, info);
                await this.Dispatcher.InvokeAsync(() =>
                {
                    this.RecentSettings.Remove(fullPath);
                    this.RecentSettings.Insert(0, fullPath);
                    this.configs[nameof(RecentSettings)] = this.RecentSettings.ToArray();
                    this.IsModified = false;
                });
            }
            finally
            {
                await this.EndProgressAsync();
            }
        }

        public async Task LoadSettingsAsync(string filename)
        {
            try
            {
                await this.BeginProgressAsync();
                var fullPath = Path.GetFullPath(filename);
                var isOpened = await this.Dispatcher.InvokeAsync(() => this.isOpened);
                var info = await ReadSettingsAsync(fullPath);
                if (isOpened == true)
                {
                    await Task.Run(this.FontDescriptor.Dispose);
                    await this.EndCloseAsync();
                }
                await this.OpenFontDescriptorAsync(info.Font, info.Size, info.DPI, info.Face);
                await this.EndOpenAsync();
                await this.Dispatcher.InvokeAsync(() =>
                {
                    this.UpdateCheckState(info.Characters);
                    this.Settings.PropertyChanged -= ExportSettings_PropertyChanged;
                    this.Settings.Update(info);
                    this.Settings.PropertyChanged += ExportSettings_PropertyChanged;
                    this.IsModified = false;
                });
            }
            finally
            {
                await this.EndProgressAsync();
            }
        }

        public async Task<FontData> PreviewAsync()
        {
            try
            {
                await this.BeginProgressAsync();
                var value = await Task.Run(() =>
                {
                    var dataSettings = this.Settings.Convert(this.selectedCharacters.ToArray());
                    return this.FontDescriptor.CreateData(dataSettings);
                });
                return value;
            }
            finally
            {
                await this.EndProgressAsync();
            }
        }

        public async Task SelectCharactersAsync(params uint[] characters)
        {
            try
            {
                await this.BeginProgressAsync();
                await this.Dispatcher.InvokeAsync(() =>
                {
                    foreach (var item in this.Groups)
                    {
                        item.IsChecked = false;
                    }
                    this.UpdateCheckState(characters);
                });
            }
            finally
            {
                await this.EndProgressAsync();
            }
        }

        private static CharacterGroup[] CreateGroups(CharacterContext context, string name, uint min, uint max)
        {
            if (max - min <= 0xff)
            {
                return new CharacterGroup[] { new CharacterGroup(context, name, min, max) };
            }
            else
            {
                var groupList = new List<CharacterGroup>();
                var index = 0;
                while (max - min > 0xff)
                {
                    groupList.Add(new CharacterGroup(context, $"{name} {index++}", min, min + 0xff));
                    min += 0xff + 1;
                }
                if (max - min <= 0xff)
                {
                    groupList.Add(new CharacterGroup(context, $"{name} {index++}", min, max));
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

        public uint[] SelectedCharacters => this.selectedCharacters.ToArray();

        public IEnumerable<IMenuItem> MenuItems => MenuItemUtility.GetMenuItems<IMenuItem>(this, this.serviceProvider);

        public IEnumerable<IToolBarItem> ToolBarItems => ToolBarItemUtility.GetToolBarItems(this, this.serviceProvider);

        public int VerticalAdvance => (this.FontDescriptor != null ? this.FontDescriptor.Height : 22);

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
            //await this.OpenAsync(@"..\..\..\Fonts\SF-Mono-Semibold.otf", 22, 96, 0);
            await this.OpenAsync(@"..\..\..\Fonts\gulim.ttc", 14, 72, 0);
            //await this.OpenAsync(@"C:\Users\s2quake\Desktop\AppleSDGothicNeo-Semibold.otf");
            //await this.LoadSettingsAsync(@"..\..\..\Fonts\settings.xml");
        }

        private void SelectedCharacters_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.IsModified = true;
        }

        private void ExportSettings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.IsModified = true;
        }

        private Task OpenFontDescriptorAsync(string fontPath, int size, int dpi, int faceIndex)
        {
            return Task.Run(() =>
            {
                this.FontDescriptor = new FontDescriptor(fontPath, (uint)dpi, size, faceIndex);
                this.selectedCharacters = new ObservableCollection<uint>();
                this.selectedCharacters.CollectionChanged += SelectedCharacters_CollectionChanged;
                this.groupList.Clear();
                this.context = new CharacterContext(this.FontDescriptor, this.selectedCharacters);
                foreach (var (name, min, max) in NamesList.Items)
                {
                    var items = CreateGroups(this.context, name, min, max);
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

        private async Task BeginOpenAsync()
        {
            await this.Dispatcher.InvokeAsync(() =>
            {
                if (this.IsOpened == true)
                    throw new InvalidOperationException("font already open.");
            });
        }

        private async Task EndOpenAsync()
        {
            await this.Dispatcher.InvokeAsync(() =>
            {
                this.Groups.Clear();
                foreach (var item in this.groupList)
                {
                    this.SatisfyImportsOnce(item);
                    if (item.IsVisible == true)
                        this.Groups.Add(item);
                }
                foreach (var item in this.Groups)
                {
                    item.PropertyChanged += Group_PropertyChanged;
                }
                this.DisplayName = this.FontDescriptor.Name;
                this.SelectedGroup = this.Groups.FirstOrDefault();
                this.IsModified = false;
                this.IsOpened = true;
                this.OnOpened(EventArgs.Empty);
            });
        }

        private void Group_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        private async Task BeginCloseAsync()
        {
            await this.Dispatcher.InvokeAsync(() =>
            {
                if (this.IsOpened == false)
                    throw new InvalidOperationException("font does not open.");
            });
        }

        private async Task EndCloseAsync()
        {
            await this.Dispatcher.InvokeAsync(() =>
            {
                this.NotifyOfPropertyChange(nameof(this.VerticalAdvance));
                this.DisplayName = "JSFont";
                this.IsOpened = false;
                this.OnClosed(EventArgs.Empty);
            });
        }

        private async Task BeginProgressAsync()
        {
            await this.Dispatcher.InvokeAsync(() =>
            {
                if (this.IsProgressing == true)
                    throw new InvalidOperationException();
                this.IsProgressing = true;
            });
        }

        private async Task EndProgressAsync()
        {
            await this.Dispatcher.InvokeAsync(() =>
            {
                this.IsProgressing = false;
            });
        }

        #region IShell

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
