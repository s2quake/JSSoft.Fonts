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
    class ZoomInMenuItem : MenuItemBase
    {
        private readonly IShell shell;

        [ImportingConstructor]
        public ZoomInMenuItem(IServiceProvider serviceProvider, IShell shell)
            : base(serviceProvider)
        {
            this.shell = shell;
            this.DisplayName = "Zoom In";
            this.InputGesture = new KeyGesture(Key.OemPlus, ModifierKeys.Control);
            this.Icon = "Images/zoom-in.png";
        }

        protected override bool OnCanExecute(object parameter) => this.shell.IsProgressing == false;

        protected override void OnExecute(object parameter) => this.shell.ZoomLevel *= 2.0;
    }
}
