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
        private BitmapSource source;
        private GlyphMetrics glyphMetrics;

        internal Character(uint id)
        {
            this.ID = id;
        }

        public Character(CharacterRow row, CharacterContext context, uint id)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.Row = row;
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
            if (this.Row != null && this.Row.Group is CharacterGroup group)
            {
                return $"{group.Name}: '{this.Text}'";
            }
            return $"{(char)this.ID}";
        }

        public uint ID { get; }

        public string Text
        {
            get
            {
                switch (this.ID)
                {
                    case 0:
                        return "\\0";
                    case 7:
                        return "\\a";
                    case 8:
                        return "\\b";
                    case 12:
                        return "\\f";
                    case 10:
                        return "\\n";
                    case 13:
                        return "\\r";
                    case 9:
                        return "\\t";
                    case 11:
                        return "\\v";
                    default:
                        return $"{(char)this.ID}";
                }
            }
        }

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

        public BitmapSource Source
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

        public CharacterRow Row { get; }

        public IEnumerable<IMenuItem> MenuItems => MenuItemUtility.GetMenuItems(this, AppBootstrapperBase.Current);

        #region ICharacter

        ICharacterRow ICharacter.Row => this.Row;

        #endregion
    }
}
