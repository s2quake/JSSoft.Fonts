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
        Task OpenAsync(string fontPath, int faceIndex);

        Task CloseAsync();

        Task ExportAsync(string filename);

        Task SaveSettingsAsync(string filename);

        Task LoadSettingsAsync(string filename);

        Task<ImageSource[]> PreviewAsync();

        bool IsOpened { get; }

        bool IsModified { get; }

        bool IsProgressing { get; }

        string DisplayName { get; }

        double ZoomLevel { get; set; }

        ExportSettings Settings { get; }

        IEnumerable<ICharacterGroup> Groups { get; }

        ICharacterGroup SelectedGroup { get; set; }

        IEnumerable<string> RecentSettings { get; }

        event EventHandler Opened;

        event EventHandler Closed;
    }
}
