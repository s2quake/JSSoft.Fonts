using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font
{
    public sealed class FontGlyph
    {
        public uint ID { get; internal set; }

        public FontPage Page { get; internal set; }

        public Rectangle Rectangle { get; internal set; }

        public Bitmap Bitmap { get; internal set; }

        public GlyphMetrics Metrics { get; internal set; }
    }
}
