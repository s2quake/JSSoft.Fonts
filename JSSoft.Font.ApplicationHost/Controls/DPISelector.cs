using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace JSSoft.Font.ApplicationHost.Controls
{
    [TemplatePart(Name = nameof(PART_EditableComboBox), Type = typeof(ComboBox))]
    public class DPISelector : Control
    {
        public const string PART_EditableComboBox = nameof(PART_EditableComboBox);

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(int), typeof(DPISelector),
                new FrameworkPropertyMetadata(0, ValuePropertyChangedCallback));

        private static readonly DependencyPropertyKey ValueListPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(ValueList), typeof(IList<int>), typeof(DPISelector),
                new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty ValueListProperty = ValueListPropertyKey.DependencyProperty;

        private static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(ComboBox.Text), typeof(string), typeof(DPISelector),
                new FrameworkPropertyMetadata(null, TextPropertyChangedCallback));

        private readonly ObservableCollection<int> valueList = new ObservableCollection<int>() { 72, 96 };
        private ComboBox comboBox;

        public DPISelector()
        {
            this.SetValue(ValueListPropertyKey, this.valueList);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.comboBox = this.Template.FindName(PART_EditableComboBox, this) as ComboBox;
            if (this.comboBox != null)
            {
                this.comboBox.ItemsSource = this.valueList;
                this.comboBox.SelectedItem = this.valueList.FirstOrDefault();
                BindingOperations.SetBinding(this, TextProperty, new Binding(nameof(ComboBox.Text)) { Source = this.comboBox });
            }
        }

        public int Value
        {
            get => (int)this.GetValue(ValueProperty);
            set => this.SetValue(ValueProperty, value);
        }

        public IList<int> ValueList => (IList<int>)this.GetValue(ValueListProperty);

        private static void ValuePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DPISelector control)
            {
                control.UpdateComboBoxText();
            }
        }

        private static void TextPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DPISelector control)
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
            var text = (string)this.GetValue(TextProperty);
            var value = int.Parse(text);
            this.SetValue(ValueProperty, value);
        }
    }
}
