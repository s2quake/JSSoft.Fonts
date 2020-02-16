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

namespace JSSoft.Font.ApplicationHost.Dialogs.Views
{
    /// <summary>
    /// PreviewView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PreviewView : UserControl
    {
        public PreviewView()
        {
            InitializeComponent();
        }

        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            this.ZoomLevel.Value *= 2;
        }

        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            this.ZoomLevel.Value /= 2;
        }
    }
}
