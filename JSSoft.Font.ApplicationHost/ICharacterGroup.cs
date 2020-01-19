namespace JSSoft.Font
{
    public interface ICharacterGroup
    {
        string Name { get; }

        bool? IsChecked { get; set; }

        ICharacterRow[] Items { get; }
    }
}