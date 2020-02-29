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
    public static class LoadSettingsCommand
    {
        public static bool CanExecute(IShell shell)
        {
            return shell.IsProgressing == false;
        }

        public static async Task ExecuteAsync(IShell shell)
        {
            var dialog = new OpenFileDialog()
            {
                Filter = "settings files (*.jsfs)|*.jsfs|all files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true,
            };

            if (dialog.ShowDialog() == true)
            {
                await shell.LoadSettingsAsync(dialog.FileName);
            }
        }
    }
}
