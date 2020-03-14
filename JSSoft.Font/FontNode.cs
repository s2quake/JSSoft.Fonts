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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace JSSoft.Font
{
    class FontNode
    {
        private const int minimumLength = 128;

        private readonly List<Point> pointList;
        private readonly FontDataSettings settings;
        private readonly List<FontGlyphData> glyphList = new List<FontGlyphData>(10);
        private readonly List<Point> rightBottomList;
        private int count;

        private FontNode(FontPage page, FontNode parent, Rectangle rectangle, FontDataSettings settings)
        {
            this.Page = page;
            this.Parent = parent;
            this.Rectangle = rectangle;
            this.pointList = parent == null ? new List<Point>(settings.Capacity * 2) { Point.Empty } : parent.pointList;
            this.rightBottomList = parent == null ? new List<Point>(settings.Capacity) : parent.rightBottomList;
            this.settings = settings;
            this.Childs = Slice(rectangle).Select(item => new FontNode(page, this, item, settings)).ToArray();
            while (parent != null)
            {
                parent.count++;
                parent = parent.Parent;
            }
        }

        public FontGlyphData HitTest(Point point)
        {
            if (Intersect(this.Rectangle, point) == false)
                return null;
            if (this.Childs.Any() == false)
            {
                foreach (var item in this.glyphList)
                {
                    if (Intersect(item.Rectangle, point) == true)
                        return item;
                }
            }
            else
            {
                foreach (var item in this.Childs)
                {
                    if (item.HitTest(point) is FontGlyphData glyphData)
                        return glyphData;
                }
            }
            return null;
        }

        public static FontNode Create(FontPage page, FontDataSettings settings)
        {
            var rectangle = new Rectangle(0, 0, settings.Width, settings.Height);
            return new FontNode(page, null, rectangle, settings);
        }

        public override string ToString()
        {
            return $"{this.Rectangle}";
        }

        public IReservator ReserveRegion(FontGlyph glyph)
        {
            if (glyph == null)
                throw new ArgumentNullException(nameof(glyph));
            if (glyph.Bitmap == null)
                return null;
            if (this.Parent != null)
                throw new InvalidOperationException("This method is only available on the root node.");

            foreach (var item in this.pointList)
            {
                if (this.ReserveRegion(glyph, item) is IReservator reservator)
                    return reservator;
            }
            return null;
        }

        public Rectangle Rectangle { get; }

        public FontPage Page { get; }

        public FontNode Parent { get; }

        public FontNode[] Childs { get; } = new FontNode[] { };

        public IReadOnlyList<FontGlyphData> GlyphList => this.glyphList;

        private static Rectangle[] Slice(Rectangle rectangle)
        {
            if (rectangle.Width < minimumLength || rectangle.Height < minimumLength)
            {
                return new Rectangle[] { };
            }
            var centerX = rectangle.Left + rectangle.Width / 2;
            var centerY = rectangle.Top + rectangle.Height / 2;
            return new Rectangle[]
            {
                Rectangle.FromLTRB(rectangle.Left, rectangle.Top, centerX, centerY),
                Rectangle.FromLTRB(centerX, rectangle.Top, rectangle.Right, centerY),
                Rectangle.FromLTRB(rectangle.Left, centerY, centerX, rectangle.Bottom),
                Rectangle.FromLTRB(centerX, centerY, rectangle.Right, rectangle.Bottom),
            };
        }

        private static bool Intersect(Rectangle rectangle, Point location)
        {
            return location.X >= rectangle.Left && location.X < rectangle.Right && location.Y >= rectangle.Top && location.Y < rectangle.Bottom;
        }

        private void CollectNode(Rectangle rectangle, List<FontNode> nodeList)
        {
            if (this.Rectangle.IntersectsWith(rectangle) == false)
                return;

            foreach (var item in this.glyphList)
            {
                if (item.IntersectsWith(rectangle) == true)
                    return;
            }

            nodeList.Add(this);
            foreach (var item in this.Childs)
            {
                item.CollectNode(rectangle, nodeList);
            }
        }

        private void AddGlyphData(FontGlyphData glyphData)
        {
            if (this.glyphList.Capacity >= this.glyphList.Count + 1)
            {
                this.glyphList.Capacity += 10;
            }
            this.glyphList.Add(glyphData);
        }

        private IReservator ReserveRegion(FontGlyph glyph, Point point)
        {
            var padding = this.settings.Padding;
            var metrics = glyph.Metrics;
            var width = metrics.Width;
            var height = metrics.Height;
            var rect = new Rectangle(point.X + padding.Left, point.Y + padding.Top, width, height);
            var paddingRect = this.Page.GeneratePaddingRectangle(rect);
            if (paddingRect.Right > this.Rectangle.Right || paddingRect.Bottom > this.Rectangle.Bottom)
                return null;

            var spacingRect = this.Page.GenerateSpacingRectangle(rect);
            var nodeList = new List<FontNode>(this.count);
            this.CollectNode(spacingRect, nodeList);
            if (nodeList.Any() == false)
                return null;
            return new Reservator(() => this.Reserve(glyph, nodeList.ToArray(), rect));
        }

        private void Reserve(FontGlyph glyph, FontNode[] nodes, Rectangle region)
        {
            var glyphData = new FontGlyphData(this.Page, glyph, region);
            foreach (var item in nodes)
            {
                item.AddGlyphData(glyphData);
            }
            var spacingRect = glyphData.SpacingRectangle;
            var lt = new Point(spacingRect.Left, spacingRect.Top);
            var rt = new Point(spacingRect.Right, spacingRect.Top);
            var lb = new Point(spacingRect.Left, spacingRect.Bottom);
            var rb = new Point(spacingRect.Right, spacingRect.Bottom);

            this.pointList.Remove(lt);
            this.pointList.Remove(lb);
            this.pointList.Insert(0, rt);
            if (this.rightBottomList.Contains(lb) == false)
                this.pointList.Add(lb);

            this.rightBottomList.Add(rb);
        }

        private FontNode HitTest(Rectangle rectangle)
        {
            if (this.Rectangle.IntersectsWith(rectangle) == false)
                return null;

            foreach (var item in this.glyphList)
            {
                if (item.IntersectsWith(rectangle) == true)
                    return null;
            }
            foreach (var item in this.Childs)
            {
                if (item.HitTest(rectangle) is FontNode node)
                    return node;
            }
            return this;
        }
    }
}
