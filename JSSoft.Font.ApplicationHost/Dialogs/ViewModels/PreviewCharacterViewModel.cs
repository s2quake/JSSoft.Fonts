using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
