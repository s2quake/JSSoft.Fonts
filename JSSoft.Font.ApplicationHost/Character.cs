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
        private readonly CharacterContext context;
        private readonly FontGlyph glyph;
        private bool isChecked;
        private ImageSource source;
        private GlyphMetrics glyphMetrics;

        internal Character(uint id)
        {
            this.ID = id;
        }

        public Character(CharacterContext context, uint id)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.ID = id;
            this.IsEnabled = this.context.Glyphs.ContainsKey(id);
            if (this.context.Glyphs.ContainsKey(id))
            {
                this.glyph = this.context.Glyphs[id];
                this.glyphMetrics = this.context.Glyphs[id].Metrics;
            }
            else
            {
                this.glyphMetrics.VerticalAdvance = this.context.Height;
            }
            this.context.Register(this);
        }

        public override string ToString()
        {
            return $"{(char)this.ID}";
        }

        public uint ID { get; }

        public char Text => (char)this.ID;

        public bool IsEnabled { get; }

        public bool IsChecked
        {
            get => this.isChecked;
            set
            {
                if (this.isChecked != value)
                {
                    this.isChecked = value;
                    this.NotifyOfPropertyChange(nameof(IsChecked));
                }
            }
        }

        public ImageSource Source
        {
            get
            {
                lock (this)
                {
                    if (this.glyph != null && this.glyph.Bitmap != null)
                    {
                        var bitmap = this.glyph.Bitmap;
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
                    return this.source;
                }
            }
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

        public void SetChecked(bool value)
        {
            if (this.isChecked != value)
            {
                this.isChecked = value;
                this.NotifyOfPropertyChange(nameof(IsChecked));
            }
        }
    }
}
