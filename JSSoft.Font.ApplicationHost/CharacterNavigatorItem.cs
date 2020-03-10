using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font.ApplicationHost
{
    class CharacterNavigatorItem : PropertyChangedBase, ICharacterNavigatorItem
    {
        private bool isCurrent;

        public CharacterNavigatorItem(ICharacter character)
        {
            this.Character = character;
        }

        public ICharacter Character { get; }

        public bool IsCurrent
        {
            get => this.isCurrent;
            set
            {
                this.isCurrent = value;
                this.NotifyOfPropertyChange(nameof(IsCurrent));
            }
        }
    }
}
