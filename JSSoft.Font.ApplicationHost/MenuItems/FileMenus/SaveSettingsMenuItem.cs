using JSSoft.Font.ApplicationHost.Commands;
using Ntreev.ModernUI.Framework;
using System;
using System.ComponentModel.Composition;

namespace JSSoft.Font.ApplicationHost.MenuItems.FileMenus
{
    [Export(typeof(IMenuItem))]
    [ParentType(typeof(FileMenuItem))]
    class SaveSettingsMenuItem : MenuItemBase
    {
        private readonly IShell shell;

        [ImportingConstructor]
        public SaveSettingsMenuItem(IShell shell)
        {
            this.shell = shell;
            this.DisplayName = "Save Settings...";
            this.shell.Opened += (s, e) => this.InvokeCanExecuteChangedEvent();
            this.shell.Closed += (s, e) => this.InvokeCanExecuteChangedEvent();
        }

        protected override bool OnCanExecute(object parameter) => SaveSettingsCommand.CanExecute(this.shell);

        protected async override void OnExecute(object parameter)
        {
            try
            {
                await SaveSettingsCommand.ExecuteAsync(this.shell);
            }
            catch (Exception e)
            {
                AppMessageBox.ShowError(e);
            }
        }
    }
}
