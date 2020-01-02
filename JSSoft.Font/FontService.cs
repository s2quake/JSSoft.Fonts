using Ntreev.Library.Threading;
using SharpFont;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace JSSoft.Font
{
    [Export(typeof(IFontService))]
    class FontService : IFontService
    {
        private Library lib;
        private Face face;
        private Dispatcher dispatcher;
        private Dictionary<uint, BitmapSource> bitmapByID = new Dictionary<uint, BitmapSource>();
        private Dictionary<uint, GlyphMetrics> metricsByID = new Dictionary<uint, GlyphMetrics>();

        public FontService()
        {

        }

        public async Task CloseAsync()
        {
            await this.dispatcher.DisposeAsync();
            this.dispatcher = null;
        }

        public Task OpenAsync(string path)
        {
            this.dispatcher = new Dispatcher(this);
            return this.dispatcher.InvokeAsync((Action)(() =>
            {
                var pixelSize = (double)this.Height * this.DPI / 72;
                this.lib = new Library();
                this.face = new Face(this.lib, path);
                this.face.SetCharSize(0, this.Height, 0, this.DPI);

                this.VerticalAdvance = (int)Math.Round(this.face.Height * pixelSize / this.face.UnitsPerEM);

                var (min, max) = NamesList.Range;
                for (var i = min; i <= max; i++)
                {
                    this.RegisterItem(i);
                }
            }));
        }

        public uint DPI { get; set; } = 96;

        public int Height { get; set; } = 22;

        public int VerticalAdvance { get; set; }

        private void RegisterItem(uint charCode)
        {
            var glyph = this.CreateGlyph(charCode);
            if (glyph == null)
                return ;

            var ftbmp = glyph.Bitmap;
            var metrics = glyph.Metrics;
            var height = (double)Math.Round((double)glyph.LinearVerticalAdvance);
            var width = (double)Math.Round((double)glyph.LinearHorizontalAdvance);
            var baseLine = height + (height * glyph.Face.Descender / glyph.Face.Height);
            var glyphMetrics = new GlyphMetrics()
            {
                Width = (int)metrics.Width,
                Height = (int)metrics.Height,
                HorizontalBearingX = (int)metrics.HorizontalBearingX,
                HorizontalBearingY = (int)metrics.HorizontalBearingY,
                HorizontalAdvance = (int)metrics.HorizontalAdvance,
                VerticalBearingX = (int)metrics.VerticalBearingX,
                VerticalBearingY = (int)metrics.VerticalBearingY,
                VerticalAdvance = (int)metrics.VerticalAdvance,
                BaseLine = (int)Math.Round(baseLine),
            };
            var bitmapSource = this.CreateBitmapSource(ftbmp);
            this.metricsByID.Add(charCode, glyphMetrics);
            if (bitmapSource != null)
            {
                this.bitmapByID.Add(charCode, bitmapSource);
            }
        }

        private BitmapSource CreateBitmapSource(FTBitmap ftbmp)
        {
            if (ftbmp.Rows > 0 && ftbmp.Width > 0)
            {
                var bitmap = ftbmp.ToGdipBitmap(System.Drawing.Color.White);
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
                    return bitmapImage;
                }
            }
            return null;
        }

        private GlyphSlot CreateGlyph(uint charCode)
        {
            var index = this.face.GetCharIndex(charCode);
            if (index == 0)
                return null;
            this.face.LoadGlyph(index, LoadFlags.Default, LoadTarget.Normal);
            this.face.Glyph.RenderGlyph(RenderMode.Normal);
            return this.face.Glyph;
        }

        #region IFontService

        IReadOnlyDictionary<uint, BitmapSource> IFontService.Bitmaps => this.bitmapByID;

        IReadOnlyDictionary<uint, GlyphMetrics> IFontService.Metrics => this.metricsByID;

        #endregion
    }
}
