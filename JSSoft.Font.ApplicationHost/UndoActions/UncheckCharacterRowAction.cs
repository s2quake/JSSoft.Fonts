using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font.ApplicationHost.UndoActions
{
    class UncheckCharacterRowAction : UndoBase
    {
        private readonly ICharacterRow row;
        private readonly bool?[] items;

        public UncheckCharacterRowAction(ICharacterRow row)
        {
            this.row = row;
            this.items = new bool?[row.Items.Length];
            for (var i = 0; i < this.items.Length; i++)
            {
                var character = row.Items[i];
                if (character.IsEnabled == true)
                {
                    this.items[i] = character.IsChecked;
                }
            }
        }

        public override string ToString()
        {
            return $"Uncheck Row: {(char)this.row.Index}";
        }

        protected override void OnRedo()
        {
            this.row.IsChecked = false;
        }

        protected override void OnUndo()
        {
            for (var i = 0; i < this.items.Length; i++)
            {
                var isChecked = this.items[i];
                var character = this.row.Items[i];
                if (isChecked != null && character.IsChecked != isChecked.Value)
                {
                    character.IsChecked = isChecked.Value;
                }
            }
        }
    }
}
