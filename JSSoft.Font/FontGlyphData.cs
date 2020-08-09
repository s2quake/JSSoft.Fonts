// MIT License
// 
// Copyright (c) 2020 Jeesu Choi
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System.Drawing;

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
            this.PaddingRectangle = page.GeneratePaddingRectangle(rectangle);
            this.SpacingRectangle = page.GenerateSpacingRectangle(rectangle);
        }

        public bool IntersectsWith(Rectangle rectangle)
        {
            return this.SpacingRectangle.IntersectsWith(rectangle);
        }

        public uint ID => this.glyph.ID;

        public Bitmap Bitmap => this.glyph.Bitmap;

        public GlyphMetrics Metrics => this.glyph.Metrics;

        public Rectangle Rectangle { get; }

        public Rectangle PaddingRectangle { get; }

        public Rectangle SpacingRectangle { get; }

        public FontPage Page { get; }
    }
}
