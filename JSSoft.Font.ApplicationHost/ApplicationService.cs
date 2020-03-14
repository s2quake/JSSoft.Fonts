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

using Ntreev.ModernUI.Framework;
using System.Windows;

namespace JSSoft.Font.ApplicationHost
{
    public static class ApplicationService
    {
        private const string CharacterNavigator = nameof(CharacterNavigator);
        private const string Shell = nameof(Shell);
        private const string UndoService = nameof(UndoService);
        private const string Configs = nameof(Configs);

        public static readonly DependencyProperty CharacterNavigatorProperty =
            DependencyProperty.RegisterAttached(CharacterNavigator, typeof(ICharacterNavigator), typeof(ApplicationService),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty ShellProperty =
            DependencyProperty.RegisterAttached(Shell, typeof(IShell), typeof(ApplicationService),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty UndoServiceProperty =
            DependencyProperty.RegisterAttached(UndoService, typeof(IUndoService), typeof(ApplicationService),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty ConfigsProperty =
            DependencyProperty.RegisterAttached(Configs, typeof(IAppConfiguration), typeof(ApplicationService),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        public static ICharacterNavigator GetCharacterNavigator(DependencyObject d)
        {
            return (ICharacterNavigator)d.GetValue(CharacterNavigatorProperty);
        }

        public static void SetCharacterNavigator(DependencyObject d, ICharacterNavigator value)
        {
            d.SetValue(CharacterNavigatorProperty, value);
        }

        public static IShell GetShell(DependencyObject d)
        {
            return (IShell)d.GetValue(ShellProperty);
        }

        public static void SetShell(DependencyObject d, IShell value)
        {
            d.SetValue(ShellProperty, value);
        }

        public static IUndoService GetUndoService(DependencyObject d)
        {
            return (IUndoService)d.GetValue(UndoServiceProperty);
        }

        public static void SetUndoService(DependencyObject d, IUndoService value)
        {
            d.SetValue(UndoServiceProperty, value);
        }

        public static IAppConfiguration GetConfigs(DependencyObject d)
        {
            return (IAppConfiguration)d.GetValue(ConfigsProperty);
        }

        public static void SetConfigs(DependencyObject d, IAppConfiguration value)
        {
            d.SetValue(ConfigsProperty, value);
        }
    }
}
