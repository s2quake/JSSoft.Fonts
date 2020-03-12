using Ntreev.ModernUI.Framework.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
