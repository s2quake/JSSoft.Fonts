using JSSoft.Font.ApplicationHost.UndoActions;
using Ntreev.ModernUI.Framework;
using System.ComponentModel.Composition;

namespace JSSoft.Font.ApplicationHost.ContextMenus.Character
{
    [Export(typeof(IMenuItem))]
    [ParentType(typeof(ICharacter))]
    [DefaultMenu]
    class CheckMenu : MenuItemBase<ICharacter>
    {
        private readonly IUndoService undoService;

        [ImportingConstructor]
        public CheckMenu(IUndoService undoService)
        {
            this.undoService = undoService;
            this.HideOnDisabled = true;
            this.DisplayName = "Check";
        }

        protected override bool OnCanExecute(ICharacter obj)
        {
            return obj.IsEnabled == true && obj.IsChecked == false;
        }

        protected override void OnExecute(ICharacter obj)
        {
            this.undoService.Execute(new CheckCharacterAction(obj));
        }
    }
}
