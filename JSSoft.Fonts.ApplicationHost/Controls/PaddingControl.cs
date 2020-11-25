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
    [TemplatePart(Name = PART_Left, Type = typeof(TextBox))]
    [TemplatePart(Name = PART_Top, Type = typeof(TextBox))]
    [TemplatePart(Name = PART_Right, Type = typeof(TextBox))]
    [TemplatePart(Name = PART_Bottom, Type = typeof(TextBox))]
    public class PaddingControl : Control
    {
        public const string PART_Left = nameof(PART_Left);
        public const string PART_Top = nameof(PART_Top);
        public const string PART_Right = nameof(PART_Right);
        public const string PART_Bottom = nameof(PART_Bottom);

        public static readonly DependencyProperty ValueProperty =
           DependencyProperty.Register(nameof(Value), typeof(FontPadding), typeof(PaddingControl),
               new FrameworkPropertyMetadata(default(FontPadding), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ValuePropertyChangedCallback));

        public static readonly RoutedEvent ValueChangedEvent =
            EventManager.RegisterRoutedEvent(nameof(ValueChanged), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PaddingControl));

        private TextBox leftControl;
        private TextBox topControl;
        private TextBox rightControl;
        private TextBox bottomControl;

        public PaddingControl()
        {

        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.DetachEvent(this.leftControl);
            this.DetachEvent(this.topControl);
            this.DetachEvent(this.rightControl);
            this.DetachEvent(this.bottomControl);
            this.leftControl = this.Template.FindName(PART_Left, this) as TextBox;
            this.topControl = this.Template.FindName(PART_Top, this) as TextBox;
            this.rightControl = this.Template.FindName(PART_Right, this) as TextBox;
            this.bottomControl = this.Template.FindName(PART_Bottom, this) as TextBox;
            this.UpdateValue(this.leftControl, this.Value.Left);
            this.UpdateValue(this.topControl, this.Value.Top);
            this.UpdateValue(this.rightControl, this.Value.Right);
            this.UpdateValue(this.bottomControl, this.Value.Bottom);
            this.AttachEvent(this.leftControl);
            this.AttachEvent(this.topControl);
            this.AttachEvent(this.rightControl);
            this.AttachEvent(this.bottomControl);
        }

        public FontPadding Value
        {
            get => (FontPadding)this.GetValue(ValueProperty);
            set => this.SetValue(ValueProperty, value);
        }

        public event RoutedEventHandler ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }

        private static void ValuePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PaddingControl self)
            {
                self.UpdateValue();
            }
        }

        private void UpdateValue()
        {
            this.UpdateValue(this.leftControl, this.Value.Left);
            this.UpdateValue(this.topControl, this.Value.Top);
            this.UpdateValue(this.rightControl, this.Value.Right);
            this.UpdateValue(this.bottomControl, this.Value.Bottom);
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
                var leftValue = this.Value.Left;
                var topValue = this.Value.Top;
                var rightValue = this.Value.Right;
                var bottomValue = this.Value.Bottom;
                if (this.leftControl == textBox)
                {
                    leftValue = int.Parse(this.leftControl.Text);
                }
                else if (this.topControl == textBox)
                {
                    topValue = int.Parse(this.topControl.Text);
                }
                else if (this.rightControl == textBox)
                {
                    rightValue = int.Parse(this.rightControl.Text);
                }
                else if (this.bottomControl == textBox)
                {
                    bottomValue = int.Parse(this.bottomControl.Text);
                }
                var value = new FontPadding(leftValue, topValue, rightValue, bottomValue);
                if (this.Value != value)
                {
                    this.Value = value;
                }
            }
        }
    }
}
