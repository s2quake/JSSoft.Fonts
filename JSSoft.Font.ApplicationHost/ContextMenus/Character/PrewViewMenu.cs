using JSSoft.Font.ApplicationHost.Dialogs.ViewModels;
using Ntreev.Library;
using Ntreev.ModernUI.Framework;
using System.ComponentModel.Composition;

namespace JSSoft.Font.ApplicationHost.ContextMenus.Character
{
    [Export(typeof(IMenuItem))]
    [ParentType(typeof(ICharacter))]
    [Order(1)]
    class PrewViewMenu : MenuItemBase<ICharacter>
    {
        public PrewViewMenu()
        {
            this.DisplayName = "Preview";
        }

        protected override bool OnCanExecute(ICharacter obj)
        {
            return obj.IsEnabled == true;
        }

        protected override void OnExecute(ICharacter obj)
        {
            var dialog = new PreviewCharacterViewModel(obj);
            dialog.ShowDialog();
        }
    }
}
