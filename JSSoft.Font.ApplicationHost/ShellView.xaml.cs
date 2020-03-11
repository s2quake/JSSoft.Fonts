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

using FirstFloor.ModernUI.Windows.Controls;
using JSSoft.Font.ApplicationHost.Input;
using JSSoft.Font.ApplicationHost.UndoActions;
using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
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
using System.Windows.Shapes;

namespace JSSoft.Font.ApplicationHost
{
    /// <summary>
    /// ShellView.xaml에 대한 상호 작용 논리
    /// </summary>
    [Export]
    public partial class ShellView : ModernWindow
    {
        public ShellView()
        {
            InitializeComponent();
        }

        [ImportingConstructor]
        public ShellView(IShell shell, IAppConfiguration configs, ICharacterNavigator navigator, IUndoService undoService)
        {
            InitializeComponent();
            this.CommandBindings.Add(new CommandBinding(FontCommands.NavigateBackward, NavigateBackward_Execute, NavigateBackward_CanExecute));
            this.CommandBindings.Add(new CommandBinding(FontCommands.NavigateForward, NavigateForward_Execute, NavigateForward_CanExecute));
            this.CommandBindings.Add(new CommandBinding(FontCommands.Undo, Undo_Execute, Undo_CanExecute));
            this.CommandBindings.Add(new CommandBinding(FontCommands.Redo, Redo_Execute, Redo_CanExecute));
            ApplicationService.SetShell(this, shell);
            ApplicationService.SetConfigs(this, configs);
            ApplicationService.SetCharacterNavigator(this, navigator);
            ApplicationService.SetUndoService(this, undoService);
        }

        public ICharacterNavigator Navigator => ApplicationService.GetCharacterNavigator(this);

        public IUndoService UndoService => ApplicationService.GetUndoService(this);

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
        }

        private void NavigateBackward_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            this.Navigator.Backward();
        }

        private void NavigateBackward_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.Navigator.CanBackward;
            e.Handled = true;
        }

        private void NavigateForward_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            this.Navigator.Forward();
        }

        private void NavigateForward_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.Navigator.CanForward;
            e.Handled = true;
        }

        private void Undo_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            this.UndoService.Undo();
            e.Handled = true;
        }

        private void Undo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.UndoService.CanUndo;
            e.Handled = true;

            if (e.CanExecute == true)
            {
                int qwer = 0;
            }
        }

        private void Redo_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            this.UndoService.Redo();
            e.Handled = true;
        }

        private void Redo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.UndoService.CanRedo;
            e.Handled = true;
        }
    }
}
