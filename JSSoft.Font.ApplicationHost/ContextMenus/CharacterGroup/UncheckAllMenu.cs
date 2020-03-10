using JSSoft.Font.ApplicationHost.UndoActions;
using Ntreev.ModernUI.Framework;
using System.ComponentModel.Composition;

namespace JSSoft.Font.ApplicationHost.ContextMenus.CharacterGroup
{
    [Export(typeof(IMenuItem))]
    [ParentType(typeof(ICharacterGroup))]
    [DefaultMenu]
    class UncheckAllMenu : MenuItemBase<ICharacterGroup>
    {
        private readonly IUndoService undoService;

        [ImportingConstructor]
        public UncheckAllMenu(IUndoService undoService)
        {
            this.undoService = undoService;
            this.HideOnDisabled = true;
            this.DisplayName = "Uncheck All";
        }

        protected override bool OnCanExecute(ICharacterGroup obj)
        {
            return obj.IsChecked != false;
        }

        protected override void OnExecute(ICharacterGroup obj)
        {
            this.undoService.Execute(new UncheckCharacterGroupAction(obj));
        }
    }
}
