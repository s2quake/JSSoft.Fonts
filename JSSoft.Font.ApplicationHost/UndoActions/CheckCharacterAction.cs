using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font.ApplicationHost.UndoActions
{
    class CheckCharacterAction : UndoBase
    {
        private readonly ICharacter character;

        public CheckCharacterAction(ICharacter character)
        {
            this.character = character;
        }

        public override string ToString()
        {
            return $"Check Character: {(char)this.character.ID}";
        }

        protected override void OnRedo()
        {
            this.character.IsChecked = true;
        }

        protected override void OnUndo()
        {
            this.character.IsChecked = false;
        }
    }
}
