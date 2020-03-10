using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace JSSoft.Font.ApplicationHost
{
    public interface ICharacter
    {
        ICharacterRow Row { get; }

        GlyphMetrics GlyphMetrics { get; }

        uint ID { get; }

        bool IsChecked { get; set; }

        bool IsEnabled { get; }

        BitmapSource Source { get; }
    }
}