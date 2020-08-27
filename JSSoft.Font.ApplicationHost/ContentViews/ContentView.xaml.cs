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

using JSSoft.Font.ApplicationHost.Input;
using JSSoft.Font.ApplicationHost.UndoActions;
using JSSoft.ModernUI.Framework;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace JSSoft.Font.ApplicationHost.ContentViews
{
    /// <summary>
    /// ContentView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ContentView : UserControl
    {
        private GridLength propertyWidth;
        private double propertyMinWidth;

        private readonly CommandBinding showPropertyWindowCommand;
        private readonly CommandBinding HidePropertyWindowCommand;

        public ContentView()
        {
            InitializeComponent();
            this.showPropertyWindowCommand = new CommandBinding(FontCommands.ShowPropertyWindow, ShowPropertyWindow_Execute, ShowPropertyWindow_CanExecute);
            this.HidePropertyWindowCommand = new CommandBinding(FontCommands.HidePropertyWindow, HidePropertyWindow_Execute, HidePropertyWindow_CanExecute);
        }

        public IUndoService UndoService => ApplicationService.GetUndoService(this);

        public IAppConfiguration Configs => ApplicationService.GetConfigs(this);

        private void ShowPropertyWindow_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            this.PropertyWindow.Visibility = Visibility.Visible;
            this.PropertyWindowColumn.Width = this.propertyWidth;
            this.PropertyWindowColumn.MinWidth = this.propertyMinWidth;
            e.Handled = true;
            this.Focus();
        }

        private void ShowPropertyWindow_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.PropertyWindow.Visibility != Visibility.Visible;
            e.Handled = true;
        }

        private void HidePropertyWindow_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            this.propertyWidth = this.PropertyWindowColumn.Width;
            this.propertyMinWidth = this.PropertyWindowColumn.MinWidth;
            this.PropertyWindow.Visibility = Visibility.Collapsed;
            this.PropertyWindowColumn.Width = new GridLength(0);
            this.PropertyWindowColumn.MinWidth = 0;
            e.Handled = true;
            this.Focus();
        }

        private void HidePropertyWindow_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.PropertyWindow.Visibility == Visibility.Visible;
            e.Handled = true;
        }

        private void Expander_Loaded(object sender, RoutedEventArgs e)
        {
            var expander = sender as Expander;
            if (expander.DataContext == null)
                return;

            if (this.Configs.TryGetValue<bool>(this.GetType(), expander.DataContext.GetType(), nameof(expander.IsExpanded), out var isExpanded) == true)
            {
                expander.IsExpanded = isExpanded;
            }
        }

        private void Expander_Unloaded(object sender, RoutedEventArgs e)
        {
            var expander = sender as Expander;
            if (expander.DataContext == null)
                return;

            this.Configs.SetValue(this.GetType(), expander.DataContext.GetType(), nameof(expander.IsExpanded), expander.IsExpanded);
        }

        private void ListBoxItem_CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.DataContext is ICharacterGroup group)
            {
                var isChecked = group.IsChecked ?? false;
                if (isChecked == true)
                {
                    this.UndoService.Execute(new UncheckCharacterGroupAction(group));
                }
                else
                {
                    this.UndoService.Execute(new CheckCharacterGroupAction(group));
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is Window window)
            {
                window.CommandBindings.Add(this.showPropertyWindowCommand);
                window.CommandBindings.Add(this.HidePropertyWindowCommand);
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is Window window)
            {
                window.CommandBindings.Remove(this.showPropertyWindowCommand);
                window.CommandBindings.Remove(this.HidePropertyWindowCommand);
            }
        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            var expander = sender as Expander;
            if (expander.DataContext == null)
                return;

            this.Configs.SetValue(this.GetType(), expander.DataContext.GetType(), nameof(expander.IsExpanded), expander.IsExpanded);
        }
    }
}
