using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace JSSoft.Font.ApplicationHost
{
    class Character : PropertyChangedBase, ICharacter
    {
        private readonly FontDescriptor fontDescriptor;
        private bool isEnabled;
        private bool isChecked;
        private ImageSource source;
        private GlyphMetrics glyphMetrics;

        internal Character(uint id)
        {
            this.ID = id;
        }

        public Character(CharacterRow row, FontDescriptor fontDescriptor, uint id)
        {
            this.Row = row ?? throw new ArgumentNullException(nameof(row));
            this.fontDescriptor = fontDescriptor ?? throw new ArgumentNullException(nameof(fontDescriptor));
            this.ID = id;
            this.isEnabled = this.fontDescriptor.Metrics.ContainsKey(id);
            if (this.fontDescriptor.Metrics.ContainsKey(id))
            {
                this.glyphMetrics = this.fontDescriptor.Metrics[id];
            }
            else
            {
                this.glyphMetrics.VerticalAdvance = this.fontDescriptor.ItemHeight;
            }
            if (this.fontDescriptor.Bitmaps.ContainsKey(id))
            {
                var bitmap = this.fontDescriptor.Bitmaps[id];
                using (var stream = new MemoryStream())
                {
                    var bitmapImage = new BitmapImage();
                    bitmap.Save(stream, ImageFormat.Png);
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.UriSource = null;
                    bitmapImage.StreamSource = stream;
                    bitmapImage.EndInit();
                    bitmapImage.Freeze();
                    this.source = bitmapImage;
                }
            }
            this.Row.PropertyChanged += Row_PropertyChanged;
        }

        public override string ToString()
        {
            return $"{(char)this.ID}";
        }

        public CharacterRow Row { get; }

        public uint ID { get; }

        public char Text => (char)this.ID;

        public bool IsEnabled
        {
            get => this.isEnabled;
            set
            {
                this.isEnabled = value;
                this.NotifyOfPropertyChange(nameof(IsEnabled));
            }
        }

        public bool IsChecked
        {
            get
            {
                if (this.Row != null && this.Row.IsChecked != null)
                    return this.Row.IsChecked.Value;
                return this.isChecked;
            }
            set
            {
                this.isChecked = value;
                this.NotifyOfPropertyChange(nameof(IsChecked));
            }
        }

        public ImageSource Source
        {
            get => this.source;
            set
            {
                this.source = value;
                this.NotifyOfPropertyChange(nameof(Source));
            }
        }

        public GlyphMetrics GlyphMetrics
        {
            get => this.glyphMetrics;
            set
            {
                this.glyphMetrics = value;
                this.NotifyOfPropertyChange(nameof(GlyphMetrics));
            }
        }

        private void Row_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CharacterRow.IsChecked))
            {
                //if (object.Equals(this.Row.IsChecked, this.isChecked) == false)
                {
                    this.NotifyOfPropertyChange(nameof(IsChecked));
                }
            }
        }
    }
}
