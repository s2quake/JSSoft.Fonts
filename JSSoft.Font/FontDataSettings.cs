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

        public FontPadding Padding { get; set; }

        public FontSpacing Spacing { get; set; }
    }
}
