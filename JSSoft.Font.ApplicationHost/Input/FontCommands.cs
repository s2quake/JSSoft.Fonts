using JSSoft.Font.ApplicationHost.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JSSoft.Font.ApplicationHost.Input
{
    public static class FontCommands
    {
        public static readonly RoutedUICommand NavigateBackward = new RoutedUICommand(Resources.NavigateBackward, nameof(NavigateBackward), typeof(FontCommands),
            new InputGestureCollection() { new KeyGesture(Key.Left, ModifierKeys.Alt) });

        public static readonly RoutedUICommand NavigateForward = new RoutedUICommand(Resources.NavigateForward, nameof(NavigateForward), typeof(FontCommands),
            new InputGestureCollection() { new KeyGesture(Key.Right, ModifierKeys.Alt) });
    }
}
