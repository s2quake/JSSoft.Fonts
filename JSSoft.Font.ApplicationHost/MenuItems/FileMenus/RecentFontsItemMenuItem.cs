using JSSoft.Font.ApplicationHost.Commands;
using Ntreev.ModernUI.Framework;
using System;

namespace JSSoft.Font.ApplicationHost.MenuItems.FileMenus
{
    [ParentType(typeof(RecentSettingsMenuItem))]
    class RecentFontsItemMenuItem : MenuItemBase
    {
        public RecentFontsItemMenuItem(IShell shell, string filename)
        {
            this.Shell = shell;
            this.Filename = filename;
            this.DisplayName = filename;
        }

        public string Filename { get; }

        protected override bool OnCanExecute(object parameter)
        {
            return OpenFontCommand.CanExecute(this.Shell);
        }

        protected async override void OnExecute(object parameter)
        {
            try
            {
                await OpenFontCommand.ExecuteAsync(this.Shell, this.Filename);
            }
            catch (Exception e)
            {
                AppMessageBox.ShowError(e);
            }
        }

        private IShell Shell { get; }
    }
}
