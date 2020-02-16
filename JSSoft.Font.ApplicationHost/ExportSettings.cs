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
                this.textureWidth = value;
                this.NotifyOfPropertyChange(nameof(TextureWidth));
            }
        }

        public int TextureHeight
        {
            get => this.textureHeight;
            set
            {
                this.textureHeight = value;
                this.NotifyOfPropertyChange(nameof(TextureHeight));
            }
        }

        public Thickness Padding
        {
            get => this.padding;
            set
            {
                this.padding = value;
                this.NotifyOfPropertyChange(nameof(Padding));
            }
        }

        public int HorizontalSpace
        {
            get => this.horizontalSpace;
            set
            {
                this.horizontalSpace = value;
                this.NotifyOfPropertyChange(nameof(HorizontalSpace));
            }
        }

        public int VerticalSpace
        {
            get => this.verticalSpace;
            set
            {
                this.verticalSpace = value;
                this.NotifyOfPropertyChange(nameof(VerticalSpace));
            }
        }

        public static explicit operator FontDataSettings(ExportSettings settings)
        {
            return new FontDataSettings()
            {
                Width = settings.TextureWidth,
                Height = settings.TextureHeight,
                Padding = ((int)settings.Padding.Left, (int)settings.Padding.Top, (int)settings.Padding.Right, (int)settings.Padding.Bottom),
                Spacing = (settings.HorizontalSpace, settings.VerticalSpace)
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
