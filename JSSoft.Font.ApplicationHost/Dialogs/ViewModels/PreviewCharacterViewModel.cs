using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace JSSoft.Font.ApplicationHost.Dialogs.ViewModels
{
    public class PreviewCharacterViewModel : ModalDialogBase
    {
        public PreviewCharacterViewModel(ICharacter character)
        {
            this.Character = character ?? throw new ArgumentNullException(nameof(character));
            this.DisplayName = "Preview";


        }

        public ICharacter Character { get; }

        public ImageSource ImageSource => this.Character.Source;

        public GlyphMetrics GlyphMetrics => this.Character.GlyphMetrics;

        public Thickness Thickness
        {
            get
            {
                var left = this.GlyphMetrics.HorizontalBearingX;
                var top = this.GlyphMetrics.BaseLine - this.GlyphMetrics.HorizontalBearingY;
                var right = this.GlyphMetrics.HorizontalAdvance - (left + this.GlyphMetrics.Width);
                var bottom = this.GlyphMetrics.VerticalAdvance - (top + this.GlyphMetrics.Height);
                return new Thickness(left, top, right, bottom);
            }
        }
    }
}
