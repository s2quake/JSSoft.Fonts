using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font.ApplicationHost.UndoActions
{
    class UncheckCharacterGroupAction : UndoBase
    {
        private readonly ICharacterGroup group;
        private readonly bool?[][] rows;

        public UncheckCharacterGroupAction(ICharacterGroup group)
        {
            this.group = group;
            this.rows = new bool?[group.Items.Length][];
            for (var i = 0; i < this.rows.Length; i++)
            {
                this.rows[i] = new bool?[group.Items[i].Items.Length];
                for (var j = 0; j < this.rows[i].Length; j++)
                {
                    var character = group.Items[i].Items[j];
                    if (character.IsEnabled == true)
                    {
                        this.rows[i][j] = character.IsChecked;
                    }
                }
            }
        }

        public override string ToString()
        {
            return $"Uncheck Group: {this.group.Name}";
        }

        protected override void OnRedo()
        {
            this.group.IsChecked = false;
        }

        protected override void OnUndo()
        {
            for (var i = 0; i < this.rows.Length; i++)
            {
                for (var j = 0; j < this.rows[i].Length; j++)
                {
                    var isChecked = this.rows[i][j];
                    var character = this.group.Items[i].Items[j];
                    if (isChecked != null && character.IsChecked != isChecked.Value)
                    {
                        character.IsChecked = isChecked.Value;
                    }
                }
            }
        }
    }
}
