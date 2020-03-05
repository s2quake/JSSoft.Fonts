using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font.ApplicationHost
{
    public interface IPropertyItem
    {
        bool CanSupport(object obj);

        void SelectObject(object obj);

        string DisplayName { get; }

        object SelectedObject { get; }

        bool IsVisible { get; }
    }
}
