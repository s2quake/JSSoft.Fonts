using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace JSSoft.Font
{
    class CharacterItem : PropertyChangedBase
    {
        private readonly IFontService fontService;
        private bool isEnabled;
        private bool isChecked;
        private ImageSource source;

        public CharacterItem(IFontService fontService, uint id)
        {
            this.fontService = fontService ?? throw new ArgumentNullException(nameof(fontService));
            this.ID = id;
            this.isEnabled = this.fontService.Contains(id);
            if  (this.isEnabled == true)
            {
                this.source = this.fontService.GetBitmap(id);
            }
        }

        public override string ToString()
        {
            return $"{(char)this.ID}";
        }

        public uint ID { get; }

        public char Text => (char)this.ID;

        public bool IsEnabled
        {
            get => this.isEnabled;
            set
            {
                this.isEnabled = value;
                this.NotifyOfPropertyChange(nameof(IsEnabled));
                if (value == true)
                {
                    int wer = 0;
                }
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

        public ImageSource Source
        {
            get => this.source;
            set
            {
                this.source = value;
                this.NotifyOfPropertyChange(nameof(Source));
            }
        }
    }
}
