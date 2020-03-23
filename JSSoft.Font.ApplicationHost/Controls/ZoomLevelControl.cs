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
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Shapes;

namespace JSSoft.Font.ApplicationHost.Controls
{
    [TemplatePart(Name = PART_ComboBox, Type = typeof(ComboBox))]
    public class ZoomLevelControl : Control
    {
        public const string PART_ComboBox = nameof(PART_ComboBox);
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
            if (this.Template.FindName(PART_ComboBox, this) is ComboBox comboBox)
            {
                if (comboBox.ApplyTemplate())
                {
                    var popup = comboBox.Template.FindName("PART_Popup", comboBox) as Popup;
                    var textBox = comboBox.Template.FindName("PART_EditableTextBox", comboBox) as TextBox;
                    popup.Placement = this.PopupPlacement;
                    BindingOperations.SetBinding(popup, Popup.PlacementProperty, new Binding(nameof(PopupPlacement)) { Source = this });
                    textBox.VerticalAlignment = VerticalAlignment.Center;
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
