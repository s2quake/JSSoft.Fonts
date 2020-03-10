using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font.ApplicationHost.ContextMenus.CharacterGroup
{
    [Export(typeof(IMenuItem))]
    [ParentType(typeof(ICharacterGroup))]
    [DefaultMenu]
    class UncheckAllMenu : MenuItemBase<ICharacterGroup>
    {
        public UncheckAllMenu()
        {
            this.HideOnDisabled = true;
            this.DisplayName = "Uncheck All";
        }

        protected override bool OnCanExecute(ICharacterGroup obj)
        {
            return obj.IsChecked != false;
        }

        protected override void OnExecute(ICharacterGroup obj)
        {
            obj.IsChecked = false;
        }
    }
}
