using Ntreev.ModernUI.Framework.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JSSoft.Font.ApplicationHost.Dialogs.ViewModels
{
    class GlyphItemViewModel : ListBoxItemViewModel
    {
        private readonly FontGlyphData data;

        public GlyphItemViewModel(FontGlyphData data)
        {
            this.data = data;
        }

        public override string ToString() => this.DisplayName;

        public override string DisplayName => $"{FontGlyphUtility.ToString(this.data.ID)}";

        public Rect Rectangle => new Rect(this.data.Rectangle.X, this.data.Rectangle.Y, this.data.Rectangle.Width, this.data.Rectangle.Height);

        public FontGlyphData GlyphData => this.data;
    }
}
