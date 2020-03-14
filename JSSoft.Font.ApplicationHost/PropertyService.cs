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

using Ntreev.Library.Linq;
using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Threading;

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
            }, DispatcherPriority.ApplicationIdle);
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
