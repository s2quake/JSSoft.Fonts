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

using Caliburn.Micro;
using JSSoft.Font.ApplicationHost.Controls;
using Ntreev.ModernUI.Framework;
using System.Windows;

namespace JSSoft.Font.ApplicationHost
{
    class AppBootstrapper : AppBootstrapperBase
    {
        static AppBootstrapper()
        {
            ConventionManager.AddElementConvention<PaddingControl>(PaddingControl.ValueProperty, nameof(PaddingControl.Value), nameof(PaddingControl.ValueChanged));
            ConventionManager.AddElementConvention<SpacingControl>(SpacingControl.ValueProperty, nameof(SpacingControl.Value), nameof(SpacingControl.ValueChanged));
            ConventionManager.AddElementConvention<ValueSelector>(ValueSelector.ValueProperty, nameof(ValueSelector.Value), nameof(ValueSelector.ValueChanged));
            ConventionManager.AddElementConvention<CharacterControl>(CharacterControl.ValueProperty, nameof(CharacterControl.Value), nameof(FrameworkElement.DataContextChanged));
        }

        public AppBootstrapper()
            : base(typeof(IShell))
        {

        }
    }
}
