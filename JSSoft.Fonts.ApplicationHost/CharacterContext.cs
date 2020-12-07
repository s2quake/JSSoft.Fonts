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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace JSSoft.Fonts.ApplicationHost
{
    class CharacterContext
    {
        private readonly List<uint> characterList = new List<uint>(100);
        private uint[] characterArray;

        public CharacterContext(IServiceProvider serviceProvider, FontDescriptor fontDescriptor)
        {
            this.ServiceProvider = serviceProvider;
            this.FontDescriptor = fontDescriptor;
        }

        public void Register(Character character)
        {
            character.PropertyChanged += Character_PropertyChanged;
        }

        public FontDescriptor FontDescriptor { get; }

        public IReadOnlyDictionary<uint, FontGlyph> Glyphs => this.FontDescriptor.Glyphs;

        public int Height => this.FontDescriptor.Height;

        public IServiceProvider ServiceProvider { get; }

        public uint[] Characters
        {
            get
            {
                if (this.characterArray == null)
                {
                    return this.characterArray = this.characterList.ToArray();
                }
                return this.characterArray;
            }
        }

        public event EventHandler Changed;

        protected virtual void OnChanged(EventArgs e)
        {
            this.Changed?.Invoke(this, e);
        }

        private void Character_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Character.IsChecked) && sender is Character character)
            {
                if (character.IsEnabled == true)
                {
                    if (character.IsChecked == true)
                    {
                        if (this.characterList.Count == this.characterList.Capacity)
                            this.characterList.Capacity += 100;
                        this.characterList.Add(character.ID);
                        this.characterArray = null;
                        this.OnChanged(EventArgs.Empty);
                    }
                    else
                    {
                        this.characterList.Remove(character.ID);
                        this.characterArray = null;
                        this.OnChanged(EventArgs.Empty);
                    }
                }
            }
        }
    }
}
