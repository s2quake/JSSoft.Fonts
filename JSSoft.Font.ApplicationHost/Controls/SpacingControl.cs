using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace JSSoft.Font.ApplicationHost.Controls
{
    public class SpacingControl : UserControl
    {
        public static readonly DependencyProperty HorizontalProperty =
            DependencyProperty.Register(nameof(Horizontal), typeof(double), typeof(SpacingControl));

        public static readonly DependencyProperty VerticalProperty =
            DependencyProperty.Register(nameof(Vertical), typeof(double), typeof(SpacingControl));

        public SpacingControl()
        {

        }

        public double Horizontal
        {
            get => (double)this.GetValue(HorizontalProperty);
            set => this.SetValue(HorizontalProperty, value);
        }

        public double Vertical
        {
            get => (double)this.GetValue(VerticalProperty);
            set => this.SetValue(VerticalProperty, value);
        }
    }
}
