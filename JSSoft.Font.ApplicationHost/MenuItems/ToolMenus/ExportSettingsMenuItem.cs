using JSSoft.Font.ApplicationHost.Dialogs.ViewModels;
using Ntreev.ModernUI.Framework;
using System.ComponentModel.Composition;

namespace JSSoft.Font.ApplicationHost.MenuItems.ToolMenus
{
    [Export(typeof(IMenuItem))]
    [ParentType(typeof(ToolMenuItem))]
    class ExportSettingsMenuItem : MenuItemBase
    {
        private readonly IShell shell;

        [ImportingConstructor]
        public ExportSettingsMenuItem(IShell shell)
        {
            this.shell = shell;
            this.DisplayName = "Export Settings...";
            this.shell.Opened += (s, e) => this.InvokeCanExecuteChangedEvent();
            this.shell.Closed += (s, e) => this.InvokeCanExecuteChangedEvent();
        }

        protected override bool OnCanExecute(object parameter)
        {
            return this.shell.IsProgressing == false;
        }

        protected override void OnExecute(object parameter)
        {
            var settings = this.shell.Settings;
            var dialog = new ExportSettingsViewModel(settings);
            if (dialog.ShowDialog() == true)
            {
                settings.Padding = dialog.Padding;
                settings.HorizontalSpace = dialog.HorizontalSpace;
                settings.VerticalSpace = dialog.VerticalSpace;
                settings.TextureWidth = dialog.TextureWidth;
                settings.TextureHeight = dialog.TextureHeight;
            }
        }
    }
}
