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
    class SaveSettingsMenuItem : MenuItemBase
    {
        private readonly Lazy<IShell> shell;

        [ImportingConstructor]
        public SaveSettingsMenuItem(Lazy<IShell> shell)
        {
            this.shell = shell;
            this.DisplayName = "Save Settings...";
            this.Dispatcher.InvokeAsync(() =>
            {
                this.Shell.Opened += (s, e) => this.InvokeCanExecuteChangedEvent();
                this.Shell.Closed += (s, e) => this.InvokeCanExecuteChangedEvent();
            });
        }

        protected override bool OnCanExecute(object parameter)
        {
            return this.Shell.IsProgressing == false && this.Shell.IsOpened == true;
        }

        protected async override void OnExecute(object parameter)
        {
            var dialog = new SaveFileDialog()
            {
                Filter = "settings files (*.xml)|*.xml|all files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true,
            };

            if (dialog.ShowDialog() == true)
            {
                await this.Shell.SaveSettingsAsync(dialog.FileName);
            }
        }

        private IShell Shell => this.shell.Value;
    }
}
