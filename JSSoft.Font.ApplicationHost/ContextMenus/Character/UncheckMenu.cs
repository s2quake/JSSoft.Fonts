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
    class UncheckMenu : MenuItemBase<ICharacter>
    {
        public UncheckMenu()
        {
            this.HideOnDisabled = true;
            this.DisplayName = "Uncheck";
        }

        protected override bool OnCanExecute(ICharacter obj)
        {
            return obj.IsEnabled == true && obj.IsChecked == true;
        }

        protected override void OnExecute(ICharacter obj)
        {
            obj.IsChecked = false;
        }
    }
}
