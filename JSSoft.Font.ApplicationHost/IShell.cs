using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font.ApplicationHost
{
    public interface IShell
    {
        Task OpenAsync(string fontPath);

        Task CloseAsync();

        Task ExportAsync(string filename);

        bool IsOpened { get; }

        bool IsProgressing { get; }

        string DisplayName { get; }

        double ZoomLevel { get; set; }

        ExportSettings ExportSettings { get; set; }

        IEnumerable<ICharacterGroup> Groups { get; }

        ICharacterGroup SelectedGroup { get; set; }
    }
}
