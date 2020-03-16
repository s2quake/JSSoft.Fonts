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

using JSSoft.Font.ApplicationHost.Properties;
using Ntreev.ModernUI.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace JSSoft.Font.ApplicationHost.Dialogs.ViewModels
{
    public class FontLoadSettingsViewModel : ModalDialogBase
    {
        private string face;
        private int size = 14;
        private int dpi = 72;

        public FontLoadSettingsViewModel(string path)
        {
            var faces = FontDescriptor.GetFaces(path);
            this.Faces = faces;
            this.face = faces.FirstOrDefault();
            this.DisplayName = Resources.Title_FontOptions;
        }

        public async Task SelectAsync()
        {
            await this.TryCloseAsync(true);
        }

        public string[] Faces { get; }

        public string Face
        {
            get => this.face;
            set
            {
                this.face = value;
                this.NotifyOfPropertyChange(nameof(Face));
            }
        }

        public int FaceIndex
        {
            get
            {
                for (var i = 0; i < this.Faces.Length; i++)
                {
                    if (this.Faces[i] == this.Face)
                        return i;
                }
                return -1;
            }
        }

        public int Size
        {
            get => this.size;
            set
            {
                this.size = value;
                this.NotifyOfPropertyChange(nameof(Size));
            }
        }

        public int DPI
        {
            get => this.dpi;
            set
            {
                this.dpi = value;
                this.NotifyOfPropertyChange(nameof(DPI));
            }
        }
    }
}
