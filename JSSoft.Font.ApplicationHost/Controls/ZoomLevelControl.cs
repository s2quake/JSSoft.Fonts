using Ntreev.ModernUI.Framework.DataGrid.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace JSSoft.Font.ApplicationHost.Controls
{
    [TemplatePart(Name = "PART_ComboBox", Type = typeof(ComboBox))]
    public class ZoomLevelControl : UserControl
    {
        private const string pattern = @"^(\d{1,5}\.{0,1}(?=\d{1,2})\d{0,2})\s*%";

        public static readonly DependencyProperty PopupPlacementProperty =
            DependencyProperty.Register(nameof(PopupPlacement), typeof(PlacementMode), typeof(ZoomLevelControl),
                new FrameworkPropertyMetadata(PlacementMode.Bottom, PopupPlacementPropertyChangedCallback));

        private static readonly DependencyPropertyKey ItemsPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(Items), typeof(IList<ZoomLevelItem>), typeof(ZoomLevelControl),
                new FrameworkPropertyMetadata());
        public static readonly DependencyProperty ItemsProperty = ItemsPropertyKey.DependencyProperty;

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(double), typeof(ZoomLevelControl),
                new FrameworkPropertyMetadata(1.0, ValuePropertyChangedCallback));

        private readonly ObservableCollection<ZoomLevelItem> itemList;
        private ComboBox comboBox;

        public ZoomLevelControl()
        {
            this.itemList = new ObservableCollection<ZoomLevelItem>()
            {
                new ZoomLevelItem() { Level = 0.5 },
                new ZoomLevelItem() { Level = 1 },
                new ZoomLevelItem() { Level = 1.5 },
                new ZoomLevelItem() { Level = 2 },
                new ZoomLevelItem() { Level = 3 },
                new ZoomLevelItem() { Level = 4 },
            };
            this.SetValue(ItemsPropertyKey, this.itemList);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (this.Template.FindName("PART_ComboBox", this) is ComboBox comboBox)
            {
                if (comboBox.ApplyTemplate())
                {
                    var popup = comboBox.Template.FindName("PART_Popup", comboBox) as Popup;
                    var textBox = comboBox.Template.FindName("PART_EditableTextBox", comboBox) as TextBox;
                    popup.Placement = this.PopupPlacement;
                    BindingOperations.SetBinding(popup, Popup.PlacementProperty, new Binding(nameof(PopupPlacement)) { Source = this });
                    textBox.LostFocus += TextBox_LostFocus;
                    textBox.PreviewKeyDown += TextBox_PreviewKeyDown;
                }

                BindingOperations.SetBinding(comboBox, ItemsControl.ItemsSourceProperty, new Binding(nameof(Items)) { Source = this });
                comboBox.DisplayMemberPath = nameof(ZoomLevelItem.Text);
                this.comboBox = comboBox;
                this.comboBox.DropDownClosed += ComboBox_DropDownClosed;
                this.UpdateText();
            }
        }

        public PlacementMode PopupPlacement
        {
            get => (PlacementMode)this.GetValue(PopupPlacementProperty);
            set => this.SetValue(PopupPlacementProperty, value);
        }

        public IList<ZoomLevelItem> Items => (IList<ZoomLevelItem>)this.GetValue(ItemsProperty);

        public double Value
        {
            get => (double)this.GetValue(ValueProperty);
            set => this.SetValue(ValueProperty, value);
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            this.UpdateText();
        }

        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (this.comboBox.SelectedItem is ZoomLevelItem item)
            {
                this.Value = item.Level;
            }
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.None && e.Key == Key.Enter)
            {
                if (this.comboBox.Text == string.Empty)
                {
                    this.Value = 1.0;
                }
                else if (Regex.Match(this.comboBox.Text, pattern) is Match match && match.Success == true)
                {
                    var numberText = match.Groups[1].Value;
                    if (double.TryParse(numberText, out var d) == true)
                    {
                        this.Value = d / 100.0;
                        this.comboBox.Text = $"{d:0.##} %";
                    }
                }
            }
            else if (Keyboard.Modifiers == ModifierKeys.None && e.Key == Key.Escape)
            {

            }
        }

        private static void PopupPlacementPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZoomLevelControl control)
            {
                control.UpdatePopupPlacement();
            }
        }

        private static void ValuePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZoomLevelControl control)
            {
                control.UpdateText();
            }
        }

        private void UpdatePopupPlacement()
        {
            if (this.comboBox is ComboBox comboBox)
            {
                var popup = comboBox.Template.FindName("PART_Popup", comboBox) as Popup;
                popup.Placement = this.PopupPlacement;
            }
        }

        private void UpdateText()
        {
            if (this.comboBox != null)
            {
                this.comboBox.Text = $"{this.Value * 100:0.##} %";
            }
        }
    }
}
