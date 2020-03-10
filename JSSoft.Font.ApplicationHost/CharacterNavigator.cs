using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;

namespace JSSoft.Font.ApplicationHost
{
    [Export(typeof(ICharacterNavigator))]
    class CharacterNavigator : PropertyChangedBase, ICharacterNavigator
    {
        private readonly IShell shell;
        private readonly ObservableCollection<CharacterNavigatorItem> itemList = new ObservableCollection<CharacterNavigatorItem>();
        private ICharacter currentCharacter;
        private CharacterNavigatorItem currentItem;

        [ImportingConstructor]
        public CharacterNavigator(IShell shell)
        {
            this.shell = shell;
            this.shell.Opened += Shell_Opened;
            this.shell.Closed += Shell_Closed;
        }

        public void Backward()
        {
            var index = this.itemList.IndexOf(this.currentItem) - 1;
            this.Current = this.itemList[index];
        }

        public void Forward()
        {
            var index = this.itemList.IndexOf(this.currentItem) + 1;
            this.Current = this.itemList[index];
        }

        public void Add(ICharacter character)
        {
            if (character != null)
            {
                var index = this.itemList.IndexOf(this.currentItem);
                while (this.itemList.Count != index + 1)
                {
                    this.itemList.RemoveAt(this.itemList.Count - 1);
                }
                this.itemList.Add(new CharacterNavigatorItem(character));
                if (this.currentItem != null)
                    this.currentItem.IsCurrent = false;
                this.currentItem = this.itemList.Last();
                this.currentItem.IsCurrent = true;
                this.currentCharacter = this.currentItem.Character;
                while (this.itemList.Count >= this.MaximumCount)
                {
                    this.itemList.RemoveAt(0);
                }
                this.NotifyOfPropertyChange(nameof(Count));
                this.NotifyOfPropertyChange(nameof(CanBackward));
                this.NotifyOfPropertyChange(nameof(CanForward));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public void Clear()
        {
            if (this.currentItem != null)
                this.currentItem.IsCurrent = false;
            this.currentItem = null;
            this.currentCharacter = null;
            this.itemList.Clear();
            this.NotifyOfPropertyChange(nameof(Count));
            this.NotifyOfPropertyChange(nameof(CanBackward));
            this.NotifyOfPropertyChange(nameof(CanForward));
            CommandManager.InvalidateRequerySuggested();
        }

        public bool CanBackward
        {
            get
            {
                var index = this.itemList.IndexOf(this.Current);
                return index > 0;
            }
        }

        public bool CanForward
        {
            get
            {
                var index = this.itemList.IndexOf(this.Current);
                return index + 1 < this.itemList.Count;
            }
        }

        public CharacterNavigatorItem Current
        {
            get => this.currentItem;
            set
            {
                this.shell.SelectedcharacterChanged -= Shell_SelectedcharacterChanged;
                if (this.currentItem != null)
                    this.currentItem.IsCurrent = false;
                this.currentItem = value ?? throw new ArgumentNullException(nameof(value));
                this.currentCharacter = this.currentItem.Character;
                this.currentItem.IsCurrent = true;
                this.shell.SelectedCharacter = this.currentCharacter;
                this.shell.SelectedcharacterChanged += Shell_SelectedcharacterChanged;
                this.NotifyOfPropertyChange(nameof(Count));
                this.NotifyOfPropertyChange(nameof(CanBackward));
                this.NotifyOfPropertyChange(nameof(CanForward));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public IEnumerable<CharacterNavigatorItem> Items => this.itemList;

        public int Count => this.itemList.Count;

        public int MaximumCount { get; set; } = 20;

        public TimeSpan RecordDely { get; set; } = TimeSpan.FromMilliseconds(300);

        public CharacterNavigatorItem this[int index]
        {
            get => this.itemList[index];
        }

        private void Shell_Closed(object sender, EventArgs e)
        {
            this.shell.SelectedcharacterChanged -= Shell_SelectedcharacterChanged;
            this.itemList.Clear();
            this.currentCharacter = null;
            this.currentItem = null;
            CommandManager.InvalidateRequerySuggested();
        }

        private void Shell_Opened(object sender, EventArgs e)
        {
            this.shell.SelectedcharacterChanged += Shell_SelectedcharacterChanged;
        }

        private void Shell_SelectedcharacterChanged(object sender, EventArgs e)
        {
            this.Add(this.shell.SelectedCharacter);
        }

        #region ICharacterNavigator

        IEnumerable<ICharacterNavigatorItem> ICharacterNavigator.Items => this.itemList;

        ICharacterNavigatorItem ICharacterNavigator.Current
        {
            get => this.Current;
            set
            {
                if (value is CharacterNavigatorItem item)
                {
                    this.Current = item;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        ICharacterNavigatorItem ICharacterNavigator.this[int index] => this[index];

        #endregion
    }
}
