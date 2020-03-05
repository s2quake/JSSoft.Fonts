namespace JSSoft.Font.ApplicationHost
{
    public interface ICharacterRow
    {
        ICharacterGroup Group { get; }

        uint Index { get; }

        bool? IsChecked { get; set; }

        ICharacter[] Items { get; }
    }
}