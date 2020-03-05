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
    [Export]
    public partial class ShellView : ModernWindow
    {
        public static readonly RoutedCommand ShowPropertyWindow =
            new RoutedUICommand(nameof(ShowPropertyWindow), nameof(ShowPropertyWindow), typeof(ShellView));

        public static readonly RoutedCommand HidePropertyWindow =
            new RoutedUICommand(nameof(HidePropertyWindow), nameof(HidePropertyWindow), typeof(ShellView));

        [Import]
        private IAppConfiguration configs = null;

        public ShellView()
        {
            InitializeComponent();
            this.CommandBindings.Add(new CommandBinding(ShowPropertyWindow, ShowPropertyWindow_Execute, ShowPropertyWindow_CanExecute));
            this.CommandBindings.Add(new CommandBinding(HidePropertyWindow, HidePropertyWindow_Execute, HidePropertyWindow_CanExecute));
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
        }

        private void Expander_Loaded(object sender, RoutedEventArgs e)
        {
            var expander = sender as Expander;
            if (expander.DataContext == null)
                return;

            if (this.configs.TryGetValue<bool>(this.GetType(), expander.DataContext.GetType(), nameof(expander.IsExpanded), out var isExpanded) == true)
            {
                expander.IsExpanded = isExpanded;
            }
        }

        private void Expander_Unloaded(object sender, RoutedEventArgs e)
        {
            var expander = sender as Expander;
            if (expander.DataContext == null)
                return;

            this.configs.SetValue(this.GetType(), expander.DataContext.GetType(), nameof(expander.IsExpanded), expander.IsExpanded);
        }

        private void ShowPropertyWindow_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            this.PropertyWindow.Visibility = Visibility.Visible;
            e.Handled = true;
            this.Focus();
        }

        private void ShowPropertyWindow_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.PropertyWindow.Visibility != Visibility.Visible;
            e.Handled = true;
        }

        private void HidePropertyWindow_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            this.PropertyWindow.Visibility = Visibility.Collapsed;
            e.Handled = true;
            this.Focus();
        }

        private void HidePropertyWindow_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.PropertyWindow.Visibility == Visibility.Visible;
            e.Handled = true;
        }
    }
}
