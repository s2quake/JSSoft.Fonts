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

using JSSoft.Library.Linq;
using JSSoft.ModernUI.Framework.DataGrid.Controls;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Xceed.Wpf.DataGrid;

namespace JSSoft.Fonts.ApplicationHost.Controls
{
    [TemplatePart(Name = PART_DataGrid, Type = typeof(ModernDataGridControl))]
    public class CharacterView : Control
    {
        public const string PART_DataGrid = nameof(PART_DataGrid);

        public static readonly DependencyProperty CharacterGroupProperty =
            DependencyProperty.Register(nameof(CharacterGroup), typeof(ICharacterGroup), typeof(CharacterView),
                new FrameworkPropertyMetadata(CharacterGroupPropertyChangedCallback));

        public static readonly DependencyProperty CharacterProperty =
            DependencyProperty.Register(nameof(Character), typeof(ICharacter), typeof(CharacterView),
                new FrameworkPropertyMetadata(CharacterPropertyChangedCallback));

        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.Register(nameof(ItemHeight), typeof(double), typeof(CharacterView),
                new FrameworkPropertyMetadata(50.0, ItemHeightPropertyChangedCallback));

        public static readonly DependencyProperty ItemThicknessProperty =
            DependencyProperty.Register(nameof(ItemThickness), typeof(Thickness), typeof(CharacterView),
                new FrameworkPropertyMetadata(ItemThicknessPropertyChangedCallback));

