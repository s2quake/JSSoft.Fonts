using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font.ApplicationHost.UndoActions
{
    class UncheckCharacterAction : UndoBase
    {
        private readonly ICharacter character;

        public UncheckCharacterAction(ICharacter character)
        {
            this.character = character;
        }

        public override string ToString()
        {
            return $"Uncheck Character: {(char)this.character.ID}";
        }

        protected override void OnRedo()
        {
            this.character.IsChecked = false;
        }

        protected override void OnUndo()
        {
            this.character.IsChecked = true;
        }
    }
}
