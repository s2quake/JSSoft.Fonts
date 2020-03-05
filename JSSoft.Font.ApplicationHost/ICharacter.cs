using System.Windows.Media;

namespace JSSoft.Font.ApplicationHost
{
    public interface ICharacter
    {
        ICharacterRow Row { get; }

        GlyphMetrics GlyphMetrics { get; }

        uint ID { get; }

        bool IsChecked { get; set; }

        bool IsEnabled { get; }

        ImageSource Source { get; }
    }
}