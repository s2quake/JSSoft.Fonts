using JSSoft.Font.ApplicationHost.Dialogs.ViewModels;
using JSSoft.Font.ApplicationHost.Properties;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font.ApplicationHost.Commands
{
    public static class SaveSettingsCommand
    {
        public static bool CanExecute(IShell shell)
        {
            return shell.IsProgressing == false && shell.IsOpened == true;
        }

        public static async Task ExecuteAsync(IShell shell)
        {
            var dialog = new SaveFileDialog()
            {
                Filter = "settings files (*.jsfs)|*.jsfs|all files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true,
            };

            if (dialog.ShowDialog() == true)
            {
                await shell.SaveSettingsAsync(dialog.FileName);
            }
        }
    }
}
