using Ntreev.ModernUI.Framework;
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
        private const string UndoService = nameof(UndoService);

        public static readonly DependencyProperty CharacterNavigatorProperty =
            DependencyProperty.RegisterAttached(CharacterNavigator, typeof(ICharacterNavigator), typeof(ApplicationService),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty ShellProperty =
            DependencyProperty.RegisterAttached(Shell, typeof(IShell), typeof(ApplicationService),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty UndoServiceProperty =
            DependencyProperty.RegisterAttached(UndoService, typeof(IUndoService), typeof(ApplicationService),
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
    }
}
