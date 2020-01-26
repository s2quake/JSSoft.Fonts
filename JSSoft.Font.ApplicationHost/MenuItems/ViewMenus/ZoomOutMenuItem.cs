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
    class ZoomOutMenuItem : MenuItemBase
    {
        private readonly Lazy<IShell> shell;

        [ImportingConstructor]
        public ZoomOutMenuItem(Lazy<IShell> shell)
        {
            this.shell = shell;
            this.DisplayName = "Zoom Out";
            this.InputGesture = new KeyGesture(Key.OemMinus, ModifierKeys.Control);
        }

        protected override bool OnCanExecute(object parameter)
        {
            return this.Shell.IsProgressing == false;
        }

        protected override void OnExecute(object parameter)
        {
            base.OnExecute(parameter);

            this.Shell.ZoomLevel *= 2.0;
        }

        private IShell Shell => this.shell.Value;
    }
}
