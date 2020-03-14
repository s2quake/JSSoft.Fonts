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

using JSSoft.Font.ApplicationHost.Input;
using Ntreev.ModernUI.Framework;
using System.ComponentModel.Composition;

namespace JSSoft.Font.ApplicationHost.MenuItems.ViewMenus
{
    [Export(typeof(IMenuItem))]
    [ParentType(typeof(ViewMenuItem))]
    class ShowPropertyWindowMenuItem : MenuItemBase
    {
        private readonly ShellView shellView;

        [ImportingConstructor]
        public ShowPropertyWindowMenuItem(ShellView shellView)
        {
            this.shellView = shellView;
            this.HideOnDisabled = true;
            this.DisplayName = "Show Property Window";
            FontCommands.HidePropertyWindow.CanExecuteChanged += HidePropertyWindow_CanExecuteChanged;
        }

        private void HidePropertyWindow_CanExecuteChanged(object sender, System.EventArgs e)
        {
            this.InvokeCanExecuteChangedEvent();
        }

        protected override bool OnCanExecute(object parameter)
        {
            return FontCommands.ShowPropertyWindow.CanExecute(parameter, this.shellView);
        }

        protected override void OnExecute(object parameter)
        {
            FontCommands.ShowPropertyWindow.Execute(parameter, this.shellView);
            this.InvokeCanExecuteChangedEvent();
        }
    }
}
