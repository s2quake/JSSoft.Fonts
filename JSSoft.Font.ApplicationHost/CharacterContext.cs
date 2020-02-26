using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace JSSoft.Font.ApplicationHost
{
    class CharacterContext
    {
        private readonly ObservableCollection<uint> characters;

        public CharacterContext(FontDescriptor fontDescriptor, ObservableCollection<uint> characters)
        {
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
