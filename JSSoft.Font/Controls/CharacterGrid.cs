using Ntreev.ModernUI.Framework.DataGrid.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JSSoft.Font.Controls
{
    class CharacterGrid : ModernDataGridControl
    {
        protected override Size MeasureOverride(Size constraint)
        {
            return base.MeasureOverride(constraint);
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            foreach (var item in this.Columns)
            {
                //item.Width = 25;
            }
            return base.ArrangeOverride(arrangeBounds);
        }
    }
}
