using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font
{
    class CharacterItem : PropertyChangedBase
    {
        private bool isEnabled;
        private bool isChecked;

        public CharacterItem(uint id)
        {
            this.ID = id;
        }

        public uint ID { get; }

        public bool IsEnabled
        {
            get => this.isEnabled;
            set
            {
                this.isEnabled = value;
                this.NotifyOfPropertyChange(nameof(IsEnabled));
            }
        }

        public bool IsChecked
        {
            get => this.isChecked;
            set
            {
                this.isChecked = value;
                this.NotifyOfPropertyChange(nameof(IsChecked));
            }
        }
    }
}
