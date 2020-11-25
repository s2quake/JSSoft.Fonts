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

using JSSoft.ModernUI.Framework;

namespace JSSoft.Fonts.ApplicationHost.UndoActions
{
    class UncheckCharacterGroupAction : UndoBase
    {
        private readonly ICharacterGroup group;
        private readonly bool?[][] rows;

        public UncheckCharacterGroupAction(ICharacterGroup group)
        {
            this.group = group;
            this.rows = new bool?[group.Items.Length][];
            for (var i = 0; i < this.rows.Length; i++)
            {
                this.rows[i] = new bool?[group.Items[i].Items.Length];
                for (var j = 0; j < this.rows[i].Length; j++)
                {
                    var character = group.Items[i].Items[j];
                    if (character.IsEnabled == true)
                    {
                        this.rows[i][j] = character.IsChecked;
                    }
                }
            }
        }

        public override string ToString()
        {
            return $"Uncheck Group: {this.group.Name}";
        }

        protected override void OnRedo()
        {
            this.group.IsChecked = false;
        }

        protected override void OnUndo()
        {
            for (var i = 0; i < this.rows.Length; i++)
            {
                for (var j = 0; j < this.rows[i].Length; j++)
                {
                    var isChecked = this.rows[i][j];
                    var character = this.group.Items[i].Items[j];
                    if (isChecked != null && character.IsChecked != isChecked.Value)
                    {
                        character.IsChecked = isChecked.Value;
                    }
                }
            }
        }
    }
}
