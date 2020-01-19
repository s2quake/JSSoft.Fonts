using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JSSoft.Font.Controls
{
    /// <summary>
    /// CharacterControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CharacterControl : UserControl
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(CharacterControl));

        public CharacterControl()
        {
            InitializeComponent();
        }

        protected override Size MeasureOverride(Size constraint)
        {
            var size = base.MeasureOverride(constraint);
            var desireLength = Math.Max(size.Width, size.Height);
            var width = double.IsPositiveInfinity(constraint.Width) ? desireLength : constraint.Width;
            var height = double.IsPositiveInfinity(constraint.Height) ? desireLength : constraint.Height;
            var actualLength = Math.Max(width, height);
            return new Size(actualLength, actualLength);
        }

        public string Text
        {
            get => (string)this.GetValue(TextProperty);
            set => this.SetValue(TextProperty, value);
        }
    }
}
