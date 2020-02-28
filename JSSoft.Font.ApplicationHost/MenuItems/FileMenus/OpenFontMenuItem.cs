using JSSoft.Font.ApplicationHost.Commands;
using JSSoft.Font.ApplicationHost.Dialogs.ViewModels;
using JSSoft.Font.ApplicationHost.Input;
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
        private readonly Lazy<IShell> shell;

        [ImportingConstructor]
        public OpenFontMenuItem(Lazy<IShell> shell)
        {
            this.shell = shell;
            this.DisplayName = "Open Font...";
            //this.InputGesture = new KeyGesture(Key.O, ModifierKeys.Control);
        }

        public override ICommand Command => FontCommands.OpenFont;

        protected override bool OnCanExecute(object parameter) => OpenFontCommand.CanExecute(this.Shell);

        protected override void OnExecute(object parameter) => OpenFontCommand.Execute(this.Shell);

        private IShell Shell => this.shell.Value;
    }
}
