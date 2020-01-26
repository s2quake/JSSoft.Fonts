using Microsoft.Win32;
using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font.ApplicationHost.MenuItems.FileMenus
{
    [Export(typeof(IMenuItem))]
    [ParentType(typeof(FileMenuItem))]
    class ExportFontMenuItem : MenuItemBase
    {
        private readonly Lazy<IShell> shell;

        [ImportingConstructor]
        public ExportFontMenuItem(Lazy<IShell> shell)
        {
            this.shell = shell;
            this.DisplayName = "Export Font...";
        }

        //protected override bool OnCanExecute(object parameter)
        //{
        //    return this.Shell.IsProgressing == false;
        //}

        protected override void OnExecute(object parameter)
        {
            base.OnExecute(parameter);

           
        }

        private IShell Shell => this.shell.Value;
    }
}
