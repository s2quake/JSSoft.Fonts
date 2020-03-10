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

using JSSoft.Font.ApplicationHost.Commands;
using Ntreev.ModernUI.Framework;
using System;

namespace JSSoft.Font.ApplicationHost.MenuItems.FileMenus
{
    [ParentType(typeof(RecentSettingsMenuItem))]
    class RecentFontsItemMenuItem : MenuItemBase
    {
        public RecentFontsItemMenuItem(IShell shell, string filename)
        {
            this.Shell = shell;
            this.Filename = filename;
            this.DisplayName = filename;
        }

        public string Filename { get; }

        protected override bool OnCanExecute(object parameter)
        {
            return OpenFontCommand.CanExecute(this.Shell);
        }

        protected async override void OnExecute(object parameter)
        {
            try
            {
                await OpenFontCommand.ExecuteAsync(this.Shell, this.Filename);
            }
            catch (Exception e)
            {
                AppMessageBox.ShowError(e);
            }
        }

        private IShell Shell { get; }
    }
}
