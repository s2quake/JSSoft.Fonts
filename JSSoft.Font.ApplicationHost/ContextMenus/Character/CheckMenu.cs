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
    [DefaultMenu]
    class CheckMenu : MenuItemBase
    {
        public CheckMenu()
        {
            this.HideOnDisabled = true;
            this.DisplayName = "Check";
        }

        protected override bool OnCanExecute(object parameter)
        {
            if (parameter is ICharacter character && character.IsEnabled == true)
            {
                return character.IsChecked == false;
            }
            return false;
        }

        protected override void OnExecute(object parameter)
        {
            if (parameter is ICharacter character && character.IsEnabled == true)
            {
                character.IsChecked = true;
            }
        }
    }
}
