using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace JSSoft.Font.Controls
{
    class WrapGrid : Panel
    {
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(WrapGrid));

        public static readonly DependencyProperty ItemCountProperty =
            DependencyProperty.Register(nameof(ItemCount), typeof(int), typeof(WrapGrid));

        public Orientation Orientation
        {
            get => (Orientation)this.GetValue(OrientationProperty);
            set => this.SetValue(OrientationProperty, value);
        }

        public int ItemCount
        {
            get => (int)this.GetValue(ItemCountProperty);
            set => this.SetValue(ItemCountProperty, value);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            if (this.ItemCount != 0)
            {
                if (this.Orientation == Orientation.Horizontal)
                {
                    var itemWidth = (int)(constraint.Width / this.ItemCount);
                    var size = new Size(itemWidth, double.PositiveInfinity);
                    foreach (var item in this.InternalChildren)
                    {
                        if (item is FrameworkElement fe)
                        {
                            fe.Measure(size);
                        }
                    }
                }
            }

            return base.MeasureOverride(constraint);
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            var itemWidth = (int)(this.ActualWidth / this.ItemCount);
            var height = (double)0;
            var left = (double)0;
            var top = (double)0;
            for (var i = 0; i < this.InternalChildren.Count; i++)
            {
                var item = this.InternalChildren[i];

                if (i % this.ItemCount == 0)
                {
                    top += height;
                    left = 0;
                }

                if (item is FrameworkElement fe)
                {
                    var size = fe.DesiredSize;
                    var rect = new Rect(left, top, size.Width, size.Height);
                    fe.Arrange(rect);
                    height = Math.Max(height, size.Height);
                    left += fe.ActualWidth;
                }
            }

            return base.ArrangeOverride(arrangeBounds);
        }
    }
}
