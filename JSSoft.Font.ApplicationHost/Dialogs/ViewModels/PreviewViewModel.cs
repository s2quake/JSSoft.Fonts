using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace JSSoft.Font.ApplicationHost.Dialogs.ViewModels
{
    class PreviewViewModel : ModalDialogBase
    {
        private readonly FontData fontData;
        private PreviewItemViewModel image;
        private Color backgroundColor = FromColor(FontPage.DefaultBackgroundColor);
        private Color foregroundColor = FromColor(FontPage.DefaultForegroundColor);
        private Color paddingColor = FromColor(FontPage.DefaultPaddingColor);

        public static Color FromColor(System.Drawing.Color color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static System.Drawing.Color FromColor(Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        public PreviewViewModel(FontData fontData)
        {
            //var imageList = new List<ImageSource>(data.Pages.Length);
            //for (var i = 0; i < data.Pages.Length; i++)
            //{
            //    var item = data.Pages[i];
            //    var stream = new MemoryStream();
            //    var bitmapImage = new BitmapImage();
            //    item.Save(stream);
            //    bitmapImage.BeginInit();
            //    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            //    bitmapImage.UriSource = null;
            //    bitmapImage.StreamSource = stream;
            //    bitmapImage.EndInit();
            //    bitmapImage.Freeze();
            //    imageList.Add(bitmapImage);
            //    stream.Dispose();
            //}
            //return imageList.ToArray();

            

            var itemList = new List<PreviewItemViewModel>(fontData.Pages.Length);
            for (var i = 0; i < fontData.Pages.Length; i++)
            {
                var page = fontData.Pages[i];
                itemList.Add(new PreviewItemViewModel(this, i, page));
            }
            this.fontData = fontData;
            this.Items = itemList.ToArray();
            this.image = itemList.First();
            this.DisplayName = "Preview";
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
    }
}
