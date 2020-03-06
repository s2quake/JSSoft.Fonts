using Ntreev.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font.ApplicationHost.PropertyItems.ViewModels
{
    [Export(typeof(IPropertyItem))]
    [Dependency(typeof(FontInfoViewModel))]
    class CharacterGroupViewModel : PropertyItemBase
    {
        private readonly IShell shell;
        private uint min;
        private uint max;
        private ICharacterGroup characterGroup;

        [ImportingConstructor]
        public CharacterGroupViewModel(IShell shell)
        {
            this.shell = shell;
            this.DisplayName = "Group Info";
            this.shell.SelectedGroupChanged += Shell_SelectedGroupChanged;
        }

        public override bool CanSupport(object obj)
        {
            return true;
        }

        public override void SelectObject(object obj)
        {

        }

        public uint Min
        {
            get => this.min;
            private set
            {
                this.min = value;
                this.NotifyOfPropertyChange(nameof(Min));
                this.NotifyOfPropertyChange(nameof(MinString));
            }
        }

        public uint Max
        {
            get => this.max;
            private set
            {
                this.max = value;
                this.NotifyOfPropertyChange(nameof(Max));
                this.NotifyOfPropertyChange(nameof(MaxString));
            }
        }

        public string MinString => $"0x{this.Min:X}";

        public string MaxString => $"0x{this.Max:X}";

        public override bool IsVisible => true;

        public override object SelectedObject => this.characterGroup;

        private void Shell_SelectedGroupChanged(object sender, EventArgs e)
        {
            if (this.shell.SelectedGroup is ICharacterGroup group)
            {
                this.Min = group.Min;
                this.Max = group.Max;
                this.characterGroup = group;
            }
            else
            {
                this.Min = 0;
                this.Max = 0;
                this.characterGroup = null;
            }
            this.NotifyOfPropertyChange(nameof(SelectedObject));
        }
    }
}
