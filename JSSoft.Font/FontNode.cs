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

        private readonly List<Point> rightBottomList;

        private FontNode(FontNode parent, Rectangle rectangle, FontDataSettings settings)
        {
            this.Parent = parent;
            this.Rectangle = rectangle;
            this.pointList = parent == null ? new List<Point>(settings.Capacity * 2) { Point.Empty } : parent.pointList;
            this.rightBottomList = parent == null ? new List<Point>(settings.Capacity) : parent.rightBottomList;
            this.rectangleList = new List<Rectangle>();
            this.settings = settings;
            if (rectangle.Width >= minimumLength && rectangle.Height >= minimumLength)
            {
                this.Childs = Slice(rectangle).Select(item => new FontNode(this, item, settings)).ToArray();
            }
        }

        public static FontNode Create(FontDataSettings settings)
        {
            var rectangle = new Rectangle(0, 0, settings.Width, settings.Height);
            return new FontNode(null, rectangle, settings);
        }

        public override string ToString()
        {
            return $"{this.Rectangle}";
        }

        public Rectangle ReserveRegion(int width, int height)
        {
            foreach (var item in this.pointList)
            {
                var rectangle = new Rectangle(item.X, item.Y, width, height);
                if (this.ReserveRegion(rectangle) == true)
                {
                    var padding = this.settings.Padding;
                    return new Rectangle(item.X + padding.Left, item.Y + padding.Top, width, height);
                }
            }
            return Rectangle.Empty;
        }

        public Rectangle Rectangle { get; }

        public FontNode Parent { get; }

        public FontNode[] Childs { get; } = new FontNode[] { };

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
            region = Rectangle.Empty;
            for (var i = 0; i < nodes.Length; i++)
            {
                var node = this.HitTest(points[i]);
                if (node == null)
                    return false;
                nodes[i] = node;
            };
            if (r + spacing.Horizontal <= this.Rectangle.Width)
                r += spacing.Horizontal;
            if (b + spacing.Vertical <= this.Rectangle.Height)
                b += spacing.Vertical;
            region = Rectangle.FromLTRB(l, t, r, b);
            return true;
        }

        private bool TryAdd(Rectangle rectangle)
        {
            foreach (var item in this.rectangleList)
            {
                if (item.IntersectsWith(rectangle) == true)
                    return false;
            }
            this.rectangleList.Add(rectangle);
            return true;
        }

        private bool ReserveRegion(Rectangle rectangle)
        {
            if (rectangle.Right + this.settings.Padding.Horizontal > this.Rectangle.Width)
                return false;
            if (rectangle.Bottom + this.settings.Padding.Vertical > this.Rectangle.Height)
                return false;
            if (this.FindEmptyRegion(rectangle, out var nodes, out var region) == false)
                return false;

            foreach (var item in nodes.Distinct())
            {
                if (item.TryAdd(region) == false)
                    return false;
            }

            var lt = new Point(region.Left, region.Top);
            var rt = new Point(region.Right, region.Top);
            var lb = new Point(region.Left, region.Bottom);
            var rb = new Point(region.Right, region.Bottom);

            this.pointList.Remove(lt);
            this.pointList.Remove(lb);

            //if (this.rightBottomList.Contains(rt) == false)
            this.pointList.Insert(0, rt);
            if (this.rightBottomList.Contains(lb) == false)
                this.pointList.Add(lb);

            this.rectangleList.Add(rectangle);
            this.rightBottomList.Add(rb);

            return true;
        }

        private FontNode HitTest(Point location)
        {
            if (Intersect(this.Rectangle, location) == false)
                return null;

            foreach (var item in this.rectangleList)
            {
                if (Intersect(item, location) == true)
                    return null;
            }
            foreach (var item in this.Childs)
            {
                if (item.HitTest(location) is FontNode node)
                    return node;
            }
            return this;
        }
    }
}
