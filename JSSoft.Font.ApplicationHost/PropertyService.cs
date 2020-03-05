using Ntreev.Library.Linq;
using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font.ApplicationHost
{
    [Export]
    [Export(typeof(IPropertyService))]
    class PropertyService : PropertyChangedBase, IPropertyService
    {
        private IPropertyItem[] propertyItems;

        private object selectedObject;

        [ImportingConstructor]
        public PropertyService([ImportMany]IEnumerable<Lazy<IPropertyItem>> propertyItems)
        {
            this.Dispatcher.InvokeAsync(() =>
            {
                this.propertyItems = EnumerableUtility.TopologicalSort(propertyItems.Select(item => item.Value)).ToArray();
                this.NotifyOfPropertyChange(nameof(Properties));
            }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
        }

        public object SelectedObject
        {
            get => this.selectedObject;
            set
            {
                if (this.selectedObject == value)
                    return;
                this.selectedObject = value;
                this.RefreshItems();
                this.OnSelectionChanged(EventArgs.Empty);
            }
        }

        public void RefreshItems()
        {
            foreach (var item in this.propertyItems)
            {
                if (item.CanSupport(this.selectedObject) == true)
                {
                    item.SelectObject(this.selectedObject);
                }
                else
                {
                    item.SelectObject(null);
                }
            }
        }

        public IEnumerable<IPropertyItem> Properties => this.propertyItems;

        public event EventHandler SelectionChanged;

        protected virtual void OnSelectionChanged(EventArgs e)
        {
            this.SelectionChanged?.Invoke(this, e);
        }
    }
}
