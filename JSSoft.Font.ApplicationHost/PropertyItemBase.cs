using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font.ApplicationHost
{
    public abstract class PropertyItemBase : ViewModelBase, IPropertyItem
    {
        private string displayName;

        public abstract bool CanSupport(object obj);

        public abstract void SelectObject(object obj);

        public string DisplayName
        {
            get => this.displayName;
            set
            {
                this.displayName = value;
                this.NotifyOfPropertyChange(nameof(this.DisplayName));
            }
        }

        public abstract bool IsVisible { get; }

        public abstract object SelectedObject { get; }
    }
}
