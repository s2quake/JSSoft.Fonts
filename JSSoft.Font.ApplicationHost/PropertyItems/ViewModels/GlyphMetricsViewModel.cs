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
using Ntreev.Library;
using System.ComponentModel.Composition;

namespace JSSoft.Font.ApplicationHost.PropertyItems.ViewModels
{
    [Export(typeof(IPropertyItem))]
    [Dependency(typeof(FontInfoViewModel))]
    class GlyphMetricsViewModel : PropertyItemBase
    {
        private ICharacter character;

        public GlyphMetricsViewModel()
        {
            this.DisplayName = Resources.Title_CharacterInfo;
        }

        public override bool CanSupport(object obj)
        {
            return obj is ICharacter;
        }

        public override void SelectObject(object obj)
        {
            if (obj is ICharacter character && character.IsEnabled == true)
            {
                this.GlyphMetrics = character.GlyphMetrics;
                this.character = character;
            }
            else
            {
                this.GlyphMetrics = GlyphMetrics.Empty;
                this.character = null;
            }
            this.NotifyOfPropertyChange(nameof(SelectedObject));
            this.NotifyOfPropertyChange(nameof(GlyphMetrics));
        }

        public GlyphMetrics GlyphMetrics { get; private set; }

        public override bool IsVisible => true;

        public override object SelectedObject => this.character;
    }
}
