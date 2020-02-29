using JSSoft.Font.ApplicationHost.Commands;
using JSSoft.Font.ApplicationHost.Dialogs.ViewModels;
using JSSoft.Font.ApplicationHost.Properties;
using Microsoft.Win32;
using Ntreev.Library;
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
    [Order(0)]
    class OpenFontMenuItem : MenuItemBase
    {
        private readonly IShell shell;

        [ImportingConstructor]
        public OpenFontMenuItem(IServiceProvider serviceProvider, IShell shell)
            : base(serviceProvider)
        {
            this.shell = shell;
            this.DisplayName = "Open Font...";
            this.InputGesture = new KeyGesture(Key.O, ModifierKeys.Control);
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
