using System.Linq;
using JSSoft.Font.ApplicationHost.Dialogs.ViewModels;
using Ntreev.ModernUI.Framework;
using System.ComponentModel.Composition;
using System.Windows.Input;

namespace JSSoft.Font.ApplicationHost.MenuItems.ToolMenus
{
    [Export(typeof(IMenuItem))]
    [ParentType(typeof(ToolMenuItem))]
    class FindCharacterMenuItem : MenuItemBase
    {
        private readonly IShell shell;

        [ImportingConstructor]
        public FindCharacterMenuItem(IShell shell)
        {
            this.shell = shell;
            this.DisplayName = "Find Character...";
            this.InputGesture = new KeyGesture(Key.F, ModifierKeys.Control);
            this.shell.Opened += (s, e) => this.InvokeCanExecuteChangedEvent();
            this.shell.Closed += (s, e) => this.InvokeCanExecuteChangedEvent();
        }

        protected override bool OnCanExecute(object parameter)
        {
            return this.shell.IsProgressing == false && this.shell.IsOpened == true;
        }

        protected override void OnExecute(object parameter)
        {
            var dialog = new FindCharacterViewModel();
            if (dialog.ShowDialog() == true)
            {
                var query = from characterGroup in this.shell.Groups
                            from row in characterGroup.Items
                            from character in row.Items
                            where character.ID == (uint)dialog.Character
                            select character;

                var item = query.FirstOrDefault();
                if (item != null)
                {
                    this.shell.SelectedCharacter = item;
                }
            }
        }
    }
}
