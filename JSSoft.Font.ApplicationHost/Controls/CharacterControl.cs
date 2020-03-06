using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace JSSoft.Font.ApplicationHost.Controls
{
    [TemplatePart(Name = PART_Viewbox, Type = typeof(Viewbox))]
    public class CharacterControl : Control
    {
        public const string PART_Viewbox = nameof(PART_Viewbox);

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(CharacterControl));

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(nameof(Source), typeof(ImageSource), typeof(CharacterControl));

        public static readonly DependencyProperty ImageMarginProperty =
            DependencyProperty.Register(nameof(ImageMargin), typeof(Thickness), typeof(CharacterControl));

        public static readonly DependencyProperty GlyphMetricsProperty =
            DependencyProperty.Register(nameof(GlyphMetrics), typeof(GlyphMetrics), typeof(CharacterControl),
                new FrameworkPropertyMetadata(GlyphMetricsPropertyChangedCallback));

        public static readonly DependencyProperty ZoomLevelProperty =
           DependencyProperty.Register(nameof(ZoomLevel), typeof(double), typeof(CharacterControl),
               new FrameworkPropertyMetadata(1.0, ZoomLevelPropertyChangedCallback));

        private Viewbox viewbox;

        public CharacterControl()
        {

        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.viewbox = this.Template.FindName(PART_Viewbox, this) as Viewbox;
            if (this.viewbox != null)
            {
                this.viewbox.Stretch = Stretch.None;
            }
            this.UpdateImageLayout();
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            var property = e.Property;
            if (property == TextProperty || property == SourceProperty || property == GlyphMetricsProperty)
            {
                this.UpdateImageLayout();
            }
        }

        public string Text
        {
            get => (string)this.GetValue(TextProperty);
            set => this.SetValue(TextProperty, value);
        }

        public ImageSource Source
        {
            get => (ImageSource)this.GetValue(SourceProperty);
            set => this.SetValue(SourceProperty, value);
        }

        public GlyphMetrics GlyphMetrics
        {
            get => (GlyphMetrics)this.GetValue(GlyphMetricsProperty);
            set => this.SetValue(GlyphMetricsProperty, value);
        }

        public double ZoomLevel
        {
            get => (double)this.GetValue(ZoomLevelProperty);
            set => this.SetValue(ZoomLevelProperty, value);
        }

        public Thickness ImageMargin
        {
            get => (Thickness)this.GetValue(ImageMarginProperty);
            set => this.SetValue(ImageMarginProperty, value);
        }

        private static void GlyphMetricsPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CharacterControl self)
            {
                self.UpdateImageLayout();
            }
        }

        private static void ZoomLevelPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CharacterControl self)
            {
                self.UpdateImageLayout();
            }
        }

        private void UpdateImageLayout()
        {
            var metrics = this.GlyphMetrics;
            if (metrics.Width == 0 || metrics.Height == 0 || this.IsEnabled == false)
            {
                this.ImageMargin = new Thickness(0);
                if (this.viewbox != null)
                {
                    this.viewbox.Width = double.NaN;
                    this.viewbox.Height = double.NaN;
                }
            }
            else
            {
                var left = metrics.HorizontalBearingX;
                var top = metrics.BaseLine - metrics.HorizontalBearingY;
                var right = metrics.HorizontalAdvance - (left + metrics.Width);
                var bottom = metrics.VerticalAdvance - (top + metrics.Height);
                this.ImageMargin = new Thickness(left, top, right, bottom);
                if (this.viewbox != null)
                {
                    this.viewbox.Width = metrics.HorizontalAdvance * this.ZoomLevel;
                    this.viewbox.Height = metrics.VerticalAdvance * this.ZoomLevel;
                }
            }
        }
    }
}
