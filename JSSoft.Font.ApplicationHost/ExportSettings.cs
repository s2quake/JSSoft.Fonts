using JSSoft.Font.ApplicationHost.Serializations;
using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        internal FontDataSettings Convert(uint[] characters)
        {
            return new FontDataSettings()
            {
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
