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
    class ExportSettingsMenuItem : MenuItemBase
    {
        private readonly Lazy<IShell> shell;

        [ImportingConstructor]
        public ExportSettingsMenuItem(Lazy<IShell> shell)
        {
            this.shell = shell;
            this.DisplayName = "Export Settings...";
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

        protected override void OnExecute(object parameter)
        {
            var settings = this.Shell.ExportSettings;
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

        private IShell Shell => this.shell.Value;
    }
}
