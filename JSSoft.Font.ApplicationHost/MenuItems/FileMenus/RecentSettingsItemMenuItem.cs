using Microsoft.Win32;
using Ntreev.Library;
using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font.ApplicationHost.MenuItems.FileMenus
{
    [ParentType(typeof(RecentSettingsMenuItem))]
    class RecentSettingsItemMenuItem : MenuItemBase
    {
        public RecentSettingsItemMenuItem(IShell shell, string filename)
        {
            this.Shell = shell;
            this.Filename = filename;
            this.DisplayName = filename;
        }

        public string Filename { get; }

        protected override bool OnCanExecute(object parameter)
        {
            return this.Shell.IsProgressing == false;
        }

        protected async override void OnExecute(object parameter)
        {
            await this.Shell.LoadSettingsAsync(this.Filename);
        }

        private IShell Shell { get; }
    }
}
