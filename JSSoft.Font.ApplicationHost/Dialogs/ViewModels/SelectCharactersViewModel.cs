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

using Ntreev.ModernUI.Framework;
using System;
using System.Linq;

namespace JSSoft.Font.ApplicationHost.Dialogs.ViewModels
{
    public class SelectCharactersViewModel : ModalDialogBase
    {
        private uint[] characters;
        private string charactersText;

        public SelectCharactersViewModel(params uint[] characters)
        {
            this.characters = characters ?? throw new ArgumentNullException(nameof(characters));
            this.charactersText = CharacterCollection.ToString(characters);
            this.DisplayName = "Select Characters";
        }

        public void Select()
        {
            this.TryClose(true);
        }

        public uint[] Characters
        {
            get => this.characters;
            set
            {
                this.characters = value ?? throw new ArgumentNullException(nameof(value));
                this.charactersText = CharacterCollection.ToString(value);
                this.NotifyOfPropertyChange(nameof(Characters));
            }
        }

        public string CharactersText
        {
            get => this.charactersText;
            set
            {
                try
                {
                    CharacterCollection.Validate(value);
                    this.charactersText = value;
                    this.characters = CharacterCollection.Parse(value).ToArray();
                    this.CanSelect = true;
                    this.NotifyOfPropertyChange(nameof(Characters));
                    this.NotifyOfPropertyChange(nameof(CharactersText));
                    this.NotifyOfPropertyChange(nameof(CanSelect));
                }
                catch
                {
                    this.CanSelect = false;
                    this.NotifyOfPropertyChange(nameof(CanSelect));
                    throw;
                }
            }
        }

        public bool CanSelect { get; private set; }
    }
}
