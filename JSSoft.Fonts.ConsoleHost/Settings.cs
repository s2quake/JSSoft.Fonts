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

using JSSoft.Library.Commands;
using System.ComponentModel;

namespace JSSoft.Fonts.ConsoleHost
{
    [ResourceDescription("Properties/Resources")]
    class Settings
    {
        [CommandPropertyRequired]
        public string FontPath { get; set; }

        [CommandPropertyRequired]
        public string OutputPath { get; set; }

        [CommandProperty("characters")]
        public CharacterCollection Characters { get; set; } = CharacterCollection.Empty;

        [CommandProperty("dpi", InitValue = FontDescriptor.DefaultDPI)]
        public int DPI { get; set; }

        [CommandProperty(InitValue = FontDescriptor.DefaultSize)]
        public int Size { get; set; }

        [CommandProperty(InitValue = FontDescriptor.DefaultFace)]
        public int Face { get; set; }

        [CommandProperty(InitValue = FontDataSettings.DefaultWidth)]
        public int TextureWidth { get; set; }

        [CommandProperty(InitValue = FontDataSettings.DefaultHeight)]
        public int TextureHeight { get; set; }

        [CommandProperty(InitValue = FontDataSettings.DefaultPadding)]
        public FontPadding Padding { get; set; }

        [CommandProperty(InitValue = FontDataSettings.DefaultSpacing)]
        public FontSpacing Spacing { get; set; }
    }
}
