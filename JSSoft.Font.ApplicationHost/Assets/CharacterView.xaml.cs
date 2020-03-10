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

using JSSoft.Font.ApplicationHost.UndoActions;
using Ntreev.ModernUI.Framework.DataGrid.Controls;
using System.Windows;
using System.Windows.Input;
using Xceed.Wpf.DataGrid;

namespace JSSoft.Font.ApplicationHost.Assets
{
    partial class CharacterView : ResourceDictionary
    {
        public CharacterView()
        {
            InitializeComponent();
        }

        private bool VerifyModernDataCell(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                return false;
            if (!(sender is ModernDataCell cell))
                return false;
            var shell = ApplicationService.GetShell(cell);
            if (shell == null || shell.IsOpened == false)
                return false;
            if (!(cell.DataContext is CharacterRowView rowView))
                return false;
            var column = cell.ParentColumn;
            if (!(rowView[column.Index] is ICharacter))
                return false;
            return true;
        }

        private void ModernDataCell_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.VerifyModernDataCell(sender, e) == false)
                return;

            var cell = sender as ModernDataCell;
            var rowView = cell.DataContext as CharacterRowView;
            var undoService = ApplicationService.GetUndoService(cell);
            var column = cell.ParentColumn;
            var item = rowView[column.Index] as ICharacter;
            var isChecked = item.IsChecked;
            if (undoService != null)
            {
                if (isChecked == true)
                {
                    undoService.Execute(new UncheckCharacterAction(item));
                }
                else
                {
                    undoService.Execute(new CheckCharacterAction(item));
                }
            }
            else
            {
                item.IsChecked = !item.IsChecked;
            }
        }

        private bool VerifyRowSelector(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                return false;
            if (!(sender is RowSelector rowSelector))
                return false;
            var shell = ApplicationService.GetShell(rowSelector);
            if (shell == null || shell.IsOpened == false)
                return false;
            var dataRow = rowSelector.DataContext as ModernDataRow;
            if (!(dataRow.DataContext is CharacterRowView))
                return false;
            return true;
        }

        private void RowSelector_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.VerifyRowSelector(sender, e) == false)
                return;

            var rowSelector = sender as RowSelector;
            var dataRow = rowSelector.DataContext as ModernDataRow;
            var rowView = dataRow.DataContext as CharacterRowView;
            var row = rowView.Row;
            var undoService = ApplicationService.GetUndoService(rowSelector);
            var isChecked = row.IsChecked ?? false;
            if (undoService != null)
            {
                if (isChecked == true)
                {
                    undoService.Execute(new UncheckCharacterRowAction(row));
                }
                else
                {
                    undoService.Execute(new CheckCharacterRowAction(row));
                }
            }
            else
            {
                row.IsChecked = !isChecked;
            }
        }
    }
}
