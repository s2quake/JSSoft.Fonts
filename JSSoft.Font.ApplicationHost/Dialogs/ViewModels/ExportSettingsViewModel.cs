using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace JSSoft.Font.ApplicationHost.Dialogs.ViewModels
{
    class ExportSettingsViewModel : ModalDialogBase
    {
        private int textureWidth = 1024;
        private int textureHeight = 1024;
        private Thickness padding;
        private int horizontalSpace = 1;
        private int verticalSpace = 1;
        private Color backgroundColor = ColorUtility.FromColor(FontPage.DefaultBackgroundColor);
        private Color foregroundColor = ColorUtility.FromColor(FontPage.DefaultForegroundColor);
        private Color paddingColor = ColorUtility.FromColor(FontPage.DefaultPaddingColor);

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

        public void SelectBackgroundColor()
        {
            var dialog = new SelectColorViewModel()
            {
                CurrentColor = this.BackgroundColor
            };
            if (dialog.ShowDialog() == true)
            {
                this.BackgroundColor = dialog.CurrentColor;
            }
        }

        public void SelectForegroundColor()
        {
            var dialog = new SelectColorViewModel()
            {
                CurrentColor = this.ForegroundColor
            };
            if (dialog.ShowDialog() == true)
            {
                this.ForegroundColor = dialog.CurrentColor;
            }
        }

        public void SelectPaddingColor()
        {
            var dialog = new SelectColorViewModel()
            {
                CurrentColor = this.PaddingColor
            };
            if (dialog.ShowDialog() == true)
            {
                this.PaddingColor = dialog.CurrentColor;
            }
        }

        public Color BackgroundColor
        {
            get => this.backgroundColor;
            set
            {
                this.backgroundColor = value;
                this.NotifyOfPropertyChange(nameof(BackgroundColor));
            }
        }

        public Color ForegroundColor
        {
            get => this.foregroundColor;
            set
            {
                this.foregroundColor = value;
                this.NotifyOfPropertyChange(nameof(ForegroundColor));
            }
        }

        public Color PaddingColor
        {
            get => this.paddingColor;
            set
            {
                this.paddingColor = value;
                this.NotifyOfPropertyChange(nameof(PaddingColor));
            }
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
