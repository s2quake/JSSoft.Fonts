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

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace JSSoft.Font.ApplicationHost.Controls
{
    public class ValueSelector : Control
    {
        public const string PART_EditableComboBox = nameof(PART_EditableComboBox);

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(int), typeof(ValueSelector),
                new FrameworkPropertyMetadata(0, ValuePropertyChangedCallback));

        public static readonly DependencyProperty ValuesProperty =
            DependencyProperty.Register(nameof(Values), typeof(int[]), typeof(ValueSelector),
                new FrameworkPropertyMetadata(new int[] { }, (d, e) => { }, (d, baseValue) => baseValue ?? new int[] { }));

        private static readonly DependencyPropertyKey TextPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(ComboBox.Text), typeof(string), typeof(ValueSelector),
                new FrameworkPropertyMetadata($"{0}", TextPropertyChangedCallback, TextPropertyCoerceValueCallback));
        public static readonly DependencyProperty TextProperty = TextPropertyKey.DependencyProperty;

        private ComboBox comboBox;
        private TextChangedEventHandler handler;

        public ValueSelector()
        {

        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (this.comboBox != null && this.handler != null)
            {
                this.comboBox.RemoveHandler(TextBoxBase.TextChangedEvent, handler);
            }
            this.comboBox = this.Template.FindName(PART_EditableComboBox, this) as ComboBox;
            if (this.comboBox != null)
            {
                this.handler = new TextChangedEventHandler(ComboBox_TextChanged);
                this.comboBox.AddHandler(TextBoxBase.TextChangedEvent, this.handler);

            }
        }

        public int Value
        {
            get => (int)this.GetValue(ValueProperty);
            set => this.SetValue(ValueProperty, value);
        }

        public string Text => (string)this.GetValue(TextProperty);

        [TypeConverter(typeof(ValuesTypeConverter))]
        public int[] Values
        {
            get => (int[])this.GetValue(ValuesProperty);
            set => this.SetValue(ValuesProperty, value);
        }

        private void ComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.Source is TextBox textBox)
            {
                this.SetValue(ValueProperty, int.Parse(textBox.Text));
            }
        }

        private static void ValuePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.CoerceValue(TextProperty);
        }

        private static void TextPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static object TextPropertyCoerceValueCallback(DependencyObject d, object baseValue)
        {
            return $"{d.GetValue(ValueProperty)}";
        }
    }
}
