using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font.ApplicationHost
{
    class CharacterRow : PropertyChangedBase, ICharacterRow
    {
        private readonly FontDescriptor fontDescriptor;
        private bool? isChecked { get; set; }

        internal CharacterRow(uint index)
        {
            this.Index = index;
            this.Item0 = new Character(0x0);
            this.Item1 = new Character(0x1);
            this.Item2 = new Character(0x2);
            this.Item3 = new Character(0x3);
            this.Item4 = new Character(0x4);
            this.Item5 = new Character(0x5);
            this.Item6 = new Character(0x6);
            this.Item7 = new Character(0x7);
            this.Item8 = new Character(0x8);
            this.Item9 = new Character(0x9);
            this.ItemA = new Character(0xA);
            this.ItemB = new Character(0xB);
            this.ItemC = new Character(0xC);
            this.ItemD = new Character(0xD);
            this.ItemE = new Character(0xE);
            this.ItemF = new Character(0xF);
            this.Items = new Character[]
            {
                this.Item0, this.Item1, this.Item2, this.Item3, this.Item4, this.Item5, this.Item6, this.Item7,
                this.Item8, this.Item9, this.ItemA, this.ItemB, this.ItemC, this.ItemD, this.ItemE, this.ItemF,
            };
        }

        public CharacterRow(CharacterGroup group, FontDescriptor fontDescriptor, uint min, uint max)
        {
            this.Group = group ?? throw new ArgumentNullException(nameof(group));
            this.fontDescriptor = fontDescriptor ?? throw new ArgumentNullException(nameof(fontDescriptor));
            if (min >= max)
                throw new ArgumentOutOfRangeException(nameof(min));

            this.Index = (min & 0xfffffff0) >> 4;
            this.Item0 = new Character(this, this.fontDescriptor, 0x0 + min);
            this.Item1 = new Character(this, this.fontDescriptor, 0x1 + min);
            this.Item2 = new Character(this, this.fontDescriptor, 0x2 + min);
            this.Item3 = new Character(this, this.fontDescriptor, 0x3 + min);
            this.Item4 = new Character(this, this.fontDescriptor, 0x4 + min);
            this.Item5 = new Character(this, this.fontDescriptor, 0x5 + min);
            this.Item6 = new Character(this, this.fontDescriptor, 0x6 + min);
            this.Item7 = new Character(this, this.fontDescriptor, 0x7 + min);
            this.Item8 = new Character(this, this.fontDescriptor, 0x8 + min);
            this.Item9 = new Character(this, this.fontDescriptor, 0x9 + min);
            this.ItemA = new Character(this, this.fontDescriptor, 0xA + min);
            this.ItemB = new Character(this, this.fontDescriptor, 0xB + min);
            this.ItemC = new Character(this, this.fontDescriptor, 0xC + min);
            this.ItemD = new Character(this, this.fontDescriptor, 0xD + min);
            this.ItemE = new Character(this, this.fontDescriptor, 0xE + min);
            this.ItemF = new Character(this, this.fontDescriptor, 0xF + min);
            this.Items = new Character[]
            {
                this.Item0, this.Item1, this.Item2, this.Item3, this.Item4, this.Item5, this.Item6, this.Item7,
                this.Item8, this.Item9, this.ItemA, this.ItemB, this.ItemC, this.ItemD, this.ItemE, this.ItemF,
            };
            this.Group.PropertyChanged += Group_PropertyChanged;
        }

        public bool TestVisible()
        {
            foreach (var item in this.Items)
            {
                if (item.IsEnabled == true)
                    return true;
            }
            return false;
        }

        public CharacterGroup Group { get; }

        public Character[] Items { get; }

        public uint Index { get; }

        public bool? IsChecked
        {
            get
            {
                if (this.Group != null && this.Group.IsChecked != null)
                    return this.Group.IsChecked.Value;
                return this.isChecked;
            }
            set
            {
                if (this.isChecked != value)
                {
                    this.isChecked = value;
                    this.NotifyOfPropertyChange(nameof(IsChecked));
                }
            }
        }

        public Character Item0 { get; }

        public Character Item1 { get; }

        public Character Item2 { get; }

        public Character Item3 { get; }

        public Character Item4 { get; }

        public Character Item5 { get; }

        public Character Item6 { get; }

        public Character Item7 { get; }

        public Character Item8 { get; }

        public Character Item9 { get; }

        public Character ItemA { get; }

        public Character ItemB { get; }

        public Character ItemC { get; }

        public Character ItemD { get; }

        public Character ItemE { get; }

        public Character ItemF { get; }

        private void Group_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CharacterGroup.IsChecked))
            {
                //if (object.Equals(this.Group.IsChecked, this.isChecked) == false)
                {
                    this.NotifyOfPropertyChange(nameof(IsChecked));
                }
            }
        }

        #region ICharacterRow

        ICharacter ICharacterRow.Item0 => this.Item0;

        ICharacter ICharacterRow.Item1 => this.Item1;

        ICharacter ICharacterRow.Item2 => this.Item2;

        ICharacter ICharacterRow.Item3 => this.Item3;

        ICharacter ICharacterRow.Item4 => this.Item4;

        ICharacter ICharacterRow.Item5 => this.Item5;

        ICharacter ICharacterRow.Item6 => this.Item6;

        ICharacter ICharacterRow.Item7 => this.Item7;

        ICharacter ICharacterRow.Item8 => this.Item8;

        ICharacter ICharacterRow.Item9 => this.Item9;

        ICharacter ICharacterRow.ItemA => this.ItemA;

        ICharacter ICharacterRow.ItemB => this.ItemB;

        ICharacter ICharacterRow.ItemC => this.ItemC;

        ICharacter ICharacterRow.ItemD => this.ItemD;

        ICharacter ICharacterRow.ItemE => this.ItemE;

        ICharacter ICharacterRow.ItemF => this.ItemF;

        ICharacter[] ICharacterRow.Items => this.Items;

        #endregion
    }
}
