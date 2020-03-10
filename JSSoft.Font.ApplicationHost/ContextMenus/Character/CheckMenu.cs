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
    class CheckMenu : MenuItemBase<ICharacter>
    {
        public CheckMenu()
        {
            this.HideOnDisabled = true;
            this.DisplayName = "Check";
        }

        protected override bool OnCanExecute(ICharacter obj)
        {
            return obj.IsEnabled == true && obj.IsChecked == false;
        }

        protected override void OnExecute(ICharacter obj)
        {
            obj.IsChecked = true;
        }
    }
}
