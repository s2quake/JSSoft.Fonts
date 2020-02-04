using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JSSoft.Font.ApplicationHost
{
    public class ExportSettings : PropertyChangedBase
    {
        private int textureWidth = 1024;
        private int textureHeight = 1024;
        private Thickness padding = new Thickness(1);

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
    }
}
