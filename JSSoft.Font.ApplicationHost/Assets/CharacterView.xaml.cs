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

        private void ModernDataCell_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ModernDataCell cell && cell.DataContext is CharacterRowView rowView && e.LeftButton == MouseButtonState.Pressed)
            {
                var column = cell.ParentColumn;
                if (rowView[column.Index] is ICharacter item)
                {
                    item.IsChecked = !item.IsChecked;
                }
            }
        }

        private void RowSelector_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is RowSelector rowSelector && e.LeftButton == MouseButtonState.Pressed)
            {
                if (rowSelector.DataContext is ModernDataRow dataRow && dataRow.DataContext is CharacterRowView rowView)
                {
                    var isChecked = rowView.IsChecked ?? false;
                    rowView.IsChecked = !isChecked;
                }
            }
        }
    }
}
