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
    class UncheckAllMenu : MenuItemBase<ICharacterRow>
    {
        public UncheckAllMenu()
        {
            this.HideOnDisabled = true;
            this.DisplayName = "Uncheck All";
        }

        protected override bool OnCanExecute(ICharacterRow obj)
        {
            return obj.IsChecked != false;
        }

        protected override void OnExecute(ICharacterRow obj)
        {
            obj.IsChecked = false;
        }
    }
}
