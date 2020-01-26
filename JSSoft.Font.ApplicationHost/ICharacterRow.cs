namespace JSSoft.Font.ApplicationHost
{
    public interface ICharacterRow
    {
        uint Index { get; }

        bool? IsChecked { get; set; }

        ICharacter[] Items { get; }
    }
}