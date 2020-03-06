using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font
{
    class FontNode
    {
        private const int minimumLength = 128;

        private readonly List<Point> pointList;
        private readonly List<Rectangle> rectangleList;
        private readonly FontDataSettings settings;
        private readonly List<FontGlyphData> glyphList;

        private readonly List<Point> rightBottomList;

        private FontNode(FontPage page, FontNode parent, Rectangle rectangle, FontDataSettings settings)
        {
            this.Page = page;
            this.Parent = parent;
            this.Rectangle = rectangle;
            this.pointList = parent == null ? new List<Point>(settings.Capacity * 2) { Point.Empty } : parent.pointList;
            this.rightBottomList = parent == null ? new List<Point>(settings.Capacity) : parent.rightBottomList;
            this.glyphList = parent == null ? new List<FontGlyphData>(settings.Capacity) : parent.glyphList;
            this.rectangleList = new List<Rectangle>();
            this.settings = settings;
            if (rectangle.Width >= minimumLength && rectangle.Height >= minimumLength)
            {
                var childRectangles = Slice(rectangle);
                this.Childs = new FontNode[childRectangles.Length];
                for (var i = 0; i < childRectangles.Length; i++)
                {
                    this.Childs[i] = new FontNode(page, this, childRectangles[i], settings);
                }
            }
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
            if (glyph.Bitmap == null)
                return null;

            var metrics = glyph.Metrics;
            var width = metrics.Width;
            var height = metrics.Height;

            foreach (var item in this.pointList)
            {
                var rectangle = new Rectangle(item.X, item.Y, width, height);
                if (this.ReserveRegion(glyph, rectangle) is IReservator reservator)
                {
                    return reservator;
                }
            }
            return null;
        }

        public Rectangle Rectangle { get; }

        public FontPage Page { get; }

        public FontNode Parent { get; }

        public FontNode[] Childs { get; } = new FontNode[] { };

        public IReadOnlyList<FontGlyphData> GlyphList => this.glyphList;

        private static bool Intersect(Rectangle rectangle, Point location)
        {
            return location.X >= rectangle.Left && location.X < rectangle.Right && location.Y >= rectangle.Top && location.Y < rectangle.Bottom;
        }

        private static Rectangle[] Slice(Rectangle rectangle)
        {
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

        private bool FindEmptyRegion(Rectangle rectangle, out FontNode[] nodes, out Rectangle region)
        {
            var spacing = this.settings.Spacing;
            var paddingH = this.settings.Padding.Horizontal;
            var paddingV = this.settings.Padding.Vertical;
            var l = rectangle.Left;
            var t = rectangle.Top;
            var r = rectangle.Right + paddingH;
            var b = rectangle.Bottom + paddingV;
            var points = new Point[]
            {
                new Point(l, t),
                new Point(r, t),
                new Point(l, b),
                new Point(r, b)
            };
            nodes = new FontNode[points.Length];
            region = Rectangle.FromLTRB(l, t, r, b);
            for (var i = 0; i < nodes.Length; i++)
            {
                nodes[i] = this.HitTest(region);
                if (nodes[i] == null || nodes[i].Childs.Any() == true)
                {
                    return false;
                }
                //if  (nodes[i] != null && nodes[i].Childs.Any() == true)
                //{
                //    int qwer = 0;
                //}
            };
            if (r + spacing.Horizontal <= this.Rectangle.Width)
                r += spacing.Horizontal;
            if (b + spacing.Vertical <= this.Rectangle.Height)
                b += spacing.Vertical;
            region = Rectangle.FromLTRB(l, t, r, b);
            return true;
        }

        private bool VerifyAddRegion(Rectangle rectangle)
        {
            foreach (var item in this.rectangleList)
            {
                if (item.IntersectsWith(rectangle) == true)
                    return false;
            }
            return true;
        }

        private void AddRegion(Rectangle rectangle)
        {
            foreach (var item in this.rectangleList)
            {
                if (item.IntersectsWith(rectangle) == true)
                    throw new ArgumentException("qwerqwrqwerwqe");
            }
            this.rectangleList.Add(rectangle);
        }

        private IReservator ReserveRegion(FontGlyph glyph, Rectangle rectangle)
        {
            if (rectangle.Right + this.settings.Padding.Horizontal > this.Rectangle.Width)
                return null;
            if (rectangle.Bottom + this.settings.Padding.Vertical > this.Rectangle.Height)
                return null;

            if (this.FindEmptyRegion(rectangle, out var nodes, out var region) == false)
                return null;

            //foreach (var item in nodes)
            //{
            //    if (item == null)
            //        return null;
            //}

            var nodeList = new List<FontNode>(nodes.Length);
            foreach (var item in nodes.Distinct())
            {
                if (item.VerifyAddRegion(region) == false)
                    return null;
                nodeList.Add(item);
            }

            return new Reservator(() => this.Reserve(glyph, nodeList.ToArray(), region));
        }

        private void Reserve(FontGlyph glyph, FontNode[] nodes, Rectangle region)
        {
            foreach (var item in nodes)
            {
                item.AddRegion(region);
            }
            var lt = new Point(region.Left, region.Top);
            var rt = new Point(region.Right, region.Top);
            var lb = new Point(region.Left, region.Bottom);
            var rb = new Point(region.Right, region.Bottom);
            var padding = this.settings.Padding;
            var glyphWidth = glyph.Metrics.Width;
            var glyphHeight = glyph.Metrics.Height;
            var glyphRegion = new Rectangle(region.Left + padding.Left, region.Top + padding.Top, glyphWidth, glyphHeight);

            this.pointList.Remove(lt);
            this.pointList.Remove(lb);

            //if (this.rightBottomList.Contains(rt) == false)
            this.pointList.Insert(0, rt);
            if (this.rightBottomList.Contains(lb) == false)
                this.pointList.Add(lb);

            this.rightBottomList.Add(rb);
            this.glyphList.Add(new FontGlyphData(this.Page, glyph, glyphRegion));
        }

        private FontNode HitTest(Rectangle rectangle)
        {
            if (this.Rectangle.IntersectsWith(rectangle) == false)
                return null;

            foreach (var item in this.rectangleList)
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
