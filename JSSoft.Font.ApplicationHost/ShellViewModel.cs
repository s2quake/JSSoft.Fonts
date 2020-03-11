// MIT License
// 
// Copyright (c) 2020 Jeesu Choi
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

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
        private ObservableCollection<uint> checkedCharacters = new ObservableCollection<uint>();
        private CharacterGroup selectedGroup;
        private Character selectedCharacter;
        private double zoomLevel = 1.0;
        private bool isOpened;
        private bool isModified;
        private FontInfo fontInfo = FontInfo.Empty;
        private CharacterContext context;
        private string settingsPath;

        [ImportingConstructor]
        public ShellViewModel(IServiceProvider serviceProvider, IAppConfiguration configs, PropertyService propertyService)
            : base(serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.configs = configs;
            this.PropertyService = propertyService;
            this.DisplayName = "JSFont";
            this.Settings.PropertyChanged += ExportSettings_PropertyChanged;
            this.Dispatcher.InvokeAsync(this.ReadRecentSettings);
            this.Dispatcher.InvokeAsync(this.ReadRecentFonts);
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
                    var name = Path.GetFileNameWithoutExtension(filename);
                    var dataSettings = this.Settings.Convert(name, this.checkedCharacters.ToArray());
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

        public Task SaveSettingsAsync()
        {
            if (this.SettingsPath == string.Empty)
                throw new InvalidOperationException("invalid settings path");
            return this.SaveSettingsAsync(this.SettingsPath);
        }

        public async Task SaveSettingsAsync(string filename)
        {
            try
            {
                var fullPath = Path.GetFullPath(filename ?? throw new ArgumentNullException(nameof(filename)));
                await this.BeginProgressAsync();
                var info = await this.Dispatcher.InvokeAsync(() => ExportSettingsInfo.Create(this));
                await WriteSettingsAsync(fullPath, info);
                await this.Dispatcher.InvokeAsync(() =>
                {
                    this.RecentSettings.Remove(fullPath);
                    this.RecentSettings.Insert(0, fullPath);
                    this.configs[nameof(RecentSettings)] = this.RecentSettings.ToArray();
                    this.SettingsPath = filename;
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
                var fullPath = Path.GetFullPath(filename ?? throw new ArgumentNullException(nameof(filename)));
                await this.BeginProgressAsync();
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
                    this.SettingsPath = filename;
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
                    var dataSettings = this.Settings.Convert(string.Empty, this.checkedCharacters.ToArray());
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

        public ObservableCollection<string> RecentFonts { get; } = new ObservableCollection<string>();

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
                    this.OnSelectedGroupChanged(EventArgs.Empty);
                }
            }
        }

        public Character SelectedCharacter
        {
            get => this.selectedCharacter;
            set
            {
                if (this.selectedCharacter != value)
                {
                    if (value != null && value.Row != null)
                        this.SelectedGroup = value.Row.Group;
                    this.selectedCharacter = value;
                    this.NotifyOfPropertyChange(nameof(SelectedCharacter));
                    this.NotifyOfPropertyChange(nameof(CharacterRows));
                    this.PropertyService.SelectedObject = this.selectedCharacter;
                    this.OnSelectedcharacterChanged(EventArgs.Empty);
                }
            }
        }

        public CharacterRow[] CharacterRows => this.selectedGroup != null ? this.selectedGroup.Items : new CharacterRow[] { };

        public uint[] CheckedCharacters => this.checkedCharacters.ToArray();

        public IEnumerable<IMenuItem> MenuItems => MenuItemUtility.GetMenuItems(this, this.serviceProvider);

        public IEnumerable<IToolBarItem> ToolBarItems => ToolBarItemUtility.GetToolBarItems(this, this.serviceProvider);

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

        public string SettingsPath
        {
            get => this.settingsPath ?? string.Empty;
            private set
            {
                this.settingsPath = value;
                this.NotifyOfPropertyChange(nameof(SettingsPath));
            }
        }

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

        public PropertyService PropertyService { get; }

        public FontInfo FontInfo
        {
            get => this.fontInfo;
            private set
            {
                this.fontInfo = value;
                this.NotifyOfPropertyChange(nameof(FontInfo));
            }
        }

        public event EventHandler Opened;

        public event EventHandler Closed;

        public event EventHandler SelectedGroupChanged;

        public event EventHandler SelectedcharacterChanged;

        protected virtual void OnOpened(EventArgs e) => this.Opened?.Invoke(this, e);

        protected virtual void OnClosed(EventArgs e) => this.Closed?.Invoke(this, e);

        protected virtual void OnSelectedGroupChanged(EventArgs e) => this.SelectedGroupChanged?.Invoke(this, e);

        protected virtual void OnSelectedcharacterChanged(EventArgs e) => this.SelectedcharacterChanged?.Invoke(this, e);

        protected override void OnDeactivate(bool close)
        {
            if (close == true)
            {
                this.FontDescriptor?.Dispose();
                this.FontDescriptor = null;
            }
            base.OnDeactivate(close);
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
        }

        private void CheckedCharacters_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.IsModified = true;
        }

        private void ExportSettings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.IsModified = true;
        }

        private async Task OpenFontDescriptorAsync(string fontPath, int size, int dpi, int faceIndex)
        {
            var fullPath = Path.GetFullPath(fontPath);
            await Task.Run(() =>
            {
                var fontDescriptor = new FontDescriptor(fullPath, (uint)dpi, size, faceIndex);
                this.checkedCharacters = new ObservableCollection<uint>();
                this.checkedCharacters.CollectionChanged += CheckedCharacters_CollectionChanged;
                this.groupList.Clear();
                this.context = new CharacterContext(fontDescriptor, this.checkedCharacters);
                foreach (var (name, min, max) in NamesList.Items)
                {
                    var items = CreateGroups(this.context, name, min, max);
                    Array.ForEach(items, item => this.groupList.Add(item));
                }
                this.FontDescriptor = fontDescriptor;
            });
            await this.Dispatcher.InvokeAsync(() =>
            {
                this.RecentFonts.Remove(fullPath);
                this.RecentFonts.Insert(0, fullPath);
                this.configs[nameof(RecentFonts)] = this.RecentFonts.ToArray();
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
                if (characterByID.ContainsKey(item) == true)
                    characterByID[item].IsChecked = true;
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

        private void ReadRecentFonts()
        {
            if (this.configs.Contains(nameof(RecentFonts)) == true)
            {
                var settigns = this.configs[nameof(RecentFonts)] as string[];
                foreach (var item in settigns)
                {
                    try
                    {
                        var fullPath = Path.GetFullPath(item);
                        if (File.Exists(fullPath) == true)
                            this.RecentFonts.Add(fullPath);
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
                this.DisplayName = this.FontDescriptor.Name;
                this.SelectedGroup = this.Groups.FirstOrDefault();
                this.FontInfo = new FontInfo()
                {
                    FaceName = this.FontDescriptor.Name,
                    DPI = (int)this.FontDescriptor.DPI,
                    Size = this.FontDescriptor.Size,
                    Height = this.FontDescriptor.Height,
                    BaseLine = this.FontDescriptor.BaseLine,
                };
                this.IsModified = false;
                this.IsOpened = true;
                this.OnOpened(EventArgs.Empty);
            });
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
                this.Groups.Clear();
                this.DisplayName = "JSFont";
                this.SelectedGroup = null;
                this.SelectedCharacter = null;
                this.FontInfo = FontInfo.Empty;
                this.IsModified = false;
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

        IEnumerable<string> IShell.RecentFonts => this.RecentFonts;

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

        ICharacter IShell.SelectedCharacter
        {
            get => this.SelectedCharacter;
            set
            {
                if (value is Character || value is null)
                {
                    this.SelectedCharacter = value as Character;
                }
                else
                {
                    throw new ArgumentException(nameof(value));
                }
            }
        }

        IPropertyService IShell.PropertyService => this.PropertyService;

        #endregion
    }
}
