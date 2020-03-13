﻿// MIT License
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
using Ntreev.Library;
using Ntreev.ModernUI.Framework;
using System;
using System.ComponentModel.Composition;
using System.Windows.Input;

namespace JSSoft.Font.ApplicationHost.MenuItems.FileMenus
{
    [Export(typeof(IMenuItem))]
    [ParentType(typeof(FileMenuItem))]
    [Dependency(typeof(OpenFontMenuItem))]
    class CloseFontMenuItem : MenuItemBase
    {
        private readonly IShell shell;

        [ImportingConstructor]
        public CloseFontMenuItem(IShell shell)
        {
            this.shell = shell;
            this.DisplayName = "Close";
            this.shell.Opened += (s, e) => this.InvokeCanExecuteChangedEvent();
            this.shell.Closed += (s, e) => this.InvokeCanExecuteChangedEvent();
        }

        protected override bool OnCanExecute(object parameter) => CloseFontCommand.CanExecute(this.shell);

        protected override async void OnExecute(object parameter)
        {
            try
            {
                await CloseFontCommand.ExecuteAsync(this.shell);
            }
            catch (Exception e)
            {
                AppMessageBox.ShowError(e);
            }
        }
    }
}