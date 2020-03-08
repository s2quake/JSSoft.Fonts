using JSSoft.Font.ApplicationHost.Commands;
using Ntreev.ModernUI.Framework;
using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows.Input;

namespace JSSoft.Font.ApplicationHost.MenuItems.FileMenus
{
    [Export(typeof(IMenuItem))]
    [ParentType(typeof(FileMenuItem))]
    [CategoryName("Settings")]
    class SaveSettingsMenuItem : MenuItemBase
    {
        private readonly IShell shell;

        [ImportingConstructor]
        public SaveSettingsMenuItem(IShell shell)
        {
            this.shell = shell;
            this.DisplayName = "Save Settings...";
            this.InputGesture = new KeyGesture(Key.S, ModifierKeys.Control);
            this.shell.Opened += (s, e) => this.InvokeCanExecuteChangedEvent();
            this.shell.Closed += (s, e) => this.InvokeCanExecuteChangedEvent();
        }

        protected override bool OnCanExecute(object parameter) => SaveSettingsCommand.CanExecute(this.shell);

        protected async override void OnExecute(object parameter)
        {
            try
            {
                if (this.shell.SettingsPath != string.Empty)
                    await this.shell.SaveSettingsAsync();
                else
                    await SaveSettingsCommand.ExecuteAsync(this.shell);
            }
            catch (Exception e)
            {
                AppMessageBox.ShowError(e);
            }
        }
    }
}
