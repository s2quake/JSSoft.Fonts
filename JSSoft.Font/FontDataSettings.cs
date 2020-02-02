using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font
{
    public class FontDataSettings
    {
        public int Width { get; set; } = 128;

        public int Height { get; set; } = 128;

        public (int Left, int Top, int Right, int Bottom) Padding { get; set; }

        public (int Horz, int Vert) Spacing { get; set; }
    }
}
