using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font.ApplicationHost
{
    public interface IPropertyService
    {
        IEnumerable<IPropertyItem> Properties { get; }

        object SelectedObject { get; set; }

        event EventHandler SelectionChanged;
    }
}
