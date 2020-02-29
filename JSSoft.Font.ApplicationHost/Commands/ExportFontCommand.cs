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
    public static class ExportFontCommand
    {
        public static bool CanExecute(IShell shell)
        {
            return shell.IsProgressing == false && shell.IsOpened == true;
        }

        public static async Task ExecuteAsync(IShell shell)
        {
            var dialog = new SaveFileDialog()
            {
                Filter = "xml files (*.xml)|*.xml|all files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true,
            };

            if (dialog.ShowDialog() == true)
            {
                await shell.ExportAsync(dialog.FileName);
            }
        }
    }
}
