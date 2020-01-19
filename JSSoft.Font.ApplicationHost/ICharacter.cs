using System.Windows.Media;

namespace JSSoft.Font.ApplicationHost
{
    public interface ICharacter
    {
        GlyphMetrics GlyphMetrics { get; set; }

        uint ID { get; }

        bool IsChecked { get; set; }

        bool IsEnabled { get; set; }

        ImageSource Source { get; set; }
    }
}