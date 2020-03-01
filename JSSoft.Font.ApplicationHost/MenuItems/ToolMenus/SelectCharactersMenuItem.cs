using JSSoft.Font.ApplicationHost.Dialogs.ViewModels;
using Ntreev.ModernUI.Framework;
using System.ComponentModel.Composition;

namespace JSSoft.Font.ApplicationHost.MenuItems.ToolMenus
{
    [Export(typeof(IMenuItem))]
    [ParentType(typeof(ToolMenuItem))]
    class SelectCharactersMenuItem : MenuItemBase
    {
        private readonly IShell shell;

        [ImportingConstructor]
        public SelectCharactersMenuItem(IShell shell)
        {
            this.shell = shell;
            this.DisplayName = "Select Characters...";
            this.shell.Opened += (s, e) => this.InvokeCanExecuteChangedEvent();
            this.shell.Closed += (s, e) => this.InvokeCanExecuteChangedEvent();
        }

        protected override bool OnCanExecute(object parameter)
        {
            return this.shell.IsProgressing == false;
        }

        protected override async void OnExecute(object parameter)
        {
            var dialog = new SelectCharactersViewModel(this.shell.SelectedCharacters);
            if (dialog.ShowDialog() == true)
            {
                await this.shell.SelectCharactersAsync(dialog.Characters);
            }
        }
    }
}
