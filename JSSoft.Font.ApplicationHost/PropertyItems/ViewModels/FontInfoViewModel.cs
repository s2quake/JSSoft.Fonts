using Ntreev.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font.ApplicationHost.PropertyItems.ViewModels
{
    [Export(typeof(IPropertyItem))]
    class FontInfoViewModel : PropertyItemBase
    {
        private readonly IShell shell;
        private FontInfo fontInfo = FontInfo.Empty;

        [ImportingConstructor]
        public FontInfoViewModel(IShell shell)
        {
            this.shell = shell;
            this.DisplayName = "Font Info";
            this.shell.Opened += Shell_Opened;
            this.shell.Closed += Shell_Closed;
        }

        public override bool CanSupport(object obj)
        {
            return true;
        }

        public override void SelectObject(object obj)
        {
            //this.character = obj as ICharacter;
            //if (this.character != null && this.character.IsEnabled == true)
            //{
            //    this.GlyphMetrics = this.character.GlyphMetrics;
            //}
            //else
            //{
            //    this.GlyphMetrics = GlyphMetrics.Empty;
            //}
            //this.NotifyOfPropertyChange(nameof(SelectedObject));
            //this.NotifyOfPropertyChange(nameof(GlyphMetrics));
        }

        public FontInfo FontInfo
        {
            get => this.fontInfo;
            private set
            {
                this.fontInfo = value;
                this.NotifyOfPropertyChange(nameof(FontInfo));
            }
        }

        public override bool IsVisible => true;

        public override object SelectedObject => null;
        
        private void Shell_Closed(object sender, EventArgs e)
        {
            this.FontInfo = this.shell.FontInfo;
        }

        private void Shell_Opened(object sender, EventArgs e)
        {
            this.FontInfo = this.shell.FontInfo;
        }
    }
}
