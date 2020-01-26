using Ntreev.ModernUI.Framework.DataGrid.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            if (sender is ModernDataCell cell && cell.DataContext is ICharacterRow row && e.LeftButton == MouseButtonState.Pressed)
            {
                var column = cell.ParentColumn;
                var item = row.Items[column.Index];
                item.IsChecked = !item.IsChecked;
            }
        }

        private void RowSelector_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is RowSelector rowSelector && e.LeftButton == MouseButtonState.Pressed)
            {
                if (rowSelector.DataContext is ModernDataRow dataRow && dataRow.DataContext is ICharacterRow row)
                {
                    var isChecked = row.IsChecked ?? false;
                    row.IsChecked = !isChecked;
                }
            }
        }
    }
}
