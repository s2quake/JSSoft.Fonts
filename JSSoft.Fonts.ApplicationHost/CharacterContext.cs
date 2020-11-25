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

namespace JSSoft.Fonts.ApplicationHost
{
    class CharacterContext
    {
        private readonly ObservableCollection<uint> characters;

        public CharacterContext(IServiceProvider serviceProvider, FontDescriptor fontDescriptor, ObservableCollection<uint> characters)
        {
            this.ServiceProvider = serviceProvider;
            this.FontDescriptor = fontDescriptor;
            this.characters = characters;
        }

        public void Register(Character character)
        {
            character.PropertyChanged += Character_PropertyChanged;
        }

        public FontDescriptor FontDescriptor { get; }

        public IReadOnlyDictionary<uint, FontGlyph> Glyphs => this.FontDescriptor.Glyphs;

        public int Height => this.FontDescriptor.Height;

        public IServiceProvider ServiceProvider { get; }

        private void Character_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Character.IsChecked) && sender is Character character)
            {
                if (character.IsEnabled == true)
                {
                    if (character.IsChecked == true)
                    {
                        characters.Add(character.ID);
                    }
                    else
                    {
                        characters.Remove(character.ID);
                    }
                }
            }
        }
    }
}
