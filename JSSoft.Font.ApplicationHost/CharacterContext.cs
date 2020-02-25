using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
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
        private readonly HashSet<char> characters = new HashSet<char>();
        public CharacterContext(FontDescriptor fontDescriptor)
        {
            this.FontDescriptor = fontDescriptor;
        }

        public void Register(Character character)
        {
            character.PropertyChanged += Character_PropertyChanged;
        }

        private void Character_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Character.IsChecked) && sender is Character character)
            {
                if (character.IsChecked == true)
                {
                    characters.Add((char)character.ID);
                }
                else
                {
                    characters.Remove((char)character.ID);
                }
            }
        }

        public FontDescriptor FontDescriptor { get; }

        public IReadOnlyDictionary<uint, FontGlyph> Glyphs => this.FontDescriptor.Glyphs;

        public int Height => this.FontDescriptor.ItemHeight;
    }
}
