using Microsoft.Win32;
using Ntreev.Library;
using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font.ApplicationHost.ToolBarItems
{
    [Export(typeof(IToolBarItem))]
    [ParentType(typeof(IShell))]
    [Order(-1)]
    class OpenFontToolBarItem : ToolBarItemBase
    {
        private readonly Lazy<IShell> shell;

        [ImportingConstructor]
        public OpenFontToolBarItem(Lazy<IShell> shell)
        {
            this.shell = shell;
            this.Icon = "/JSSoft.Font.ApplicationHost;component/Images/open-folder-with-document.png";
        }

        protected override bool OnCanExecute(object parameter) => this.Shell.IsProgressing == false;

        protected override async void OnExecute(object parameter)
        {
            base.OnExecute(parameter);

            var dialog = new OpenFileDialog()
            {
                Filter = "font files (*.otf)|*.otf|all files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true,
            };

            if (dialog.ShowDialog() == true)
            {
                await this.Shell.OpenAsync(dialog.FileName);
            }
        }

        private IShell Shell => this.shell.Value;
    }
}
