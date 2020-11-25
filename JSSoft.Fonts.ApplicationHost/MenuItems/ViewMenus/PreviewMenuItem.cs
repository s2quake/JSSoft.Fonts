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

using JSSoft.Fonts.ApplicationHost.Dialogs.ViewModels;
using JSSoft.Fonts.ApplicationHost.Properties;
using JSSoft.ModernUI.Framework;
using System.ComponentModel.Composition;

namespace JSSoft.Fonts.ApplicationHost.MenuItems.ViewMenus
{
    [Export(typeof(IMenuItem))]
    [ParentType(typeof(ViewMenuItem))]
    class PreviewMenuItem : MenuItemBase
    {
        private readonly IShell shell;
        private readonly IAppConfiguration configs;

        [ImportingConstructor]
        public PreviewMenuItem(IShell shell, IAppConfiguration configs)
        {
            this.shell = shell;
            this.configs = configs;
            this.DisplayName = Resources.MenuItem_Preview;
            this.shell.Opened += (s, e) => this.InvokeCanExecuteChangedEvent();
            this.shell.Closed += (s, e) => this.InvokeCanExecuteChangedEvent();
        }

        protected override bool OnCanExecute(object parameter)
        {
            return this.shell.IsProgressing == false && this.shell.IsOpened == true;
        }

        protected async override void OnExecute(object parameter)
        {
            var images = await this.shell.PreviewAsync();
            var dialog = new PreviewViewModel(this.configs, images);
            await dialog.ShowDialogAsync();
        }
    }
}
