// MIT License
// 
// Copyright (c) 2020 Jeesu Choi
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using JSSoft.ModernUI.Framework;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
