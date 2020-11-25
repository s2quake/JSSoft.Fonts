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

using JSSoft.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace JSSoft.Font.ApplicationHost.Dialogs.ViewModels
{
    class PreviewItemViewModel : PropertyChangedBase
    {
        private readonly PreviewViewModel parent;
        private readonly FontPage page;
        private readonly ObservableCollection<GlyphItemViewModel> glyphList = new ObservableCollection<GlyphItemViewModel>();
        private GlyphItemViewModel glyph;
        private ImageSource imageSource;

        public PreviewItemViewModel(PreviewViewModel parent, int index, FontPage page)
        {
            this.parent = parent;
            this.page = page;
            this.Index = index;
            this.ClickCommand = new DelegateCommand(this.Click, this.CanClick);
            this.parent.PropertyChanged += Parent_PropertyChanged;
            var query = page.Glyphs.OrderBy(item => item.ID);
            foreach (var item in query)
            {
                this.glyphList.Add(new GlyphItemViewModel(item));
            }
            this.RefreshImage();
        }

        public void Click(Point point)
        {
            var x = (int)(point.X / this.parent.ZoomLevel);
            var y = (int)(point.Y / this.parent.ZoomLevel);
            var glyphData = this.page.HitTest(new System.Drawing.Point(x, y));
            if (glyphData != null)
            {
                var viewModel = this.glyphList.FirstOrDefault(item => item.GlyphData == glyphData);
                if (viewModel != null)
                {
                    this.Glyph = viewModel;
                }
            }
        }

        public int Index { get; }

        public ImageSource ImageSource
        {
            get => this.imageSource;
            private set
            {
                this.imageSource = value;
                this.NotifyOfPropertyChange(nameof(ImageSource));
            }
        }

        public double Width => (double)this.page.Width;

        public double Height => (double)this.page.Height;

        public double ActualWidth => (double)this.page.Width * this.parent.ZoomLevel;

        public double ActualHeight => (double)this.page.Height * this.parent.ZoomLevel;

        public IEnumerable<GlyphItemViewModel> Glyphs => this.glyphList;

        public GlyphItemViewModel Glyph
        {
            get => this.glyph;
            set
            {
                this.glyph = value;
                this.NotifyOfPropertyChange(nameof(Glyph));
                this.NotifyOfPropertyChange(nameof(GlyphMargin));
                this.NotifyOfPropertyChange(nameof(ActualGlyphMargin));
            }
        }

        public Thickness GlyphMargin
        {
            get
            {
                if (this.glyph != null)
                {
                    var rect = this.glyph.Rectangle;
                    var l = rect.Left;
                    var t = rect.Top;
                    var r = this.Width - rect.Right;
                    var b = this.Height - rect.Bottom;
                    return new Thickness(l, t, r, b);
                }
                return new Thickness(0);
            }
        }

        public Thickness ActualGlyphMargin
        {
            get
            {
                if (this.glyph != null)
                {
                    var rect = this.glyph.Rectangle;
                    var l = rect.Left;
                    var t = rect.Top;
                    var r = this.Width - rect.Right;
                    var b = this.Height - rect.Bottom;
                    return new Thickness(l * this.parent.ZoomLevel, t * this.parent.ZoomLevel, r * this.parent.ZoomLevel, b * this.parent.ZoomLevel);
                }
                return new Thickness(0);
            }
        }

        public ICommand ClickCommand { get; }

        public async void RefreshImage()
        {
            var image = await Task.Run(() =>
            {
                var stream = new MemoryStream();
                this.page.BackgroundColor = ColorUtility.FromColor(this.parent.BackgroundColor);
                this.page.ForegroundColor = ColorUtility.FromColor(this.parent.ForegroundColor);
                this.page.PaddingColor = ColorUtility.FromColor(this.parent.PaddingColor);
                this.page.Save(stream);
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.UriSource = null;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                stream.Dispose();
                return bitmapImage;
            });
            await this.Dispatcher.InvokeAsync(() =>
            {
                this.ImageSource = image;
            });
        }

        private bool CanClick(object parameter)
        {
            return parameter is Point;
        }

        private void Click(object parameter)
        {
            if (parameter is Point point)
            {
                this.Click(point);
            }
            else
            {
                throw new ArgumentException($"{nameof(parameter)} must be a point");
            }
        }

        private void Parent_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(PreviewViewModel.BackgroundColor):
                case nameof(PreviewViewModel.ForegroundColor):
                case nameof(PreviewViewModel.PaddingColor):
                    {
                        this.RefreshImage();
                    }
                    break;
                case nameof(PreviewViewModel.ActualWidth):
                case nameof(PreviewViewModel.ActualHeight):
                    {
                        this.NotifyOfPropertyChange(e.PropertyName);
                        this.NotifyOfPropertyChange(nameof(ActualGlyphMargin));
                    }
                    break;
            }
        }
    }
}
