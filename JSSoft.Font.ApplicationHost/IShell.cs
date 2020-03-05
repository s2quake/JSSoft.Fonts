using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace JSSoft.Font.ApplicationHost
{
    public interface IShell
    {
        Task OpenAsync(string fontPath, int size, int dpi, int faceIndex);

        Task CloseAsync();

        Task ExportAsync(string filename);

        Task SaveSettingsAsync(string filename);

        Task LoadSettingsAsync(string filename);

        Task<FontData> PreviewAsync();

        Task SelectCharactersAsync(params uint[] characters);

        bool IsOpened { get; }

        bool IsModified { get; }

        bool IsProgressing { get; }

        string DisplayName { get; }

        double ZoomLevel { get; set; }

        FontInfo FontInfo { get; }

        ExportSettings Settings { get; }

        IEnumerable<ICharacterGroup> Groups { get; }

        uint[] CheckedCharacters { get; }

        ICharacterGroup SelectedGroup { get; set; }

        ICharacter SelectedCharacter { get; set; }

        IEnumerable<string> RecentSettings { get; }

        IPropertyService PropertyService { get; }

        event EventHandler Opened;

        event EventHandler Closed;

        event EventHandler SelectedGroupChanged;

        event EventHandler SelectedcharacterChanged;
    }
}
