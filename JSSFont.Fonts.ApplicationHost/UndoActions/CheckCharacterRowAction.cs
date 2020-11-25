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

namespace JSSoft.Font.ApplicationHost.UndoActions
{
    class CheckCharacterRowAction : UndoBase
    {
        private readonly ICharacterRow row;
        private readonly bool?[] items;

        public CheckCharacterRowAction(ICharacterRow row)
        {
            this.row = row;
            this.items = new bool?[row.Items.Length];
            for (var i = 0; i < this.items.Length; i++)
            {
                var character = row.Items[i];
                if (character.IsEnabled == true)
                {
                    this.items[i] = character.IsChecked;
                }
            }
        }

        public override string ToString()
        {
            return $"Check Row: {(char)this.row.Index}";
        }

        protected override void OnRedo()
        {
            this.row.IsChecked = true;
        }

        protected override void OnUndo()
        {
            for (var i = 0; i < this.items.Length; i++)
            {
                var isChecked = this.items[i];
                var character = this.row.Items[i];
                if (isChecked != null && character.IsChecked != isChecked.Value)
                {
                    character.IsChecked = isChecked.Value;
                }
            }
        }
    }
}
