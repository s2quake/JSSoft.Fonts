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

using Ntreev.Library;
using System;
using System.ComponentModel.Composition;

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
                //this.NotifyOfPropertyChange(nameof(MinString));
            }
        }

        public uint Max
        {
            get => this.max;
            private set
            {
                this.max = value;
                this.NotifyOfPropertyChange(nameof(Max));
                //this.NotifyOfPropertyChange(nameof(MaxString));
            }
        }

        //public string MinString => $"0x{this.Min:X}";

        //public string MaxString => $"0x{this.Max:X}";

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
