using FirstFloor.ModernUI.Windows.Controls;
using JSSoft.Font.ApplicationHost.Input;
using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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

        private readonly IAppConfiguration configs;
        private readonly ICharacterNavigator navigator;
        //private readonly IShell shell;
        //private readonly Stack<ICharacter> backwards = new Stack<ICharacter>();
        //private readonly Stack<ICharacter> forwards = new Stack<ICharacter>();
        private GridLength propertyWidth;
        private double propertyMinWidth;
        //private ICharacter currentCharacter;

        public ShellView()
        {
            InitializeComponent();
        }

        [ImportingConstructor]
        public ShellView(IAppConfiguration configs, ICharacterNavigator navigator)
        {
            this.configs = configs;
            this.navigator = navigator;
            //this.shell = shell;
            //this.shell.Opened += Shell_Opened;
            //this.shell.Closed += Shell_Closed;
            InitializeComponent();
            this.CommandBindings.Add(new CommandBinding(ShowPropertyWindow, ShowPropertyWindow_Execute, ShowPropertyWindow_CanExecute));
            this.CommandBindings.Add(new CommandBinding(HidePropertyWindow, HidePropertyWindow_Execute, HidePropertyWindow_CanExecute));
            this.CommandBindings.Add(new CommandBinding(FontCommands.NavigateBackward, NavigateBackward_Execute, NavigateBackward_CanExecute));
            this.CommandBindings.Add(new CommandBinding(FontCommands.NavigateForward, NavigateForward_Execute, NavigateForward_CanExecute));
        }

        public ICharacterNavigator Navigator => this.navigator;

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
            this.PropertyWindowColumn.Width = this.propertyWidth;
            this.PropertyWindowColumn.MinWidth = this.propertyMinWidth;
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
            this.propertyWidth = this.PropertyWindowColumn.Width;
            this.propertyMinWidth = this.PropertyWindowColumn.MinWidth;
            this.PropertyWindow.Visibility = Visibility.Collapsed;
            this.PropertyWindowColumn.Width = new GridLength(0);
            this.PropertyWindowColumn.MinWidth = 0;
            e.Handled = true;
            this.Focus();
        }

        private void HidePropertyWindow_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.PropertyWindow.Visibility == Visibility.Visible;
            e.Handled = true;
        }

        //private void Shell_Closed(object sender, EventArgs e)
        //{
        //    this.shell.SelectedcharacterChanged -= Shell_SelectedcharacterChanged;
        //    this.backwards.Clear();
        //    this.forwards.Clear();
        //    this.currentCharacter = null;
        //    CommandManager.InvalidateRequerySuggested();
        //}

        //private void Shell_Opened(object sender, EventArgs e)
        //{
        //    this.shell.SelectedcharacterChanged += Shell_SelectedcharacterChanged;
        //}

        //private void Shell_SelectedcharacterChanged(object sender, EventArgs e)
        //{
        //    var character = this.shell.SelectedCharacter;
        //    if (character != null)
        //    {
        //        if (this.currentCharacter != null && this.currentCharacter != character)
        //        {
        //            this.backwards.Push(this.currentCharacter);
        //        }
        //        this.currentCharacter = character;
        //        CommandManager.InvalidateRequerySuggested();
        //    }
        //}

        private void NavigateBackward_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            //if (this.shell.SelectedCharacter != null)
            //{
            //    this.forwards.Push(this.shell.SelectedCharacter);
            //}
            //var character = this.backwards.Pop();
            //this.shell.SelectedcharacterChanged -= Shell_SelectedcharacterChanged;
            //this.shell.SelectedCharacter = character;
            //this.shell.SelectedcharacterChanged += Shell_SelectedcharacterChanged;
            this.navigator.Backward();
        }

        private void NavigateBackward_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            //e.CanExecute = this.backwards.Any();
            e.CanExecute = this.navigator.CanBackward;
            e.Handled = true;
        }

        private void NavigateForward_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            //if (this.shell.SelectedCharacter != null)
            //{
            //    this.backwards.Push(this.shell.SelectedCharacter);
            //}
            //var character = this.forwards.Pop();
            //this.shell.SelectedcharacterChanged -= Shell_SelectedcharacterChanged;
            //this.shell.SelectedCharacter = character;
            //this.shell.SelectedcharacterChanged += Shell_SelectedcharacterChanged;
            this.navigator.Forward();
        }

        private void NavigateForward_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            //e.CanExecute = this.forwards.Any();
            e.CanExecute = this.navigator.CanForward;
            e.Handled = true;
        }

        private async void Item_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem)
            {
                if (menuItem.Tag is Popup popup)
                {
                    popup.IsOpen = false;
                }
                if (menuItem.DataContext is ICharacterNavigatorItem item)
                {
                    await this.Dispatcher.InvokeAsync(() => this.navigator.Current = item, System.Windows.Threading.DispatcherPriority.Background);
                }
            }
        }
    }
}
