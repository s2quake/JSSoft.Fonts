// MIT License
// 
// Copyright (c) 2020 Jeesu Choi
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
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
        public CharacterNavigator(IShell shell, IServiceProvider serviceProvider)
            : base(serviceProvider)
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

        public CharacterNavigatorItem this[int index] => this.itemList[index];

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
