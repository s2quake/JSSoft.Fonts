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

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;

namespace JSSoft.Fonts.ApplicationHost.Dialogs.Views
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
