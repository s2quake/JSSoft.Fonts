using JSSoft.Font.ApplicationHost.Commands;
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
    class ExportFontMenuItem : MenuItemBase
    {
        private readonly IShell shell;

        [ImportingConstructor]
        public ExportFontMenuItem(IServiceProvider serviceProvider, IShell shell)
            : base(serviceProvider)
        {
            this.shell = shell;
            this.DisplayName = "Export Font...";
            this.InputGesture = new KeyGesture(Key.E, ModifierKeys.Control);
            this.shell.Opened += (s, e) => this.InvokeCanExecuteChangedEvent();
            this.shell.Closed += (s, e) => this.InvokeCanExecuteChangedEvent();
        }

        protected override bool OnCanExecute(object parameter) => ExportFontCommand.CanExecute(this.shell);

        protected async override void OnExecute(object parameter)
        {
            try
            {
                await ExportFontCommand.ExecuteAsync(this.shell);
            }
            catch (Exception e)
            {
                AppMessageBox.ShowError(e);
            }
        }
    }
}
