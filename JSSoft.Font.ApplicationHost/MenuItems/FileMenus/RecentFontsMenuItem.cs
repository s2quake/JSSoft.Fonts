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

using JSSoft.Font.ApplicationHost.Properties;
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
            this.DisplayName = Resources.MenuItem_RecentFonts;
            this.shell.RecentFonts.CollectionChanged += RecentFonts_CollectionChanged;
        }

        public override IEnumerable<IMenuItem> MenuItems => this.itemList;

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
