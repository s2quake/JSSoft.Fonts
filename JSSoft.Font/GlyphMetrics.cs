using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font
{
    public struct GlyphMetrics
    {
        public int Width { get; set; }

        public int Height { get; set; }

        public int HorizontalBearingX { get; set; }

        public int HorizontalBearingY { get; set; }

        public int HorizontalAdvance { get; set; }

        public int VerticalBearingX { get; set; }

        public int VerticalBearingY { get; set; }

        public int VerticalAdvance { get; set; }

        public int BaseLine { get; set; }
    }
}