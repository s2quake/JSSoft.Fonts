using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JSSoft.Font.ApplicationHost.Dialogs.ViewModels
{
    class ExportSettingsViewModel : ModalDialogBase
    {
        private int textureWidth = 1024;
        private int textureHeight = 1024;
        private Thickness padding;
        private int horizontalSpace = 1;
        private int verticalSpace = 1;

        public ExportSettingsViewModel(ExportSettings settings)
        {
            this.textureWidth = settings.TextureWidth;
            this.textureHeight = settings.TextureHeight;
            this.padding = settings.Padding;
            this.horizontalSpace = settings.HorizontalSpace;
            this.verticalSpace = settings.VerticalSpace;
            this.DisplayName = "Export Settings";
        }

        public void OK()
        {
            this.TryClose(true);
        }

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
