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
    [Order(0)]
    class OpenFontMenuItem : MenuItemBase
    {
        private readonly IShell shell;

        [ImportingConstructor]
        public OpenFontMenuItem(IShell shell)
        {
            this.shell = shell;
            this.DisplayName = "Open Font...";
            this.InputGesture = new KeyGesture(Key.O, ModifierKeys.Control);
            this.shell.Opened += (s, e) => this.InvokeCanExecuteChangedEvent();
            this.shell.Closed += (s, e) => this.InvokeCanExecuteChangedEvent();
        }

        protected override bool OnCanExecute(object parameter) => OpenFontCommand.CanExecute(this.shell);

        protected override async void OnExecute(object parameter)
        {
            try
            {
                await OpenFontCommand.ExecuteAsync(this.shell);
            }
            catch (Exception e)
            {
                AppMessageBox.ShowError(e);
            }
        }
    }
}
