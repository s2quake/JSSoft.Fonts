using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace JSSoft.Font.ApplicationHost.ContentViews
{
    /// <summary>
    /// ToolBarView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ToolBarView : UserControl
    {
        public ToolBarView()
        {
            InitializeComponent();
        }

        public IUndoService UndoService => ApplicationService.GetUndoService(this);

        public ICharacterNavigator Navigator => ApplicationService.GetCharacterNavigator(this);

        private async void Item_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem)
            {
                if (menuItem.Tag is Popup popup)
                {
                    popup.IsOpen = false;
                }
                if (menuItem.DataContext is ICharacterNavigatorItem item)
                {
                    await this.Dispatcher.InvokeAsync(() => this.Navigator.Current = item, DispatcherPriority.Background);
                }
            }
        }

        private void UndoItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem)
            {
                if (menuItem.Tag is Popup popup)
                {
                    popup.IsOpen = false;
                }
                if (menuItem.DataContext is IUndo item)
                {
                    this.UndoService.Undo(item);
                }
            }
        }

        private void RedoItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem)
            {
                if (menuItem.Tag is Popup popup)
                {
                    popup.IsOpen = false;
                }
                if (menuItem.DataContext is IUndo item)
                {
                    this.UndoService.Redo(item);
                }
            }
        }
    }
}
