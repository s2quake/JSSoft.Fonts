using JSSoft.Font.ApplicationHost.Commands;
using Ntreev.ModernUI.Framework;
using System;
using System.ComponentModel.Composition;

namespace JSSoft.Font.ApplicationHost.MenuItems.FileMenus
{
    [Export(typeof(IMenuItem))]
    [ParentType(typeof(FileMenuItem))]
    class LoadSettingsMenuItem : MenuItemBase
    {
        private readonly IShell shell;

        [ImportingConstructor]
        public LoadSettingsMenuItem(IShell shell)
        {
            this.shell = shell;
            this.DisplayName = "Load Settings...";
        }

        protected override bool OnCanExecute(object parameter) => LoadSettingsCommand.CanExecute(this.shell);

        protected async override void OnExecute(object parameter)
        {
            try
            {
                await LoadSettingsCommand.ExecuteAsync(this.shell);
            }
            catch (Exception e)
            {
                AppMessageBox.ShowError(e);
            }
        }
    }
}
