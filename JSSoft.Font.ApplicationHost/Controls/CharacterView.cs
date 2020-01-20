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

        private ModernDataGridControl gridControl;

        public ICharacterGroup CharacterGroup
        {
            get => (ICharacterGroup)this.GetValue(CharacterGroupProperty);
            set => this.SetValue(CharacterGroupProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.gridControl = this.Template.FindName("PART_DataGrid", this) as ModernDataGridControl;
            if (this.gridControl != null)
            {
                var binding = new Binding($"{nameof(CharacterGroup)}.{nameof(ICharacterGroup.Items)}")
                {
                    Source = this,
                };
                BindingOperations.SetBinding(this.gridControl, ModernDataGridControl.ItemsSourceProperty, binding); 
            }
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
            
        }
    }
}
