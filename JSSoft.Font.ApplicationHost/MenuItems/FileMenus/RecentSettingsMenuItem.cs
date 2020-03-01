using Ntreev.Library;
using Ntreev.ModernUI.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Linq;

namespace JSSoft.Font.ApplicationHost.MenuItems.FileMenus
{
    [Export(typeof(IMenuItem))]
    [ParentType(typeof(FileMenuItem))]
    [Order(10)]
    class RecentSettingsMenuItem : MenuItemBase
    {
        private readonly ShellViewModel shell;
        private readonly Dictionary<string, RecentSettingsItemMenuItem> itemByPath = new Dictionary<string, RecentSettingsItemMenuItem>();
        private readonly ObservableCollection<RecentSettingsItemMenuItem> itemList = new ObservableCollection<RecentSettingsItemMenuItem>();

        [ImportingConstructor]
        public RecentSettingsMenuItem(ShellViewModel shell)
        {
            this.shell = shell;
            this.DisplayName = "Recent Settings";
            this.shell.RecentSettings.CollectionChanged += RecentSettings_CollectionChanged;
        }

        public override IEnumerable<IMenuItem> ItemsSource => this.itemList;

        private void RecentSettings_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    if (item is string text && this.itemByPath.ContainsKey(text) == false)
                    {
                        var viewModel = new RecentSettingsItemMenuItem(this.shell, text);
                        this.itemByPath.Add(text, viewModel);
                        this.itemList.Add(viewModel);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    if (item is string text && this.itemByPath.ContainsKey(text) == true)
                    {
                        var viewModel = this.itemByPath[text];
                        this.itemByPath.Remove(text);
                        this.itemList.Add(viewModel);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Replace)
            {

            }
            else if (e.Action == NotifyCollectionChangedAction.Move)
            {

            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                this.itemByPath.Clear();
                this.itemList.Clear();
            }
            this.InvokeCanExecuteChangedEvent();
        }

        protected override bool OnCanExecute(object parameter)
        {
            return this.shell.IsProgressing == false && this.shell.RecentSettings.Any();
        }
    }
}
