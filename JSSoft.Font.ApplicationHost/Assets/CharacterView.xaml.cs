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
