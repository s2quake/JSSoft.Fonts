using Ntreev.Library;
using Ntreev.ModernUI.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;

namespace JSSoft.Font.ApplicationHost.MenuItems.FileMenus
{
    [Export(typeof(IMenuItem))]
    [ParentType(typeof(FileMenuItem))]
    [Category("Recent")]
    class RecentFontsMenuItem : MenuItemBase
    {
        private readonly ShellViewModel shell;
        private readonly Dictionary<string, RecentFontsItemMenuItem> itemByPath = new Dictionary<string, RecentFontsItemMenuItem>();
        private readonly ObservableCollection<RecentFontsItemMenuItem> itemList = new ObservableCollection<RecentFontsItemMenuItem>();

        [ImportingConstructor]
        public RecentFontsMenuItem(ShellViewModel shell)
        {
            this.shell = shell;
            this.DisplayName = "Recent Fonts";
            this.shell.RecentFonts.CollectionChanged += RecentFonts_CollectionChanged;
        }

        public override IEnumerable<IMenuItem> ItemsSource => this.itemList;

        private void RecentFonts_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    if (item is string text && this.itemByPath.ContainsKey(text) == false)
                    {
                        var viewModel = new RecentFontsItemMenuItem(this.shell, text);
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
            return this.shell.IsProgressing == false && this.shell.RecentFonts.Any();
        }
    }
}