        private static readonly DependencyPropertyKey ActualItemHeightPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(ActualItemHeight), typeof(double), typeof(CharacterView),
                new FrameworkPropertyMetadata());
        public static readonly DependencyProperty ActualItemHeightProperty = ActualItemHeightPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey ActualItemWidthPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(ActualItemWidth), typeof(double), typeof(CharacterView),
                new FrameworkPropertyMetadata());
        public static readonly DependencyProperty ActualItemWidthProperty = ActualItemWidthPropertyKey.DependencyProperty;

        public static readonly DependencyProperty ZoomLevelProperty =
            DependencyProperty.Register(nameof(ZoomLevel), typeof(double), typeof(CharacterView),
                new FrameworkPropertyMetadata(1.0, ZoomLevelPropertyChangedCallback));

        private readonly CharacterGroupInfoView groupView = new CharacterGroupInfoView();
        private ModernDataGridControl gridControl;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.gridControl = this.Template.FindName(PART_DataGrid, this) as ModernDataGridControl;
            if (this.gridControl != null)
            {
                this.gridControl.SelectionChanged += GridControl_SelectionChanged;
                this.gridControl.ItemsSource = this.groupView;
                this.UpdateItemsSource();
                this.UpdateActualItemHeight();
            }
        }

        public ICharacterGroup CharacterGroup
        {
            get => (ICharacterGroup)this.GetValue(CharacterGroupProperty);
            set => this.SetValue(CharacterGroupProperty, value);
        }

        public ICharacter Character
        {
            get => (ICharacter)this.GetValue(CharacterProperty);
            set => this.SetValue(CharacterProperty, value);
        }

        public double ItemHeight
        {
            get => (double)this.GetValue(ItemHeightProperty);
            set => this.SetValue(ItemHeightProperty, value);
        }

        public Thickness ItemThickness
        {
            get => (Thickness)this.GetValue(ItemThicknessProperty);
            set => this.SetValue(ItemThicknessProperty, value);
        }

        public double ActualItemHeight
        {
            get => (double)this.GetValue(ActualItemHeightProperty);
            private set => this.SetValue(ActualItemHeightPropertyKey, value);
        }

        public double ActualItemWidth
        {
            get => (double)this.GetValue(ActualItemWidthProperty);
            private set => this.SetValue(ActualItemWidthPropertyKey, value);
        }

        public double ZoomLevel
        {
            get => (double)this.GetValue(ZoomLevelProperty);
            set => this.SetValue(ZoomLevelProperty, value);
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (Keyboard.Modifiers == ModifierKeys.None && e.Key == Key.Space && this.Character != null)
            {
                foreach (var item in this.gridControl.SelectedCellRanges)
                {
                    this.ToggleChecked(item);
                }
            }
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnMouseDoubleClick(e);
        }

        private void ToggleChecked(SelectionCellRange range)
        {
            var gridContext = this.gridControl.CurrentContext;
            var query = from item in gridContext.EnumerateItems(range.ItemRange)
                        let row = item as ICharacterRow
                        from column in gridContext.EnumerateColumns(range.ColumnRange)
                        select row.Items[column.Index];
            foreach (var item in query.ToArray())
            {
                item.IsChecked = !item.IsChecked;
            }
        }

        private static void CharacterGroupPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CharacterView self)
            {
                self.UpdateItemsSource();
            }
        }

        private static void CharacterPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CharacterView self)
            {
                self.UpdateSelectedItem();
            }
        }

        private static void ItemHeightPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CharacterView self)
            {
                self.UpdateActualItemHeight();
            }
        }

        private static void ItemThicknessPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CharacterView self)
            {
                self.UpdateActualItemHeight();
            }
        }

        private static void ZoomLevelPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CharacterView self)
            {
                self.UpdateActualItemHeight();
            }
        }

        private void GridControl_SelectionChanged(object sender, Xceed.Wpf.DataGrid.DataGridSelectionChangedEventArgs e)
        {
            if (this.gridControl.CurrentItem is CharacterRowView rowView && this.gridControl.CurrentColumn is Column column)
            {
                if (rowView[column.Index] is ICharacter item)
                    this.Character = item;
            }
            else
            {
                this.Character = null;
            }
        }

        private void UpdateItemsSource()
        {
            if (this.CharacterGroup != null && this.CharacterGroup.Items != null)
            {
                this.groupView.CharacterGroup = this.CharacterGroup;
                this.Character = null;
            }
            else
            {
                this.groupView.CharacterGroup = null;
                this.Character = null;
            }
        }

        private void UpdateSelectedItem()
        {
            if (this.Character != null && this.Character.Row is ICharacterRow row)
            {
                this.gridControl.SelectionChanged -= GridControl_SelectionChanged;
                this.gridControl.SelectedCellRanges.Clear();
                var column = row.Items.IndexOf(this.Character);
                for (var i = 0; i < this.gridControl.Items.Count; i++)
                {
                    if (this.gridControl.Items.GetItemAt(i) is CharacterRowView rowView && rowView.Row == row)
                    {
                        if (object.Equals(this.gridControl.CurrentItem, rowView) == false)
                        {
                            this.gridControl.CurrentItem = rowView;
                        }
                        break;
                    }
                }

                for (var i = 0; i < this.gridControl.Columns.Count; i++)
                {
                    if (this.gridControl.Columns[i] is ColumnBase columnBase && columnBase.FieldName == $"Item{column:X}")
                    {
                        if (this.gridControl.CurrentColumn != columnBase)
                        {
                            this.gridControl.CurrentColumn = columnBase;
                        }
                        break;
                    }
                }
                this.gridControl.SelectionChanged += GridControl_SelectionChanged;
            }
        }

        private void UpdateActualItemHeight()
        {
            this.ActualItemHeight = (int)(this.ItemHeight * this.ZoomLevel + this.ItemThickness.Top + this.ItemThickness.Bottom) + 1;
            this.ActualItemWidth = (int)(this.ItemHeight * this.ZoomLevel + this.ItemThickness.Left + this.ItemThickness.Right) + 1;
            if (this.gridControl != null)
            {
                foreach (var item in this.gridControl.Columns)
                {
                    item.Width = ActualItemWidth;
                    item.MinWidth = ActualItemWidth;
                    item.MaxWidth = ActualItemWidth;
                }
            }
        }
    }
}
