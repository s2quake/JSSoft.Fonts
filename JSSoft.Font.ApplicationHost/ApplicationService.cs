using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JSSoft.Font.ApplicationHost
{
    public static class ApplicationService
    {
        private const string CharacterNavigator = nameof(CharacterNavigator);
        private const string Shell = nameof(Shell);

        public static readonly DependencyProperty CharacterNavigatorProperty =
            DependencyProperty.RegisterAttached(CharacterNavigator, typeof(ICharacterNavigator), typeof(ApplicationService),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty ShellProperty =
            DependencyProperty.RegisterAttached(Shell, typeof(IShell), typeof(ApplicationService),
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
    }
}
