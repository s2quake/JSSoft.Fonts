using JSSoft.Font.ApplicationHost.Dialogs.ViewModels;
using Ntreev.ModernUI.Framework;
using System.ComponentModel.Composition;
using System.Windows.Input;

namespace JSSoft.Font.ApplicationHost.MenuItems.ViewMenus
{
    [Export(typeof(IMenuItem))]
    [ParentType(typeof(ViewMenuItem))]
    class HidePropertyWindowMenuItem : MenuItemBase
    {
        private readonly ShellView shellView;

        [ImportingConstructor]
        public HidePropertyWindowMenuItem(ShellView shellView)
        {
            this.shellView = shellView;
            this.HideOnDisabled = true;
            this.DisplayName = "Hide Property Window";
            ShellView.ShowPropertyWindow.CanExecuteChanged += ShowPropertyWindow_CanExecuteChanged;
        }

        private void ShowPropertyWindow_CanExecuteChanged(object sender, System.EventArgs e)
        {
            this.InvokeCanExecuteChangedEvent();
        }

        protected override bool OnCanExecute(object parameter)
        {
            return ShellView.HidePropertyWindow.CanExecute(parameter, this.shellView);
        }

        protected override void OnExecute(object parameter)
        {
            ShellView.HidePropertyWindow.Execute(parameter, this.shellView);
            this.InvokeCanExecuteChangedEvent();
        }
    }
}
