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
        private Dictionary<uint, BitmapSource> imageByID = new Dictionary<uint, BitmapSource>();

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
            return this.dispatcher.InvokeAsync(() =>
            {
                this.lib = new Library();
                this.face = new Face(this.lib, path);
                this.face.SetCharSize(0, this.Height, 0, this.DPI);

                foreach (var (name, min, max) in NamesList.items)
                {
                    for (var i = min; i <= max; i++)
                    {
                        var index = face.GetCharIndex(i);
                        if (index != 0 && this.CreateBitmapSource(i) is BitmapSource bitmapSource)
                        {
                            this.imageByID.Add(i, bitmapSource);

                            //var filename = Path.Combine(Directory.GetCurrentDirectory(), "Bitmaps", $"{(uint)i}.png");
                            //using (var stream = new FileStream(filename, FileMode.Create))
                            //{
                            //    var encoder = new PngBitmapEncoder();
                            //    encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                            //    encoder.Save(stream);
                            //}
                        }
                    }
                }
            });
        }

        public bool Contains(uint id)
        {
            return this.imageByID.ContainsKey(id);
        }

        public BitmapSource GetBitmap(uint id)
        {
            return this.imageByID[id];
        }

        public uint DPI { get; set; } = 96;

        public int Height { get; set; } = 22;

        private BitmapSource CreateBitmapSource(uint id)
        {
            var ftbmp = this.CreateFreetypeBitmap(id);
            if (ftbmp == null || ftbmp.Rows == 0 || ftbmp.Width == 0)
                return null;

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

        private FTBitmap CreateFreetypeBitmap(uint id)
        {
            var index = this.face.GetCharIndex(id);
            if (index == 0)
                return null;
            this.face.LoadGlyph(index, LoadFlags.Default, LoadTarget.Normal);
            this.face.Glyph.RenderGlyph(RenderMode.Normal);
            return this.face.Glyph.Bitmap;
        }
    }
}
