using Ntreev.ModernUI.Framework.DataGrid.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public static readonly DependencyProperty PopupPlacementProperty =
            DependencyProperty.Register(nameof(PopupPlacement), typeof(PlacementMode), typeof(ZoomLevelControl));

        private ComboBox comboBox;
        public ZoomLevelControl()
        {

        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (this.Template.FindName("PART_ComboBox", this) is ComboBox comboBox)
            {
                if (comboBox.ApplyTemplate())
                {
                    var popup = comboBox.Template.FindName("PART_Popup", comboBox) as Popup;
                    popup.Placement = PlacementMode.Top;
                }

                this.comboBox = comboBox;
            }
        }

        public PlacementMode PopupPlacement
        {
            get => (PlacementMode)this.GetValue(PopupPlacementProperty);
            set => this.SetValue(PopupPlacementProperty, value);
        }
    }
}
