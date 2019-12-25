using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font
{
    class CharacterRowItem : PropertyChangedBase
    {
        private readonly IFontService fontService;

        public CharacterRowItem(IFontService fontService, uint min, uint max)
        {
            this.fontService = fontService ?? throw new ArgumentNullException(nameof(fontService));
            if (min >= max)
                throw new ArgumentOutOfRangeException(nameof(min));

            this.Index = (min & 0xfffffff0) >> 4;
            this.Item0 = new CharacterItem(this.fontService, 0x0 + min);
            this.Item1 = new CharacterItem(this.fontService, 0x1 + min);
            this.Item2 = new CharacterItem(this.fontService, 0x2 + min);
            this.Item3 = new CharacterItem(this.fontService, 0x3 + min);
            this.Item4 = new CharacterItem(this.fontService, 0x4 + min);
            this.Item5 = new CharacterItem(this.fontService, 0x5 + min);
            this.Item6 = new CharacterItem(this.fontService, 0x6 + min);
            this.Item7 = new CharacterItem(this.fontService, 0x7 + min);
            this.Item8 = new CharacterItem(this.fontService, 0x8 + min);
            this.Item9 = new CharacterItem(this.fontService, 0x9 + min);
            this.ItemA = new CharacterItem(this.fontService, 0xA + min);
            this.ItemB = new CharacterItem(this.fontService, 0xB + min);
            this.ItemC = new CharacterItem(this.fontService, 0xC + min);
            this.ItemD = new CharacterItem(this.fontService, 0xD + min);
            this.ItemE = new CharacterItem(this.fontService, 0xE + min);
            this.ItemF = new CharacterItem(this.fontService, 0xF + min);
            this.Items = new CharacterItem[]
            {
                this.Item0, this.Item1, this.Item2, this.Item3, this.Item4, this.Item5, this.Item6, this.Item7,
                this.Item8, this.Item9, this.ItemA, this.ItemB, this.ItemC, this.ItemD, this.ItemE, this.ItemF,
            };
        }

        public bool TestVisible()
        {
            foreach(var item in this.Items)
            {
                if (item.IsEnabled == true)
                    return true;
            }
            return false;
        }

        public CharacterItem[] Items { get; }

        public uint Index { get; }

        public CharacterItem Item0 { get; }

        public CharacterItem Item1 { get; }

        public CharacterItem Item2 { get; }

        public CharacterItem Item3 { get; }

        public CharacterItem Item4 { get; }

        public CharacterItem Item5 { get; }

        public CharacterItem Item6 { get; }

        public CharacterItem Item7 { get; }

        public CharacterItem Item8 { get; }

        public CharacterItem Item9 { get; }

        public CharacterItem ItemA { get; }

        public CharacterItem ItemB { get; }

        public CharacterItem ItemC { get; }

        public CharacterItem ItemD { get; }

        public CharacterItem ItemE { get; }

        public CharacterItem ItemF { get; }
    }
}
