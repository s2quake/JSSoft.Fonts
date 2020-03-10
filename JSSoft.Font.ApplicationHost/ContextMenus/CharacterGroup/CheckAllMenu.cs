using JSSoft.Font.ApplicationHost.UndoActions;
using Ntreev.ModernUI.Framework;
using System.ComponentModel.Composition;

namespace JSSoft.Font.ApplicationHost.ContextMenus.CharacterGroup
{
    [Export(typeof(IMenuItem))]
    [ParentType(typeof(ICharacterGroup))]
    [DefaultMenu]
    class CheckAllMenu : MenuItemBase<ICharacterGroup>
    {
        private readonly IUndoService undoService;

        [ImportingConstructor]
        public CheckAllMenu(IUndoService undoService)
        {
            this.undoService = undoService;
            this.HideOnDisabled = true;
            this.DisplayName = "Check All";
        }

        protected override bool OnCanExecute(ICharacterGroup obj)
        {
            return obj.IsChecked != true;
        }

        protected override void OnExecute(ICharacterGroup obj)
        {
            this.undoService.Execute(new CheckCharacterGroupAction(obj));
        }
    }
}
