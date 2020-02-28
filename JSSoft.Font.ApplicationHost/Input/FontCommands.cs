using JSSoft.Font.ApplicationHost.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace JSSoft.Font.ApplicationHost.Input
{
    public static class FontCommands
    {
        public static RoutedUICommand OpenFont { get; } = new RoutedUICommand(nameof(OpenFont), nameof(OpenFont), typeof(FontCommands),
            new InputGestureCollection() { new KeyGesture(Key.O, ModifierKeys.Control) });

        static FontCommands()
        {
            CommandManager.RegisterClassCommandBinding(typeof(Window), new CommandBinding(OpenFont, OpenFont_Executed, OpenFont_CanExecute));
        }

        private static void OpenFont_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter is IServiceProvider serviceProvider && serviceProvider.GetService(typeof(IShell)) is IShell shell)
            {
                OpenFontCommand.Execute(shell);
                e.Handled = true;
            }
        }

        private static void OpenFont_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (e.Parameter is IServiceProvider serviceProvider && serviceProvider.GetService(typeof(IShell)) is IShell shell)
            {
                e.CanExecute = OpenFontCommand.CanExecute(shell);
                e.Handled = true;
            }
        }
    }
}
