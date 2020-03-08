using JSSoft.Font.ApplicationHost.Dialogs.ViewModels;
using Ntreev.Library;
using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font.ApplicationHost.ContextMenus.Character
{
    [Export(typeof(IMenuItem))]
    [ParentType(typeof(ICharacter))]
    [Order(1)]
    class PrewViewMenu : MenuItemBase
    {
        public PrewViewMenu()
        {
            this.DisplayName = "Preview";
        }

        protected override bool OnCanExecute(object parameter)
        {
            if (parameter is ICharacter character && character.IsEnabled == true)
            {
                return true;
            }
            return false;
        }

        protected override void OnExecute(object parameter)
        {
            if (parameter is ICharacter character && character.IsEnabled == true)
            {
                var dialog = new PreviewCharacterViewModel(character);
                dialog.ShowDialog();
            }
        }
    }
}
