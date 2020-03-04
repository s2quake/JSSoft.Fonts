using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font
{
    class FontImage
    {
        public FontImage(FontGlyph glyph, Rectangle rectangle)
        {
            this.Glyph = glyph;
            this.Rectangle = rectangle;
        }

        public bool IntersectsWith(Rectangle rectangle)
        {
            return this.Rectangle.IntersectsWith(rectangle);
        }

        public FontGlyph Glyph { get; }

        public Rectangle Rectangle { get; }
    }
}
