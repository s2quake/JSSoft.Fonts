using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace JSSoft.Font.Controls
{
    [TemplatePart(Name = "PART_Image", Type = typeof(Image))]
    public class CharacterControl : UserControl
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(CharacterControl));

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(nameof(Source), typeof(ImageSource), typeof(CharacterControl));

        public static readonly DependencyProperty GlyphMetricsProperty =
            DependencyProperty.Register(nameof(GlyphMetrics), typeof(GlyphMetrics), typeof(CharacterControl),
                new FrameworkPropertyMetadata(GlyphMetricsPropertyChangedCallback));

        private Image image;

        public CharacterControl()
        {

        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.image = this.Template.FindName("PART_Image", this) as Image;
            if  (this.image != null)
            {
                this.UpdateImageLayout();
            }
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            var property = e.Property;
            if (property == TextProperty || property == SourceProperty || property == GlyphMetricsProperty)
            {
                if (this.image != null)
                {
                    this.UpdateImageLayout();
                }
            }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            var length = Math.Min(constraint.Width, constraint.Height);
            var size = new Size(length, length);


            //var metrics = this.GlyphMetrics;
            //if (metrics.HorizontalAdvance)



            return base.MeasureOverride(size);
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

        private static void GlyphMetricsPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CharacterControl self && self.image != null)
            {
                self.UpdateImageLayout();
            }
        }

        private void UpdateImageLayout()
        {
            if (this.Text == "g")
            {
                int qwer = 0;
            }
            
            var metrics = this.GlyphMetrics;
            var left = metrics.HorizontalBearingX;
            var top = metrics.VerticalAdvance - metrics.HorizontalBearingY;
            var right = metrics.HorizontalAdvance - (metrics.Width + left);
            var bottom = 0;
            this.image.Margin = new Thickness(left, top, right, bottom);
            this.image.Width = metrics.HorizontalAdvance;
            this.image.Height = metrics.VerticalAdvance;
            if (this.GlyphMetrics.Height != 0 && this.Text == "\"")
            {
                int weqr = 0;
            }
        }
    }
}
