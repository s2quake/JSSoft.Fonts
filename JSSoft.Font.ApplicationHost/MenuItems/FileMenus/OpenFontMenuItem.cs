using JSSoft.Font.ApplicationHost.Dialogs.ViewModels;
using JSSoft.Font.ApplicationHost.Properties;
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

namespace JSSoft.Font.ApplicationHost.MenuItems.FileMenus
{
    [Export(typeof(IMenuItem))]
    [ParentType(typeof(FileMenuItem))]
    [Order(0)]
    class OpenFontMenuItem : MenuItemBase
    {
        private readonly Lazy<IShell> shell;

        [ImportingConstructor]
        public OpenFontMenuItem(Lazy<IShell> shell)
        {
            this.shell = shell;
            this.DisplayName = "Open Font...";
            this.InputGesture = new KeyGesture(Key.O, ModifierKeys.Control);
        }

        protected override bool OnCanExecute(object parameter) => this.Shell.IsProgressing == false;

        protected override async void OnExecute(object parameter)
        {
            var dialog = new OpenFileDialog()
            {
                Filter = Resources.FontFilter,
                FilterIndex = 1,
                RestoreDirectory = true,
            };

            if (dialog.ShowDialog() == true)
            {
                var faceIndex = this.SelectFace(dialog.FileName);
                await this.Shell.OpenAsync(dialog.FileName, faceIndex);
            }
        }

        private int SelectFace(string path)
        {
            var faces = FontDescriptor.GetFaces(path);
            if (faces.Length == 1)
                return 0;

            var dialog = new SelectFaceViewModel(faces.ToArray());
            if (dialog.ShowDialog() == true)
            {
                for (var i = 0; i < faces.Length; i++)
                {
                    if (faces[i] == dialog.Face)
                        return i;
                }
            }
            return 0;
        }

        private IShell Shell => this.shell.Value;
    }
}
