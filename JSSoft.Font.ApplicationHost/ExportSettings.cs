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
        private int textureWidth = 1024;
        private int textureHeight = 1024;
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
    }
}
