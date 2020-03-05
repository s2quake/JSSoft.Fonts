using Ntreev.Library;
using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JSSoft.Font.ApplicationHost.Dialogs.ViewModels
{
    public class FindCharacterViewModel : ModalDialogBase
    {
        private uint character;
        private string characterText;
        private bool byCharacter;
        private bool canFind;

        public FindCharacterViewModel()
        {
            this.DisplayName = "Find Character";
        }

        public void Find()
        {
            this.TryClose(true);
        }

        public uint Character
        {
            get => this.character;
            set
            {
                this.character = value;
                this.NotifyOfPropertyChange(nameof(Character));
            }
        }

        public string CharacterText
        {
            get => this.characterText ?? string.Empty;
            set
            {
                this.Update(value);
                //try
                //{
                //    this.character = char.Parse(value);
                //    this.CanFind = true;
                //    this.NotifyOfPropertyChange(nameof(Character));
                //    this.NotifyOfPropertyChange(nameof(CharacterText));
                //    this.NotifyOfPropertyChange(nameof(CanFind));
                //}
                //catch
                //{
                //    this.CanFind = false;
                //    this.NotifyOfPropertyChange(nameof(CanFind));
                //    throw;
                //}
            }
        }

        [ConfigurationProperty]
        public bool ByCharacter
        {
            get => this.byCharacter;
            set
            {
                this.byCharacter = value;
                this.NotifyOfPropertyChange(nameof(ByCharacter));
            }
        }

        public bool CanFind
        {
            get
            {
                if (this.CharacterText == string.Empty)
                    return false;
                return this.canFind;
            }
            private set
            {
                this.canFind = value;
                this.NotifyOfPropertyChange(nameof(CanFind));
            }
        }

        private void Update(string value)
        {
            try
            {
                if (this.ByCharacter == true)
                {
                    if (value.Length > 1)
                        throw new ArgumentException("length must be 1.");
                    if (value.Length == 1)
                    {
                        this.character = (uint)value.First();
                        this.characterText = value;
                    }
                    else
                    {
                        this.character = 0;
                        this.characterText = null;
                    }
                }
                else
                {
                    this.character = ParseText(value);
                    this.characterText = value;
                }
                this.CanFind = true;
                this.NotifyOfPropertyChange(nameof(Character));
                this.NotifyOfPropertyChange(nameof(CharacterText));
                this.NotifyOfPropertyChange(nameof(CanFind));
                return;
            }
            catch
            {
                this.CanFind = false;
                this.characterText = null;
                this.NotifyOfPropertyChange(nameof(CanFind));
                throw;
            }
        }

        private static uint ParseText(string text)
        {
            var match = Regex.Match(text, "^0x([0-9a-fA-F]+)");
            if (match.Success == true)
            {
                return uint.Parse(match.Groups[1].Value, NumberStyles.HexNumber);
            }
            return uint.Parse(text);
        }
    }
}
