using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace JSSoft.Font.ApplicationHost.Dialogs.ViewModels
{
    class PreviewItemViewModel : PropertyChangedBase
    {
        private readonly PreviewViewModel parent;
        private readonly FontPage page;
        private ImageSource imageSource;

        public PreviewItemViewModel(PreviewViewModel parent, int index, FontPage page)
        {
            this.parent = parent;
            this.page = page;
            this.Index = index;
            this.parent.PropertyChanged += Parent_PropertyChanged;
            this.RefreshImage();
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
    }
}
