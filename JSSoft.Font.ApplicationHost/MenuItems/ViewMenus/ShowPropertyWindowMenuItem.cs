using JSSoft.Font.ApplicationHost.Dialogs.ViewModels;
using Ntreev.ModernUI.Framework;
using System.ComponentModel.Composition;
using System.Windows.Input;

namespace JSSoft.Font.ApplicationHost.MenuItems.ViewMenus
{
    [Export(typeof(IMenuItem))]
    [ParentType(typeof(ViewMenuItem))]
    class ShowPropertyWindowMenuItem : MenuItemBase
    {
        private readonly ShellView shellView;

        [ImportingConstructor]
        public ShowPropertyWindowMenuItem(ShellView shellView)
        {
            this.shellView = shellView;
            this.HideOnDisabled = true;
            this.DisplayName = "Show Property Window";
            ShellView.HidePropertyWindow.CanExecuteChanged += HidePropertyWindow_CanExecuteChanged;
        }

        private void HidePropertyWindow_CanExecuteChanged(object sender, System.EventArgs e)
        {
            this.InvokeCanExecuteChangedEvent();
        }

        protected override bool OnCanExecute(object parameter)
        {
            return ShellView.ShowPropertyWindow.CanExecute(parameter, this.shellView);
        }

        protected override void OnExecute(object parameter)
        {
            ShellView.ShowPropertyWindow.Execute(parameter, this.shellView);
            this.InvokeCanExecuteChangedEvent();
        }
    }
}
