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
    class OpenFontToolBarItem : ToolBarItemBase
    {
        private readonly Lazy<IShell> shell;

        [ImportingConstructor]
        public OpenFontToolBarItem(Lazy<IShell> shell)
        {
            this.shell = shell;
            this.Icon = "/JSSoft.Font.ApplicationHost;component/Images/open-folder-with-document.png";
        }

        protected override bool OnCanExecute(object parameter)
        {
            return this.Shell.IsProgressing == false;
        }

        private IShell Shell => this.shell.Value;
    }
}
