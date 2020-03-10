using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font.ApplicationHost.ContextMenus.CharacterRow
{
    [Export(typeof(IMenuItem))]
    [ParentType(typeof(ICharacterRow))]
    [DefaultMenu]
    class CheckAllMenu : MenuItemBase<ICharacterRow>
    {
        public CheckAllMenu()
        {
            this.HideOnDisabled = true;
            this.DisplayName = "Check All";
        }

        protected override bool OnCanExecute(ICharacterRow obj)
        {
            return obj.IsChecked != true;
        }

        protected override void OnExecute(ICharacterRow obj)
        {
            obj.IsChecked = true;
        }
    }

}
