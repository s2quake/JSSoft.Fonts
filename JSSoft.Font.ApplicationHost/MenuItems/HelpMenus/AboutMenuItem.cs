using JSSoft.Font.ApplicationHost.Dialogs.ViewModels;
using Ntreev.ModernUI.Framework;
using System.ComponentModel.Composition;

namespace JSSoft.Font.ApplicationHost.MenuItems.HelpMenus
{
    [Export(typeof(IMenuItem))]
    [ParentType(typeof(HelpMenuItem))]
    class AboutMenuItem : MenuItemBase
    {
        [ImportingConstructor]
        public AboutMenuItem()
        {
            this.DisplayName = "About...";
        }

        protected override void OnExecute(object parameter)
        {
            var dialog = new AboutViewModel();
            dialog.ShowDialog();
        }
    }
}
