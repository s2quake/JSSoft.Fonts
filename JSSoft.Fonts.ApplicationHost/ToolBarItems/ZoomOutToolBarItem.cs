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

using JSSoft.Fonts.ApplicationHost.Properties;
using JSSoft.ModernUI.Framework;
using System.ComponentModel.Composition;
using System.Windows.Input;

namespace JSSoft.Fonts.ApplicationHost.ToolBarItems
{
    [Export(typeof(IToolBarItem))]
    [ParentType(typeof(IShell))]
    class ZoomOutToolBarItem : ToolBarItemBase
    {
        private readonly IShell shell;

        [ImportingConstructor]
        public ZoomOutToolBarItem(IShell shell)
        {
            this.shell = shell;
            this.DisplayName = Resources.ToolBarItem_ZoomOut;
            this.InputGesture = new KeyGesture(Key.OemMinus, ModifierKeys.Control);
            this.Icon = "Images/zoom-out.png";
        }

        protected override bool OnCanExecute(object parameter)
        {
            return this.shell.IsProgressing == false;
        }

        protected override void OnExecute(object parameter)
        {
            this.shell.ZoomLevel /= 2.0;
        }
    }
}
