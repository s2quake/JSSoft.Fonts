using JSSoft.Font.ApplicationHost.Dialogs.ViewModels;
using Ntreev.ModernUI.Framework;
using System.ComponentModel.Composition;

namespace JSSoft.Font.ApplicationHost.MenuItems.ViewMenus
{
    [Export(typeof(IMenuItem))]
    [ParentType(typeof(ViewMenuItem))]
    class PreviewMenuItem : MenuItemBase
    {
        private readonly IShell shell;

        [ImportingConstructor]
        public PreviewMenuItem(IShell shell)
        {
            this.shell = shell;
            this.DisplayName = "Preview";
            this.shell.Opened += (s, e) => this.InvokeCanExecuteChangedEvent();
            this.shell.Closed += (s, e) => this.InvokeCanExecuteChangedEvent();
        }

        protected override bool OnCanExecute(object parameter)
        {
            return this.shell.IsProgressing == false && this.shell.IsOpened == true;
        }

        protected async override void OnExecute(object parameter)
        {
            var images = await this.shell.PreviewAsync();
            var dialog = new PreviewViewModel(images);
            if (dialog.ShowDialog() == true)
            {

            }
        }
    }
}
