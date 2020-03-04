using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font
{
    public class FontDataSettings
    {
        public string Name { get; set; } = string.Empty;

        public int Width { get; set; } = 512;

        public int Height { get; set; } = 512;

        public FontPadding Padding { get; set; }

        public FontSpacing Spacing { get; set; }

        public uint[] Characters { get; set; }

        internal int Capacity => this.Characters.Length;
    }
}
