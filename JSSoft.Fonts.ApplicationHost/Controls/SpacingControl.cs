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

using System.Windows;
using System.Windows.Controls;

namespace JSSoft.Fonts.ApplicationHost.Controls
{
    [TemplatePart(Name = PART_Horizontal, Type = typeof(TextBox))]
    [TemplatePart(Name = PART_Vertical, Type = typeof(TextBox))]
    public class SpacingControl : Control
    {
        public const string PART_Horizontal = nameof(PART_Horizontal);
        public const string PART_Vertical = nameof(PART_Vertical);

        public static readonly DependencyProperty ValueProperty =
           DependencyProperty.Register(nameof(Value), typeof(FontSpacing), typeof(SpacingControl),
               new FrameworkPropertyMetadata(default(FontSpacing), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ValuePropertyChangedCallback));

        public static readonly RoutedEvent ValueChangedEvent =
            EventManager.RegisterRoutedEvent(nameof(ValueChanged), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SpacingControl));

        private TextBox horzControl;
        private TextBox vertControl;

        public SpacingControl()
        {

        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.DetachEvent(this.horzControl);
            this.DetachEvent(this.vertControl);
            this.horzControl = this.Template.FindName(PART_Horizontal, this) as TextBox;
            this.vertControl = this.Template.FindName(PART_Vertical, this) as TextBox;
            this.UpdateValue(this.horzControl, this.Value.Horizontal);
            this.UpdateValue(this.vertControl, this.Value.Vertical);
            this.AttachEvent(this.horzControl);
            this.AttachEvent(this.vertControl);
        }

        public FontSpacing Value
        {
            get => (FontSpacing)this.GetValue(ValueProperty);
            set => this.SetValue(ValueProperty, value);
        }

        public event RoutedEventHandler ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }

        private static void ValuePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SpacingControl self)
            {
                self.UpdateValue();
            }
        }

        private void UpdateValue()
        {
            this.UpdateValue(this.horzControl, this.Value.Horizontal);
            this.UpdateValue(this.vertControl, this.Value.Vertical);
            this.RaiseEvent(new RoutedEventArgs(ValueChangedEvent));
        }

        private void AttachEvent(TextBox textBox)
        {
            if (textBox != null)
            {
                textBox.TextChanged += TextBox_TextChanged;
                textBox.GotFocus += TextBox_GotFocus;
            }
        }

        private void DetachEvent(TextBox textBox)
        {
            if (textBox != null)
            {
                textBox.TextChanged += TextBox_TextChanged;
                textBox.GotFocus += TextBox_GotFocus;
            }
        }

        private void UpdateValue(TextBox textBox, int value)
        {
            var textValue = $"{value}";
            if (textBox != null && textBox.Text != textValue)
            {
                textBox.Text = textValue;
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.SelectAll();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                var horzValue = this.Value.Horizontal;
                var vertValue = this.Value.Vertical;
                if (this.horzControl == textBox)
                {
                    horzValue = int.Parse(this.horzControl.Text);
                }
                else if (this.vertControl == textBox)
                {
                    vertValue = int.Parse(this.vertControl.Text);
                }
                var value = new FontSpacing(horzValue, vertValue);
                if (this.Value != value)
                {
                    this.Value = value;
                }
            }
        }
    }
}
