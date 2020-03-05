using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font.ApplicationHost
{
    public struct FontInfo
    {
        public string FaceName { get; set; }

        public int DPI { get; set; }

        public int Size { get; set; }

        public int Height { get; set; }

        public int BaseLine { get; set; }

        public static readonly FontInfo Empty = new FontInfo() { FaceName = string.Empty };
    }
}
