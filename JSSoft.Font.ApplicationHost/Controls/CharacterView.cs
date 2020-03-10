using Ntreev.Library.Linq;
using Ntreev.ModernUI.Framework.DataGrid.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Xceed.Wpf.DataGrid;

namespace JSSoft.Font.ApplicationHost.Controls
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

        private readonly CharacterGroupView groupView = new CharacterGroupView();
        private ModernDataGridControl gridControl;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.gridControl = this.Template.FindName(PART_DataGrid, this) as ModernDataGridControl;
            if (this.gridControl != null)
            {
                this.gridControl.SelectionChanged += GridControl_SelectionChanged;
                this.gridControl.PropertyChanged += GridControl_PropertyChanged;
                this.gridControl.ItemsSource = this.groupView;
                this.UpdateItemsSource();
                this.UpdateActualItemHeight();
            }
        }

        private void GridControl_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentItem")
            {
                int qwer = 0;
            }

            if (e.PropertyName == "CurrentColumn")
            {
                int qwer = 0;
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
