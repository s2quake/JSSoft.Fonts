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

using JSSoft.Font.ApplicationHost.Properties;
using System.Windows.Input;

namespace JSSoft.Font.ApplicationHost.Input
{
    public static class FontCommands
    {
        public static readonly RoutedUICommand NavigateBackward = 
            new RoutedUICommand(Resources.Command_NavigateBackward, nameof(NavigateBackward), typeof(FontCommands),
                new InputGestureCollection() { new KeyGesture(Key.Left, ModifierKeys.Alt) });

        public static readonly RoutedUICommand NavigateForward = 
            new RoutedUICommand(Resources.Command_NavigateForward, nameof(NavigateForward), typeof(FontCommands),
                new InputGestureCollection() { new KeyGesture(Key.Right, ModifierKeys.Alt) });

        public static readonly RoutedUICommand ShowPropertyWindow =
            new RoutedUICommand(nameof(ShowPropertyWindow), nameof(ShowPropertyWindow), typeof(FontCommands));

        public static readonly RoutedUICommand HidePropertyWindow =
            new RoutedUICommand(nameof(HidePropertyWindow), nameof(HidePropertyWindow), typeof(FontCommands));

        public static readonly RoutedUICommand Undo =
            new RoutedUICommand(ApplicationCommands.Undo.Text, nameof(Undo), typeof(FontCommands),
                new InputGestureCollection() { new KeyGesture(Key.Z, ModifierKeys.Control) });

        public static readonly RoutedUICommand Redo =
            new RoutedUICommand(ApplicationCommands.Redo.Text, nameof(Redo), typeof(FontCommands),
                new InputGestureCollection() { new KeyGesture(Key.Y, ModifierKeys.Control) });
    }
}
