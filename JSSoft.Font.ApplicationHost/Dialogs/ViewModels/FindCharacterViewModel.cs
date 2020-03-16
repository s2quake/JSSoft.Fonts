// MIT License
// 
// Copyright (c) 2020 Jeesu Choi
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using JSSoft.Font.ApplicationHost.Properties;
using Ntreev.Library;
using Ntreev.ModernUI.Framework;
using System;
using System.Globalization;
using System.Linq;
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
            this.DisplayName = Resources.Title_FindCharacter;
        }

        public async Task FindAsync()
        {
            await this.TryCloseAsync(true);
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
            set => this.Update(value);
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
                        throw new ArgumentException(Resources.Exception_LengthMustBeOne);
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
