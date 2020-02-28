using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace JSSoft.Font.ApplicationHost.Controls
{
    [TemplatePart(Name = nameof(PART_EditableComboBox), Type = typeof(ComboBox))]
    public class ValueSelector : Control
    {
        public const string PART_EditableComboBox = nameof(PART_EditableComboBox);

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(int), typeof(ValueSelector),
                new FrameworkPropertyMetadata(0, ValuePropertyChangedCallback));

        public static readonly DependencyProperty ValuesProperty =
            DependencyProperty.Register(nameof(Values), typeof(int[]), typeof(ValueSelector),
                new FrameworkPropertyMetadata(null));

        private static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(ComboBox.Text), typeof(string), typeof(ValueSelector),
                new FrameworkPropertyMetadata(null, TextPropertyChangedCallback));

        private ComboBox comboBox;

        public ValueSelector()
        {

        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.comboBox = this.Template.FindName(PART_EditableComboBox, this) as ComboBox;
            if (this.comboBox != null)
            {
                this.comboBox.Text = $"{0}";
                BindingOperations.SetBinding(this.comboBox, ItemsControl.ItemsSourceProperty, new Binding(nameof(Values)) { Source = this });
                BindingOperations.SetBinding(this, TextProperty, new Binding(nameof(ComboBox.Text)) { Source = this.comboBox });
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

        private static void ValuePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ValueSelector control)
            {
                control.UpdateComboBoxText();
            }
        }

        private static void TextPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ValueSelector control)
            {
                control.UpdateValue();
            }
        }

        private void UpdateComboBoxText()
        {
            if (this.comboBox != null)
            {
                this.comboBox.Text = $"{this.Value}";
            }
        }

        private void UpdateValue()
        {
            if (int.TryParse(this.Text, out var value) == true)
            {
                this.SetValue(ValueProperty, value);
            }
            else
            {
                this.SetValue(ValueProperty, 0);
            }
        }
    }
}
