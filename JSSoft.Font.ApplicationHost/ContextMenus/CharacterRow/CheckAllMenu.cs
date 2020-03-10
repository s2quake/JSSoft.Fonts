using JSSoft.Font.ApplicationHost.UndoActions;
using Ntreev.ModernUI.Framework;
using System.ComponentModel.Composition;

namespace JSSoft.Font.ApplicationHost.ContextMenus.CharacterRow
{
    [Export(typeof(IMenuItem))]
    [ParentType(typeof(ICharacterRow))]
    [DefaultMenu]
    class CheckAllMenu : MenuItemBase<ICharacterRow>
    {
        private readonly IUndoService undoService;

        [ImportingConstructor]
        public CheckAllMenu(IUndoService undoService)
        {
            this.undoService = undoService;
            this.HideOnDisabled = true;
            this.DisplayName = "Check All";
        }

        protected override bool OnCanExecute(ICharacterRow obj)
        {
            return obj.IsChecked != true;
        }

        protected override void OnExecute(ICharacterRow obj)
        {
            this.undoService.Execute(new CheckCharacterRowAction(obj));
        }
    }
}
