using JSSoft.Font.ApplicationHost.UndoActions;
using Ntreev.ModernUI.Framework;
using System.ComponentModel.Composition;

namespace JSSoft.Font.ApplicationHost.ContextMenus.Character
{
    [Export(typeof(IMenuItem))]
    [ParentType(typeof(ICharacter))]
    [DefaultMenu]
    class UncheckMenu : MenuItemBase<ICharacter>
    {
        private readonly IUndoService undoService;

        [ImportingConstructor]
        public UncheckMenu(IUndoService undoService)
        {
            this.undoService = undoService;
            this.HideOnDisabled = true;
            this.DisplayName = "Uncheck";
        }

        protected override bool OnCanExecute(ICharacter obj)
        {
            return obj.IsEnabled == true && obj.IsChecked == true;
        }

        protected override void OnExecute(ICharacter obj)
        {
            this.undoService.Execute(new UncheckCharacterAction(obj));
        }
    }
}
