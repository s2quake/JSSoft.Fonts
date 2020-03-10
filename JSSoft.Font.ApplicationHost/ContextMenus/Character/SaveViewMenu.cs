using JSSoft.Font.ApplicationHost.Dialogs.ViewModels;
using Microsoft.Win32;
using Ntreev.Library;
using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace JSSoft.Font.ApplicationHost.ContextMenus.Character
{
    [Export(typeof(IMenuItem))]
    [ParentType(typeof(ICharacter))]
    [Order(2)]
    class SaveViewMenu : MenuItemBase<ICharacter>
    {
        public SaveViewMenu()
        {
            this.DisplayName = "Save";
        }

        protected override bool OnCanExecute(ICharacter obj)
        {
            return obj.IsEnabled == true;
        }

        protected override void OnExecute(ICharacter obj)
        {
            var dialog = new SaveFileDialog()
            {
                Filter = "png files (*.png)|*.png|all files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true,
            };
            if (dialog.ShowDialog() == true)
            {
                using (var stream = new FileStream(dialog.FileName, FileMode.Create))
                {
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(obj.Source));
                    encoder.Save(stream);
                }
            }
        }
    }
}
