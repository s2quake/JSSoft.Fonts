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

namespace JSSoft.Font.ApplicationHost.Controls
{
    [TemplatePart(Name = nameof(PART_Grid), Type = typeof(Grid))]
    [TemplatePart(Name = nameof(PART_Image), Type = typeof(Image))]
    public class CharacterControl : Control
    {
        public const string PART_Grid = nameof(PART_Grid);
        public const string PART_Image = nameof(PART_Image);

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(CharacterControl));

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(nameof(Source), typeof(ImageSource), typeof(CharacterControl));

        public static readonly DependencyProperty GlyphMetricsProperty =
            DependencyProperty.Register(nameof(GlyphMetrics), typeof(GlyphMetrics), typeof(CharacterControl),
                new FrameworkPropertyMetadata(GlyphMetricsPropertyChangedCallback));

        public static readonly DependencyProperty ZoomLevelProperty =
           DependencyProperty.Register(nameof(ZoomLevel), typeof(double), typeof(CharacterControl),
               new FrameworkPropertyMetadata(1.0, ZoomLevelPropertyChangedCallback));

        private Image image;
        private Grid grid;

        public CharacterControl()
        {

        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.grid = this.Template.FindName(PART_Grid, this) as Grid;
            this.image = this.Template.FindName(PART_Image, this) as Image;
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
                if (this.image != null)
                {
                    this.image.Margin = new Thickness(0);
                    this.image.Width = double.NaN;
                    this.image.Height = double.NaN;
                }
                if (this.grid != null)
                {
                    this.grid.Width = double.NaN;
                    this.grid.Height = double.NaN;
                }
            }
            else
            {
                var left = metrics.HorizontalBearingX;
                var top = metrics.BaseLine - metrics.HorizontalBearingY;
                var right = metrics.HorizontalAdvance - (left + metrics.Width);
                var bottom = metrics.VerticalAdvance - (top + metrics.Height);
                if (this.image != null)
                {
                    this.image.Margin = new Thickness(left, top, right, bottom);
                }
                if (this.grid != null)
                {
                    this.grid.Width = metrics.VerticalAdvance * this.ZoomLevel;
                    this.grid.Height = metrics.VerticalAdvance * this.ZoomLevel;
                }
            }
        }
    }
}
