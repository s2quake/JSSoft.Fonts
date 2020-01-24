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
    [TemplatePart(Name = "PART_DataGrid", Type = typeof(ModernDataGridControl))]
    public class CharacterView : UserControl
    {
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

        private static readonly ICharacterRow[] emptyRows = new ICharacterRow[]
        {
            new CharacterRow(0x0),
            new CharacterRow(0x1),
            new CharacterRow(0x2),
            new CharacterRow(0x3),
            new CharacterRow(0x4),
            new CharacterRow(0x5),
            new CharacterRow(0x6),
            new CharacterRow(0x7),
            new CharacterRow(0x8),
            new CharacterRow(0x9),
            new CharacterRow(0xA),
            new CharacterRow(0xB),
            new CharacterRow(0xC),
            new CharacterRow(0xD),
            new CharacterRow(0xE),
            new CharacterRow(0xF),
        };

        private ModernDataGridControl gridControl;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.gridControl = this.Template.FindName("PART_DataGrid", this) as ModernDataGridControl;
            if (this.gridControl != null)
            {
                //var binding = new Binding($"{nameof(CharacterGroup)}.{nameof(ICharacterGroup.Items)}")
                //{
                //    Source = this,
                //};
                //BindingOperations.SetBinding(this.gridControl, ModernDataGridControl.ItemsSourceProperty, binding);
                this.gridControl.SelectionChanged += GridControl_SelectionChanged;
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
                this.Character.IsChecked = !this.Character.IsChecked;
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
            if (this.gridControl.CurrentItem is ICharacterRow row && this.gridControl.CurrentColumn is Column column)
            {
                var prop = typeof(ICharacterRow).GetProperty(column.FieldName);
                this.Character = prop.GetValue(row, null) as ICharacter;
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
                this.gridControl.ItemsSource = this.CharacterGroup.Items;
            }
            else
            {
                this.gridControl.ItemsSource = emptyRows;
            }
        }

        private void UpdateSelectedItem()
        {

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
