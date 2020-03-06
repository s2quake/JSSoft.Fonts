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

        //private Image image;
        private Viewbox viewbox;

        public CharacterControl()
        {

        }

        //public void Save()
        //{
        //    if (image.Source is System.Windows.Media.Imaging.BitmapImage bitmap)
        //    {
        //        var encoder = new System.Windows.Media.Imaging.PngBitmapEncoder();
        //        encoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(bitmap));
        //        using (var fileStream = new System.IO.FileStream(@"C:\Users\s2quake\Desktop\character.png", System.IO.FileMode.Create))
        //        {
        //            encoder.Save(fileStream);
        //        }
        //    }
        //}

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.viewbox = this.Template.FindName(PART_Viewbox, this) as Viewbox;
            if (this.viewbox != null)
            {
                //this.image = new Image();
                //RenderOptions.SetBitmapScalingMode(this.image, BitmapScalingMode.NearestNeighbor);
                //this.viewbox.Child = this.image;
                ////this.viewbox.HorizontalAlignment = HorizontalAlignment.Center;
                ////this.viewbox.VerticalAlignment = VerticalAlignment.Center;
                //this.viewbox.Stretch = Stretch.None;
                //BindingOperations.SetBinding(this.image, Image.SourceProperty, new Binding(nameof(Source)) { Source = this });
                //BindingOperations.SetBinding(this.image, Image.WidthProperty, new Binding($"{nameof(Source)}.{nameof(BitmapSource.PixelWidth)}") 
                //{
                //    RelativeSource = new RelativeSource(RelativeSourceMode.Self) 
                //});
                //BindingOperations.SetBinding(this.image, Image.HeightProperty, new Binding($"{nameof(Source)}.{nameof(BitmapSource.PixelHeight)}")
                //{
                //    RelativeSource = new RelativeSource(RelativeSourceMode.Self)
                //});
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

        //protected override Size ArrangeOverride(Size arrangeBounds)
        //{
        //    var metrics = this.GlyphMetrics;
        //    var left = Math.Floor((arrangeBounds.Width - metrics.HorizontalAdvance * this.ZoomLevel) * 0.5);
        //    var top = Math.Floor((arrangeBounds.Height - metrics.VerticalAdvance * this.ZoomLevel) * 0.5);
        //    if (this.viewbox != null)
        //    {
        //        this.viewbox.Margin = new Thickness(left, top, 0, 0);
        //    }
        //    return base.ArrangeOverride(arrangeBounds);
        //}

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
                //if (this.image != null)
                //{
                //    this.image.Margin = new Thickness(0);
                //    this.image.Width = double.NaN;
                //    this.image.Height = double.NaN;
                //}
                if (this.viewbox != null)
                {
                    this.viewbox.Width = double.NaN;
                    this.viewbox.Height = double.NaN;
                }
            }
            else
            {
                //if (metrics.ID == (uint)'b' && this.image != null)
                //{
                //    int qwqwer = 0;
                //}
                var left = metrics.HorizontalBearingX;
                var top = metrics.BaseLine - metrics.HorizontalBearingY;
                var right = metrics.HorizontalAdvance - (left + metrics.Width);
                var bottom = metrics.VerticalAdvance - (top + metrics.Height);
                this.ImageMargin = new Thickness(left, top, right, bottom);
                //if (this.image != null)
                {
                    //this.image.Margin = new Thickness(left, top, right, bottom);
                    //if (this.image.Source is BitmapSource bitmapSource)
                    //{
                    //    this.image.Width = bitmapSource.PixelWidth;
                    //    this.image.Height = bitmapSource.PixelHeight;
                    //}
                }
                if (this.viewbox != null)
                {
                    this.viewbox.Width = (metrics.HorizontalAdvance) * this.ZoomLevel;
                    this.viewbox.Height = (metrics.VerticalAdvance) * this.ZoomLevel;
                }
            }
        }
    }
}
