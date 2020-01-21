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

namespace JSSoft.Font.ApplicationHost.Controls
{
    [TemplatePart(Name = "PART_DataGrid", Type = typeof(ModernDataGridControl))]
    public class CharacterView : UserControl
    {
        public static readonly DependencyProperty CharacterGroupProperty =
            DependencyProperty.Register(nameof(CharacterGroup), typeof(ICharacterGroup), typeof(CharacterView),
                new FrameworkPropertyMetadata(CharacterGroupPropertyChangedCallback));

        public static readonly DependencyProperty VerticalAdvanceProperty =
            DependencyProperty.Register(nameof(VerticalAdvance), typeof(double), typeof(CharacterView),
                new FrameworkPropertyMetadata(VerticalAdvancePropertyChangedCallback));

        private static readonly DependencyPropertyKey ActualVerticalAdvancePropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(ActualVerticalAdvance), typeof(double), typeof(CharacterView),
                new FrameworkPropertyMetadata());
        public static readonly DependencyProperty ActualVerticalAdvanceProperty = ActualVerticalAdvancePropertyKey.DependencyProperty;

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
                this.UpdateItemsSource();
                this.UpdateActualVerticalAdvance();
            }
        }

        public ICharacterGroup CharacterGroup
        {
            get => (ICharacterGroup)this.GetValue(CharacterGroupProperty);
            set => this.SetValue(CharacterGroupProperty, value);
        }

        public double VerticalAdvance
        {
            get => (double)this.GetValue(VerticalAdvanceProperty);
            set => this.SetValue(VerticalAdvanceProperty, value);
        }

        public double ActualVerticalAdvance
        {
            get => (double)this.GetValue(ActualVerticalAdvanceProperty);
            private set => this.SetValue(ActualVerticalAdvancePropertyKey, value);
        }

        public double ZoomLevel
        {
            get => (double)this.GetValue(ZoomLevelProperty);
            set => this.SetValue(ZoomLevelProperty, value);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            return base.MeasureOverride(constraint);
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            //foreach (var item in this.Columns)
            //{
            //    //item.Width = 25;
            //}
            return base.ArrangeOverride(arrangeBounds);
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
        }

        private static void CharacterGroupPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CharacterView self)
            {
                self.UpdateItemsSource();
            }
        }

        private static void VerticalAdvancePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CharacterView self)
            {
                self.UpdateActualVerticalAdvance();
            }
        }

        private static void ZoomLevelPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CharacterView self)
            {
                self.UpdateActualVerticalAdvance();
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

        private void UpdateActualVerticalAdvance()
        {
            var actualVerticalAdvance = (int)(this.VerticalAdvance * this.ZoomLevel);
            this.ActualVerticalAdvance = actualVerticalAdvance;
            foreach (var item in this.gridControl.Columns)
            {
                item.Width = actualVerticalAdvance;
                item.MinWidth = actualVerticalAdvance;
                item.MaxWidth = actualVerticalAdvance;
            }
        }
    }
}
