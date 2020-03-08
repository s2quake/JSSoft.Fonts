using JSSoft.Font.ApplicationHost.Commands;
using Ntreev.Library;
using Ntreev.ModernUI.Framework;
using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;

namespace JSSoft.Font.ApplicationHost.MenuItems.FileMenus
{
    [Export(typeof(IMenuItem))]
    [ParentType(typeof(FileMenuItem))]
    [Category("Quit")]
    [Order(int.MaxValue)]
    class QuitMenuItem : MenuItemBase
    {
        private readonly IShell shell;

        [ImportingConstructor]
        public QuitMenuItem(IShell shell)
        {
            this.shell = shell;
            this.DisplayName = "Quit";
            this.InputGesture = new KeyGesture(Key.F4, ModifierKeys.Alt);
            this.shell.Opened += (s, e) => this.InvokeCanExecuteChangedEvent();
            this.shell.Closed += (s, e) => this.InvokeCanExecuteChangedEvent();
        }

        protected override bool OnCanExecute(object parameter) => QuitCommand.CanExecute(this.shell);

        protected override async void OnExecute(object parameter)
        {
            try
            {
                await QuitCommand.ExecuteAsync(this.shell);
            }
            catch (Exception e)
            {
                AppMessageBox.ShowError(e);
            }
        }
    }
}
