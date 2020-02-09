using Microsoft.Win32;
using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font.ApplicationHost.MenuItems.FileMenus
{
    [Export(typeof(IMenuItem))]
    [ParentType(typeof(FileMenuItem))]
    class LoadSettingsMenuItem : MenuItemBase
    {
        private readonly Lazy<IShell> shell;

        [ImportingConstructor]
        public LoadSettingsMenuItem(Lazy<IShell> shell)
        {
            this.shell = shell;
            this.DisplayName = "Load Settings...";
        }

        protected override bool OnCanExecute(object parameter)
        {
            return this.Shell.IsProgressing == false;
        }

        protected async override void OnExecute(object parameter)
        {
            var dialog = new OpenFileDialog()
            {
                Filter = "settings files (*.xml)|*.xml|all files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true,
            };

            if (dialog.ShowDialog() == true)
            {
                await this.Shell.LoadSettingsAsync(dialog.FileName);
            }
        }

        private IShell Shell => this.shell.Value;
    }
}
