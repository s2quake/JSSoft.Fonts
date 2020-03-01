using Ntreev.ModernUI.Framework;
using System.ComponentModel.Composition;
using System.Windows.Input;

namespace JSSoft.Font.ApplicationHost.MenuItems.ViewMenus
{
    [Export(typeof(IMenuItem))]
    [ParentType(typeof(ViewMenuItem))]
    class ZoomInMenuItem : MenuItemBase
    {
        private readonly IShell shell;

        [ImportingConstructor]
        public ZoomInMenuItem(IShell shell)
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
