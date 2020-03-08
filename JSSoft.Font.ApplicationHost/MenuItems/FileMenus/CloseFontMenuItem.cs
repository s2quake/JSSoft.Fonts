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
