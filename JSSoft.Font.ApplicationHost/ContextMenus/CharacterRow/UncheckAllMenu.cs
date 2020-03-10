using JSSoft.Font.ApplicationHost.UndoActions;
using Ntreev.ModernUI.Framework;
using System.ComponentModel.Composition;

namespace JSSoft.Font.ApplicationHost.ContextMenus.CharacterRow
{
    [Export(typeof(IMenuItem))]
    [ParentType(typeof(ICharacterRow))]
    [DefaultMenu]
    class UncheckAllMenu : MenuItemBase<ICharacterRow>
    {
        private readonly IUndoService undoService;

        [ImportingConstructor]
        public UncheckAllMenu(IUndoService undoService)
        {
            this.undoService = undoService;
            this.HideOnDisabled = true;
            this.DisplayName = "Uncheck All";
        }

        protected override bool OnCanExecute(ICharacterRow obj)
        {
            return obj.IsChecked != false;
        }

        protected override void OnExecute(ICharacterRow obj)
        {
            this.undoService.Execute(new UncheckCharacterRowAction(obj));
        }
    }
}
