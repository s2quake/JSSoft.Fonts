using Ntreev.Library.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font.ConsoleHost
{
    class Settings
    {
        [CommandProperty(IsRequired = true)]
        public string FontPath { get; set; }

        [CommandProperty(IsRequired = true)]
        public string FileName { get; set; }

        [CommandProperty("dpi")]
        [DefaultValue(72)]
        public int DPI { get; set; }

        [CommandProperty]
        [DefaultValue(22)]
        public int Size { get; set; }

        [CommandProperty]
        [DefaultValue(128)]
        public int TextureWidth { get; set; }

        [CommandProperty]
        [DefaultValue(128)]
        public int TextureHeight { get; set; }

        [CommandProperty("characters", IsRequired = true)]
        public string CharactersText { get; set; }

        public uint[] GetCharacters()
        {
            return new CharacterCollection(this.CharactersText).ToArray();
        }
    }
}
