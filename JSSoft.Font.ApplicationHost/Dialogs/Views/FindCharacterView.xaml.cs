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
using System.Windows.Threading;

namespace JSSoft.Font.ApplicationHost.Dialogs.Views
{
    /// <summary>
    /// FindCharacterView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class FindCharacterView : UserControl
    {
        public FindCharacterView()
        {
            InitializeComponent();
            this.Dispatcher.InvokeAsync(() => this.Character.Focus(), DispatcherPriority.Background);
        }

        private void ByCharacter_Checked(object sender, RoutedEventArgs e)
        {
            if (BindingOperations.GetBindingExpression(this.Character, TextBox.TextProperty) is BindingExpression expression)
            {
                expression.UpdateSource();
            }
            
        }

        private void ByCharacter_Unchecked(object sender, RoutedEventArgs e)
        {
            if (BindingOperations.GetBindingExpression(this.Character, TextBox.TextProperty) is BindingExpression expression)
            {
                expression.UpdateSource();
            }
        }
    }
}
