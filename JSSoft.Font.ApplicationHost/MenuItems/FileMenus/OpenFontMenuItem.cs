using Microsoft.Win32;
using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JSSoft.Font.ApplicationHost.MenuItems.FileMenus
{
    [Export(typeof(IMenuItem))]
    [ParentType(typeof(FileMenuItem))]
    class OpenFontMenuItem : MenuItemBase
    {
        private readonly Lazy<IShell> shell;

        [ImportingConstructor]
        public OpenFontMenuItem(Lazy<IShell> shell)
        {
            this.shell = shell;
            this.DisplayName = "Open Font...";
            this.InputGesture = new KeyGesture(Key.O, ModifierKeys.Control);
        }

        protected override bool OnCanExecute(object parameter) => this.Shell.IsProgressing == false;

        protected override async void OnExecute(object parameter)
        {
            var dialog = new OpenFileDialog()
            {
                Filter = "font files (*.otf)|*.otf|all files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true,
            };

            if (dialog.ShowDialog() == true)
            {
                await this.Shell.OpenAsync(dialog.FileName);
            }
        }

        private IShell Shell => this.shell.Value;
    }
}
