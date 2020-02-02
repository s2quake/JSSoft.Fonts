using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font
{
    public sealed class FontGlyphData
    {
        private readonly FontGlyph glyph;

        public FontGlyphData(FontPage page, FontGlyph glyph, Rectangle rectangle)
        {
            this.Page = page;
            this.glyph = glyph;
            this.Rectangle = rectangle;
        }

        public uint ID => this.glyph.ID;

        public Bitmap Bitmap => this.glyph.Bitmap;

        public GlyphMetrics Metrics => this.glyph.Metrics;

        public Rectangle Rectangle { get; }

        public FontPage Page { get; }
    }
}
