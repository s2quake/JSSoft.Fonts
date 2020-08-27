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

using JSSoft.Font.ApplicationHost.Serializations;
using JSSoft.ModernUI.Framework;

namespace JSSoft.Font.ApplicationHost
{
    public sealed class ExportSettings : PropertyChangedBase
    {
        private int textureWidth = 128;
        private int textureHeight = 128;
        private FontPadding paddingValue = new FontPadding(1);
        private FontSpacing spacingValue = new FontSpacing(1);

        public int TextureWidth
        {
            get => this.textureWidth;
            set
            {
                if (this.textureWidth != value)
                {
                    this.textureWidth = value;
                    this.NotifyOfPropertyChange(nameof(TextureWidth));
                }
            }
        }

        public int TextureHeight
        {
            get => this.textureHeight;
            set
            {
                if (this.textureHeight != value)
                {
                    this.textureHeight = value;
                    this.NotifyOfPropertyChange(nameof(TextureHeight));
                }
            }
        }

        public FontPadding PaddingValue
        {
            get => this.paddingValue;
            set
            {
                if (this.paddingValue != value)
                {
                    this.paddingValue = value;
                    this.NotifyOfPropertyChange(nameof(PaddingValue));
                }
            }
        }

        public FontSpacing SpacingValue
        {
            get => this.spacingValue;
            set
            {
                if (this.spacingValue != value)
                {
                    this.spacingValue = value;
                    this.NotifyOfPropertyChange(nameof(SpacingValue));
                }
            }
        }

        public void CopyTo(ExportSettings settings)
        {
            settings.TextureWidth = this.TextureWidth;
            settings.TextureHeight = this.TextureHeight;
            settings.PaddingValue = this.PaddingValue;
            settings.SpacingValue = this.SpacingValue;
        }

        public ExportSettings Clone()
        {
            var settings = new ExportSettings();
            this.CopyTo(settings);
            return settings;
        }

        internal FontDataSettings Convert(string name, uint[] characters)
        {
            return new FontDataSettings()
            {
                Name = name,
                Width = this.TextureWidth,
                Height = this.TextureHeight,
                Padding = new FontPadding((int)this.PaddingValue.Left, (int)this.PaddingValue.Top, (int)this.PaddingValue.Right, (int)this.PaddingValue.Bottom),
                Spacing = this.SpacingValue,
                Characters = characters
            };
        }

        internal void Update(ExportSettingsInfo info)
        {
            this.TextureWidth = info.TextureWidth;
            this.TextureHeight = info.TextureHeight;
            this.PaddingValue = new FontPadding(info.Padding.Left, info.Padding.Top, info.Padding.Right, info.Padding.Bottom);
            this.SpacingValue = new FontSpacing(info.Spacing.Horizontal, info.Spacing.Vertical);
        }
    }
}
