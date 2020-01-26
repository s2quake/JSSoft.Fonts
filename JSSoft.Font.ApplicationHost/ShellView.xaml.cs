using FirstFloor.ModernUI.Windows.Controls;
using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace JSSoft.Font.ApplicationHost
{
    /// <summary>
    /// ShellView.xaml에 대한 상호 작용 논리
    /// </summary>
    [Export(typeof(ShellView))]
    public partial class ShellView : ModernWindow, IPartImportsSatisfiedNotification
    {
        private readonly Lazy<IShell> shell = null;
        private readonly IEnumerable<IMenuItem> menuItems;

        public ShellView()
        {
            InitializeComponent();
        }

        [ImportingConstructor]
        public ShellView(Lazy<IShell> shell, [ImportMany]IEnumerable<IMenuItem> menuItems)
        {
            this.shell = shell;
            this.menuItems = menuItems.ToArray();
            this.InitializeComponent();
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
        }

        private void SetInputBindings(IMenuItem menuItem)
        {
            if (menuItem.InputGesture != null)
            {
                if (menuItem.IsVisible == true)
                    this.InputBindings.Add(new InputBinding(menuItem.Command, menuItem.InputGesture));

                if (menuItem is INotifyPropertyChanged notifyObject)
                {
                    notifyObject.PropertyChanged += (s, e) =>
                    {
                        if (menuItem.IsVisible == true)
                        {
                            this.InputBindings.Add(new InputBinding(menuItem.Command, menuItem.InputGesture));
                        }
                        else
                        {
                            for (var i = 0; i < this.InputBindings.Count; i++)
                            {
                                if (this.InputBindings[i].Command == menuItem)
                                {
                                    this.InputBindings.RemoveAt(i);
                                    break;
                                }
                            }
                        }
                    };
                }
            }

            foreach (var item in menuItem.ItemsSource)
            {
                this.SetInputBindings(item);
            }
        }

        private IShell Shell => this.shell.Value;

        #region IPartImportsSatisfiedNotification

        void IPartImportsSatisfiedNotification.OnImportsSatisfied()
        {
            var items = MenuItemUtility.GetMenuItems<IMenuItem>(this.Shell, this.menuItems);
            foreach (var item in items)
            {
                this.SetInputBindings(item);
            }
        }

        #endregion
    }
}
