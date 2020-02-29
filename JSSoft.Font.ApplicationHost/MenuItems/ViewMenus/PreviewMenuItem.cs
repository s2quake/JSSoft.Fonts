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
        private readonly IShell shell;

        [ImportingConstructor]
        public PreviewMenuItem(IServiceProvider serviceProvider, IShell shell)
            : base(serviceProvider)
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
