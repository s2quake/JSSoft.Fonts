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
using Ntreev.ModernUI.Framework;
using System.Windows;

namespace JSSoft.Font.ApplicationHost
{
    public sealed class ExportSettings : PropertyChangedBase
    {
        private int textureWidth = 128;
        private int textureHeight = 128;
        private Thickness padding = new Thickness(1);
        private int horizontalSpace = 1;
        private int verticalSpace = 1;

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

        public Thickness Padding
        {
            get => this.padding;
            set
            {
                if (this.padding != value)
                {
                    this.padding = value;
                    this.NotifyOfPropertyChange(nameof(Padding));
                }
            }
        }

        public int HorizontalSpace
        {
            get => this.horizontalSpace;
            set
            {
                if (this.horizontalSpace != value)
                {
                    this.horizontalSpace = value;
                    this.NotifyOfPropertyChange(nameof(HorizontalSpace));
                }
            }
        }

        public int VerticalSpace
        {
            get => this.verticalSpace;
            set
            {
                if (this.verticalSpace != value)
                {
                    this.verticalSpace = value;
                    this.NotifyOfPropertyChange(nameof(VerticalSpace));
                }
            }
        }

        public void CopyTo(ExportSettings settings)
        {
            settings.TextureWidth = this.TextureWidth;
            settings.TextureHeight = this.TextureHeight;
            settings.Padding = this.Padding;
            settings.HorizontalSpace = this.HorizontalSpace;
            settings.VerticalSpace = this.VerticalSpace;
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
                Padding = new FontPadding((int)this.Padding.Left, (int)this.Padding.Top, (int)this.Padding.Right, (int)this.Padding.Bottom),
                Spacing = new FontSpacing(this.HorizontalSpace, this.VerticalSpace),
                Characters = characters
            };
        }

        internal void Update(ExportSettingsInfo info)
        {
            this.TextureWidth = info.TextureWidth;
            this.TextureHeight = info.TextureHeight;
            this.Padding = new Thickness(info.Padding.Left, info.Padding.Top, info.Padding.Right, info.Padding.Bottom);
            this.HorizontalSpace = info.Spacing.Horizontal;
            this.VerticalSpace = info.Spacing.Vertical;
        }
    }
}
