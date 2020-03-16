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
using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace JSSoft.Font.ApplicationHost.Dialogs.ViewModels
{
    class PreviewViewModel : ModalDialogBase
    {
        private readonly IAppConfiguration configs;
        private readonly FontData fontData;
        private PreviewItemViewModel image;
        private Color backgroundColor = ColorUtility.FromColor(FontPage.DefaultBackgroundColor);
        private Color foregroundColor = ColorUtility.FromColor(FontPage.DefaultForegroundColor);
        private Color paddingColor = Colors.Blue;
        private double zoomLevel = 1.0;

        public PreviewViewModel(FontData fontData)
            : this(null, fontData)
        {

        }

        public PreviewViewModel(IAppConfiguration configs, FontData fontData)
        {
            this.configs = configs;
            this.fontData = fontData ?? throw new ArgumentNullException(nameof(fontData));
            this.DisplayName = Resources.Title_Preview;
            this.configs?.Update(this);

            var itemList = new List<PreviewItemViewModel>(fontData.Pages.Length);
            for (var i = 0; i < fontData.Pages.Length; i++)
            {
                var page = fontData.Pages[i];
                itemList.Add(new PreviewItemViewModel(this, i, page));
            }
            this.Items = itemList.ToArray();
            this.image = itemList.First();
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

        [ConfigurationProperty]
        public Color BackgroundColor
        {
            get => this.backgroundColor;
            set
            {
                this.backgroundColor = value;
                this.NotifyOfPropertyChange(nameof(BackgroundColor));
            }
        }

        [ConfigurationProperty]
        public Color ForegroundColor
        {
            get => this.foregroundColor;
            set
            {
                this.foregroundColor = value;
                this.NotifyOfPropertyChange(nameof(ForegroundColor));
            }
        }

        [ConfigurationProperty]
        public Color PaddingColor
        {
            get => this.paddingColor;
            set
            {
                this.paddingColor = value;
                this.NotifyOfPropertyChange(nameof(PaddingColor));
            }
        }

        public PreviewItemViewModel[] Items { get; }

        public PreviewItemViewModel Item
        {
            get => this.image;
            set
            {
                this.image = value;
                this.NotifyOfPropertyChange(nameof(Item));
            }
        }

        [ConfigurationProperty]
        public double ZoomLevel
        {
            get => this.zoomLevel;
            set
            {
                this.zoomLevel = value;
                this.NotifyOfPropertyChange(nameof(ZoomLevel));
                this.NotifyOfPropertyChange(nameof(ActualWidth));
                this.NotifyOfPropertyChange(nameof(ActualHeight));
            }
        }

        public double ActualWidth => this.fontData.PageWidth * this.zoomLevel;

        public double ActualHeight => this.fontData.PageHeight * this.zoomLevel;

        public Uri FontUri => this.fontData.BaseUri;

        public string FaceName => this.fontData.Name;

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            if (close == true)
            {
                this.configs?.Commit(this);
            }
        }
    }
}
