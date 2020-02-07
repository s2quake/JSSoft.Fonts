using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font.ApplicationHost.Serializations
{
    public struct ExportSettingsInfo
    {
        public string Font { get; set; }

        public int Size { get; set; }

        public int DPI { get; set; }

        public int TextureWidth { get; set; }

        public int TextureHeight { get; set; }

        public (int Left, int Top, int Right, int Bottom) Padding { get; set; }

        public CharacterGroupInfo[] Groups { get; set; }
    }
}
