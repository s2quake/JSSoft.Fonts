using Ntreev.ModernUI.Framework;
using Ntreev.ModernUI.Framework.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font.ApplicationHost
{
    class CharacterRowView : PropertyChangedBase
    {
        private ICharacterRow row;

        public CharacterRowView()
        {

        }

        public ICharacterRow Row
        {
            get => this.row;
            set
            {
                this.row = value;
                this.Refresh();
            }
        }

        public uint Index => this.row.Index;

        public bool? IsChecked
        {
            get => this.row.IsChecked;
            set => this.row.IsChecked = value;
        }

        public object this[int index]
        {
            get => this.row.Items[index];
        }
    }
}
