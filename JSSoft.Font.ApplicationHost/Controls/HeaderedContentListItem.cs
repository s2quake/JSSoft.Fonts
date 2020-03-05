using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace JSSoft.Font.ApplicationHost.Controls
{
    public class HeaderedContentListItem : ListBoxItem
    {
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(nameof(Header), typeof(object), typeof(HeaderedContentListItem),
                new FrameworkPropertyMetadata(null, HeaderPropertyChangedCallback));

        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.Register(nameof(HeaderTemplate), typeof(DataTemplate), typeof(HeaderedContentListItem),
                new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty HeaderTemplateSelectorProperty =
            DependencyProperty.Register(nameof(HeaderTemplateSelector), typeof(DataTemplateSelector), typeof(HeaderedContentListItem),
                new FrameworkPropertyMetadata(null));

        public object Header
        {
            get => (object)this.GetValue(HeaderProperty);
            set => this.SetValue(HeaderProperty, value);
        }

        public DataTemplate HeaderTemplate
        {
            get => (DataTemplate)this.GetValue(HeaderTemplateProperty);
            set => this.SetValue(HeaderTemplateProperty, value);
        }

        public DataTemplateSelector HeaderTemplateSelector
        {
            get => (DataTemplateSelector)this.GetValue(HeaderTemplateSelectorProperty);
            set => this.SetValue(HeaderTemplateSelectorProperty, value);
        }

        private static void HeaderPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement fe && fe.Parent is FrameworkElement parent)
            {
                parent.InvalidateMeasure();
            }
        }
    }
}
