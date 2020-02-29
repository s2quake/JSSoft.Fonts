using JSSoft.Font.ApplicationHost.Dialogs.ViewModels;
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

namespace JSSoft.Font.ApplicationHost.MenuItems.ToolMenus
{
    [Export(typeof(IMenuItem))]
    [ParentType(typeof(ToolMenuItem))]
    class SelectCharactersMenuItem : MenuItemBase
    {
        private readonly IShell shell;

        [ImportingConstructor]
        public SelectCharactersMenuItem(IServiceProvider serviceProvider, IShell shell)
            : base(serviceProvider)
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
