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
    [Dependency(typeof(SaveSettingsMenuItem))]
    [CategoryName("Settings")]
    class SaveSettingsAsMenuItem : MenuItemBase
    {
        private readonly IShell shell;

        [ImportingConstructor]
        public SaveSettingsAsMenuItem(IShell shell)
        {
            this.shell = shell;
            this.DisplayName = "Save Settings as...";
            this.InputGesture = new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift); 
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
