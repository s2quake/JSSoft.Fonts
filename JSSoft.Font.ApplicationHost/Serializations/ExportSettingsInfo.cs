using Ntreev.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JSSoft.Font.ApplicationHost.Serializations
{
    [XmlRoot]
    public struct ExportSettingsInfo
    {
        [XmlElement(Order = 0)]
        public string Font { get; set; }

        [XmlElement(Order = 1)]
        public int Size { get; set; }

        [XmlElement(Order = 2)]
        public int DPI { get; set; }

        [XmlElement(Order = 3)]
        public int TextureWidth { get; set; }

        [XmlElement(Order = 4)]
        public int TextureHeight { get; set; }

        [XmlIgnore]
        public (int Left, int Top, int Right, int Bottom) Padding { get; set; }

        [XmlIgnore]
        public (int Vertical, int Horizontal) Spacing { get; set; }

        [XmlIgnore]
        public uint[] Characters { get; set; }

        [XmlElement(nameof(Padding), Order = 5)]
        public string PaddingValue
        {
            get => $"{this.Padding.Top},{this.Padding.Right},{this.Padding.Bottom},{this.Padding.Left}";
            set
            {
                var items = value.Split(',');
                this.Padding = (int.Parse(items[0]), int.Parse(items[1]), int.Parse(items[2]), int.Parse(items[3]));
            }
        }

        [XmlElement(nameof(Spacing), Order = 6)]
        public string SpacingValue
        {
            get => $"{this.Spacing.Vertical},{this.Spacing.Horizontal}";
            set
            {
                var items = value.Split(',');
                this.Spacing = (int.Parse(items[0]), int.Parse(items[1]));
            }
        }

        [XmlElement(nameof(Characters), Order = 7)]
        public string CharactersValue
        {
            get => $"{new CharacterIDCollection(this.Characters)}";
            set => this.Characters = new CharacterIDCollection(value).ToArray();
        }

        internal static ExportSettingsInfo Create(ShellViewModel shell)
        {
            var settings = shell.ExportSettings;
            var fontDescriptor = shell.FontDescriptor;
            var query = from characterGroup in shell.Groups
                        where characterGroup.IsChecked != false
                        from row in characterGroup.Items
                        where row.IsChecked != false
                        from item in row.Items
                        where item.IsChecked && item.IsEnabled
                        select item.ID;

            return new ExportSettingsInfo()
            {
                Font = fontDescriptor.FontPath,
                Size = fontDescriptor.Height,
                DPI = (int)fontDescriptor.DPI,
                TextureWidth = settings.TextureWidth,
                TextureHeight = settings.TextureHeight,
                Padding = ((int)settings.Padding.Left, (int)settings.Padding.Top, (int)settings.Padding.Right, (int)settings.Padding.Bottom),
                Spacing = (settings.HorizontalSpace, settings.VerticalSpace),
                Characters = query.ToArray(),
            };
        }
    }
}
