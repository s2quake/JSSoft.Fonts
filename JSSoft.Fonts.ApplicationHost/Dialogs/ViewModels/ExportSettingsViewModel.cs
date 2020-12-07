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

using JSSoft.Fonts.ApplicationHost.Properties;
using JSSoft.ModernUI.Framework;
using JSSoft.ModernUI.Framework.Dialogs.ViewModels;
using System.Threading.Tasks;
using System.Windows.Media;

namespace JSSoft.Fonts.ApplicationHost.Dialogs.ViewModels
{
    class ExportSettingsViewModel : ModalDialogBase
    {
        private int textureWidth;
        private int textureHeight;
        private FontPadding paddingValue;
        private FontSpacing spacingValue;
        private Color backgroundColor = ColorUtility.FromColor(FontPage.DefaultBackgroundColor);
        private Color foregroundColor = ColorUtility.FromColor(FontPage.DefaultForegroundColor);
        private Color paddingColor = ColorUtility.FromColor(FontPage.DefaultPaddingColor);

        public ExportSettingsViewModel(ExportSettings settings)
        {
            this.textureWidth = settings.TextureWidth;
            this.textureHeight = settings.TextureHeight;
            this.paddingValue = settings.PaddingValue;
            this.spacingValue = settings.SpacingValue;
            this.DisplayName = Resources.Title_EditExportSettings;
        }

        public async Task OKAsync()
        {
            await this.TryCloseAsync(true);
        }

        public async Task SelectBackgroundColorAsync()
        {
            var dialog = new SelectColorViewModel()
            {
                CurrentColor = this.BackgroundColor
            };
            if (await dialog.ShowDialogAsync() == true)
            {
                this.BackgroundColor = dialog.CurrentColor;
            }
        }

        public async Task SelectForegroundColorAsync()
        {
            var dialog = new SelectColorViewModel()
            {
                CurrentColor = this.ForegroundColor
            };
            if (await dialog.ShowDialogAsync() == true)
            {
                this.ForegroundColor = dialog.CurrentColor;
            }
        }

        public async Task SelectPaddingColorAsync()
        {
            var dialog = new SelectColorViewModel()
            {
                CurrentColor = this.PaddingColor
            };
            if (await dialog.ShowDialogAsync() == true)
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

        public FontPadding PaddingValue
        {
            get => this.paddingValue;
            set
            {
                this.paddingValue = value;
                this.NotifyOfPropertyChange(nameof(PaddingValue));
            }
        }

        public FontSpacing SpacingValue
        {
            get => this.spacingValue;
            set
            {
                this.spacingValue = value;
                this.NotifyOfPropertyChange(nameof(SpacingValue));
            }
        }
    }
}
