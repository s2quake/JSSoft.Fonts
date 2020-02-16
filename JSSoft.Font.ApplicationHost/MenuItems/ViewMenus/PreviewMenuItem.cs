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
            this.Dispatcher.InvokeAsync(() =>
            {
                this.Shell.Opened += (s, e) => this.InvokeCanExecuteChangedEvent();
                this.Shell.Closed += (s, e) => this.InvokeCanExecuteChangedEvent();
            });
        }

        protected override bool OnCanExecute(object parameter)
        {
            return this.Shell.IsProgressing == false && this.Shell.IsOpened == true;
        }

        protected async override void OnExecute(object parameter)
        {
            var images = await this.Shell.PreviewAsync();
            var dialog = new PreviewViewModel(images);
            if (dialog.ShowDialog() == true)
            {

            }
        }

        private IShell Shell => this.shell.Value;
    }
}
