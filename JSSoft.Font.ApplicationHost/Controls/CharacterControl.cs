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

        public static readonly DependencyProperty CharacterProperty =
            DependencyProperty.Register(nameof(Character), typeof(ICharacter), typeof(CharacterControl),
                new FrameworkPropertyMetadata(CharacterPropertyChangedCallback));

        private static void CharacterPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.CoerceValue(ImageWidthProperty);
            d.CoerceValue(ImageHeightProperty);
            d.CoerceValue(ImageMarginProperty);
        }

        private static readonly DependencyPropertyKey ImageMarginPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(ImageMargin), typeof(Thickness), typeof(CharacterControl),
                new FrameworkPropertyMetadata(ImageMarginPropertyChangedCallback, ImageMarginPropertyCoerceValueCallback));
        public static readonly DependencyProperty ImageMarginProperty = ImageMarginPropertyKey.DependencyProperty;

        private static void ImageMarginPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
        }

        private static object ImageMarginPropertyCoerceValueCallback(DependencyObject d, object baseValue)
        {
            if (d.GetValue(CharacterProperty) is ICharacter character)
            {
                var metrics = character.GlyphMetrics;
                var left = metrics.HorizontalBearingX;
                var top = metrics.BaseLine - metrics.HorizontalBearingY;
                var right = metrics.HorizontalAdvance - (left + metrics.Width);
                var bottom = metrics.VerticalAdvance - (top + metrics.Height);
                return new Thickness(left, top, right, bottom);
            }

            return baseValue;
        }

        private static readonly DependencyPropertyKey ImageWidthPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(ImageWidth), typeof(double), typeof(CharacterControl),
                new FrameworkPropertyMetadata(ImageWidthPropertyChangedCallback, ImageWidthPropertyCoerceValueCallback));
        public static readonly DependencyProperty ImageWidthProperty = ImageWidthPropertyKey.DependencyProperty;

        private static void ImageWidthPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static object ImageWidthPropertyCoerceValueCallback(DependencyObject d, object baseValue)
        {
            if (d.GetValue(CharacterProperty) is ICharacter character)
            {
                var zoomLevel = (double)d.GetValue(ZoomLevelProperty);
                var metrics = character.GlyphMetrics;
                return metrics.HorizontalAdvance * zoomLevel; 
            }
            return baseValue;
        }

        private static readonly DependencyPropertyKey ImageHeightPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(ImageHeight), typeof(double), typeof(CharacterControl),
                new FrameworkPropertyMetadata(ImageHeightPropertyChangedCallback, ImageHeightPropertyCoerceValueCallback));
        public static readonly DependencyProperty ImageHeightProperty = ImageHeightPropertyKey.DependencyProperty;

        public static readonly DependencyProperty ZoomLevelProperty =
           DependencyProperty.Register(nameof(ZoomLevel), typeof(double), typeof(CharacterControl),
               new FrameworkPropertyMetadata(1.0, ZoomLevelPropertyChangedCallback));

        private static void ImageHeightPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static object ImageHeightPropertyCoerceValueCallback(DependencyObject d, object baseValue)
        {
            if (d.GetValue(CharacterProperty) is ICharacter character)
            {
                var zoomLevel = (double)d.GetValue(ZoomLevelProperty);
                var metrics = character.GlyphMetrics;
                return metrics.VerticalAdvance * zoomLevel;
            }
            return baseValue;
        }

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
                //this.viewbox.UseLayoutRounding = false;
            }
            this.UpdateImageLayout();
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            var property = e.Property;
            if (property == CharacterProperty)
            {
                this.UpdateImageLayout();
            }
        }

        public ICharacter Character
        {
            get => (ICharacter)this.GetValue(CharacterProperty);
            set => this.SetValue(CharacterProperty, value);
        }

        public double ZoomLevel
        {
            get => (double)this.GetValue(ZoomLevelProperty);
            set => this.SetValue(ZoomLevelProperty, value);
        }

        public Thickness ImageMargin
        {
            get => (Thickness)this.GetValue(ImageMarginProperty);
        }

        public double ImageWidth
        {
            get => (double)this.GetValue(ImageWidthProperty);
        }

        public double ImageHeight
        {
            get => (double)this.GetValue(ImageHeightProperty);
        }

        private static void ZoomLevelPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.CoerceValue(ImageWidthProperty);
            d.CoerceValue(ImageHeightProperty);
            d.CoerceValue(ImageMarginProperty);
        }

        private void UpdateImageLayout()
        {
            //var metrics = this.Character.GlyphMetrics;
            //if (metrics.Width == 0 || metrics.Height == 0 || this.IsEnabled == false)
            //{
            //    this.ImageMargin = new Thickness(0);
            //    if (this.viewbox != null)
            //    {
            //        this.viewbox.Width = double.NaN;
            //        this.viewbox.Height = double.NaN;
            //    }
            //}
            //else
            //{
            //    var left = metrics.HorizontalBearingX;
            //    var top = metrics.BaseLine - metrics.HorizontalBearingY;
            //    var right = metrics.HorizontalAdvance - (left + metrics.Width);
            //    var bottom = metrics.VerticalAdvance - (top + metrics.Height);
            //    this.ImageMargin = new Thickness(left, top, right, bottom);
            //    if (this.viewbox != null)
            //    {
            //        this.viewbox.Width = metrics.HorizontalAdvance * this.ZoomLevel;
            //        this.viewbox.Height = metrics.VerticalAdvance * this.ZoomLevel;
            //    }
            //}
        }
    }
}
