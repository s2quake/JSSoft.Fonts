using Microsoft.Win32;
using Ntreev.Library;
using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font.ApplicationHost.MenuItems.FileMenus
{
    [Export(typeof(IMenuItem))]
    [ParentType(typeof(FileMenuItem))]
    [Order(10)]
    class RecentSettingsMenuItem : MenuItemBase
    {
        private readonly Lazy<ShellViewModel> shell;
        private readonly Dictionary<string, RecentSettingsItemMenuItem> itemByPath = new Dictionary<string, RecentSettingsItemMenuItem>();
        private readonly ObservableCollection<RecentSettingsItemMenuItem> itemList = new ObservableCollection<RecentSettingsItemMenuItem>();

        [ImportingConstructor]
        public RecentSettingsMenuItem(Lazy<ShellViewModel> shell)
        {
            this.shell = shell;
            this.DisplayName = "Recent Settings";
        }

        public override IEnumerable<IMenuItem> ItemsSource => this.itemList;

        protected override void OnImportsSatisfied()
        {
            this.Dispatcher.InvokeAsync(() =>
            {
                this.Shell.RecentSettings.CollectionChanged += RecentSettings_CollectionChanged;
            });
        }

        private void RecentSettings_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    if (item is string text && this.itemByPath.ContainsKey(text) == false)
                    {
                        var viewModel = new RecentSettingsItemMenuItem(this.Shell, text);
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
            return this.Shell.IsProgressing == false && this.Shell.RecentSettings.Any();
        }

        private ShellViewModel Shell => this.shell.Value;
    }
}
