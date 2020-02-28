using Ntreev.Library.Threading;
using SharpFont;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JSSoft.Font
{
    public sealed class FontDescriptor : IDisposable
    {
        private readonly Dictionary<uint, FontGlyph> glyphByID = new Dictionary<uint, FontGlyph>();
        private Library lib;
        private Face face;

        public FontDescriptor(string path, uint dpi, int size)
            : this(path, dpi, size, 0)
        {

        }

        public FontDescriptor(string path, uint dpi, int size, int faceIndex)
        {
            var pixelSize = (double)size * dpi / 72;
            this.lib = new Library();
            this.face = new Face(this.lib, Path.GetFullPath(path), faceIndex);
            this.face.SetCharSize(0, size, 0, dpi);
            this.Height = (int)Math.Round(this.face.Height * pixelSize / this.face.UnitsPerEM);
            this.BaseLine = this.Height + (this.Height * this.face.Descender / this.face.Height);
            var (min, max) = NamesList.Range;
            for (var i = min; i <= max; i++)
            {
                this.RegisterItem(i);
            }
            this.Name = this.face.FamilyName;
            this.FaceIndex = faceIndex;
            this.DPI = dpi;
            this.Size = size;
            this.FontPath = path;
        }

        public static string[] GetFaces(string path)
        {
            var fullpath = Path.GetFullPath(path);
            using (var lib = new Library())
            using (var face = new Face(lib, fullpath, 0))
            {
                var faceList = new List<string>()
                {
                    face.FamilyName
                };
                for (var i = 1; i < face.FaceCount; i++)
                {
                    using (var childFace = new Face(lib, fullpath, i))
                    {
                        faceList.Add(childFace.FamilyName);
                    }
                }
                return faceList.ToArray();
            }
        }

        public FontData CreateData(FontDataSettings settings)
        {
            return FontData.Create(this, settings);
        }

        public uint DPI { get; private set; }

        public int Size { get; private set; }

        public int Height { get; private set; }

        public int BaseLine { get; private set; }

        public string Name { get; private set; } = string.Empty;

        public int FaceIndex { get; private set; }

        public string FontPath { get; private set; }

        public IReadOnlyDictionary<uint, FontGlyph> Glyphs => this.glyphByID;

        private void RegisterItem(uint charCode)
        {
            var glyph = this.CreateGlyph(charCode);
            if (glyph == null)
                return;

            var ftbmp = glyph.Bitmap;
            var metrics = glyph.Metrics;
            var height = (double)Math.Round((double)glyph.LinearVerticalAdvance);
            _ = (double)Math.Round((double)glyph.LinearHorizontalAdvance);
            var baseLine = height + (height * glyph.Face.Descender / glyph.Face.Height);
            var glyphMetrics = new GlyphMetrics()
            {
                ID = charCode,
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
            var charItem = new FontGlyph()
            {
                ID = charCode,
                Bitmap = this.CreateBitmap(ftbmp),
                Metrics = glyphMetrics,
            };
            this.glyphByID.Add(charCode, charItem);
        }

        private Bitmap CreateBitmap(FTBitmap ftbmp)
        {
            if (ftbmp.Rows > 0 && ftbmp.Width > 0)
            {
                var bitmap = ftbmp.ToGdipBitmap(System.Drawing.Color.White);
                return bitmap;
            }
            return null;
        }

        private GlyphSlot CreateGlyph(uint charCode)
        {
            var index = this.face.GetCharIndex(charCode);
            if (index == 0)
                return null;
            try
            {
                this.face.LoadGlyph(index, LoadFlags.Default, LoadTarget.Normal);
            }
            catch
            {
                return null;
            }
            this.face.Glyph.RenderGlyph(RenderMode.Normal);
            return this.face.Glyph;
        }

        public void Dispose()
        {
            this.glyphByID.Clear();
            this.Height = 0;
            this.face?.Dispose();
            this.face = null;
            this.lib?.Dispose();
            this.lib = null;
        }
    }
}
