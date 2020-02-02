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

namespace JSSoft.Font.ApplicationHost.MenuItems.ViewMenus
{
    [Export(typeof(IMenuItem))]
    [ParentType(typeof(ViewMenuItem))]
    class PreviewMenuItem : MenuItemBase
    {
        private readonly Lazy<IShell> shell;

        [ImportingConstructor]
        public PreviewMenuItem(Lazy<IShell> shell)
        {
            this.shell = shell;
            this.DisplayName = "Preview";
        }

        protected override bool OnCanExecute(object parameter)
        {
            return this.Shell.IsProgressing == false && this.Shell.IsOpened == true;
        }

        protected override void OnExecute(object parameter)
        {
            base.OnExecute(parameter);

            
        }

        private IShell Shell => this.shell.Value;
    }
}
