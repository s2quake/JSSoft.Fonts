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

using System.Linq;
using JSSoft.Font.ApplicationHost.Dialogs.ViewModels;
using Ntreev.ModernUI.Framework;
using System.ComponentModel.Composition;
using System.Windows.Input;

namespace JSSoft.Font.ApplicationHost.MenuItems.ToolMenus
{
    [Export(typeof(IMenuItem))]
    [ParentType(typeof(ToolMenuItem))]
    class FindCharacterMenuItem : MenuItemBase
    {
        private readonly IShell shell;

        [ImportingConstructor]
        public FindCharacterMenuItem(IShell shell)
        {
            this.shell = shell;
            this.DisplayName = "Find Character...";
            this.InputGesture = new KeyGesture(Key.F, ModifierKeys.Control);
            this.shell.Opened += (s, e) => this.InvokeCanExecuteChangedEvent();
            this.shell.Closed += (s, e) => this.InvokeCanExecuteChangedEvent();
        }

        protected override bool OnCanExecute(object parameter)
        {
            return this.shell.IsProgressing == false && this.shell.IsOpened == true;
        }

        protected override void OnExecute(object parameter)
        {
            var dialog = new FindCharacterViewModel();
            if (dialog.ShowDialog() == true)
            {
                var query = from characterGroup in this.shell.Groups
                            from row in characterGroup.Items
                            from character in row.Items
                            where character.ID == (uint)dialog.Character
                            select character;

                var item = query.FirstOrDefault();
                if (item != null)
                {
                    this.shell.SelectedCharacter = item;
                }
            }
        }
    }
}
