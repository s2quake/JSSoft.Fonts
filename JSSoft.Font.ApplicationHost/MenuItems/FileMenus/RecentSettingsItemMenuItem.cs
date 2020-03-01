using Ntreev.ModernUI.Framework;
using System;

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
            try
            {
                await this.Shell.LoadSettingsAsync(this.Filename);
            }
            catch (Exception e)
            {
                AppMessageBox.ShowError(e);
            }
        }

        private IShell Shell { get; }
    }
}
