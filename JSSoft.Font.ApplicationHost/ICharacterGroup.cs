using System.ComponentModel;

namespace JSSoft.Font.ApplicationHost
{
    public interface ICharacterGroup
    {
        string Name { get; }

        bool? IsChecked { get; set; }

        uint Min { get; }

        uint Max { get; }

        ICharacterRow[] Items { get; }
    }
}