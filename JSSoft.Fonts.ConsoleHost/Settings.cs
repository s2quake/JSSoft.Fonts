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
    [ResourceDescription]
    class Settings
    {
        [CommandPropertyRequired]
        public string FontPath { get; set; }

        [CommandPropertyRequired(DefaultValue = "")]
        public string OutputPath { get; set; }

        [CommandProperty("characters")]
        [CommandPropertyTrigger(nameof(OutputPath), "", IsInequality = true)]
        public CharacterCollection Characters { get; set; } = CharacterCollection.Empty;

        [CommandProperty("dpi", InitValue = FontDescriptor.DefaultDPI)]
        [CommandPropertyTrigger(nameof(OutputPath), "", IsInequality = true)]
        public int DPI { get; set; }

        [CommandProperty(InitValue = FontDescriptor.DefaultSize)]
        [CommandPropertyTrigger(nameof(OutputPath), "", IsInequality = true)]
        public int Size { get; set; }

        [CommandProperty(InitValue = FontDescriptor.DefaultFace)]
        [CommandPropertyTrigger(nameof(OutputPath), "", IsInequality = true)]
        public int Face { get; set; }

        [CommandProperty(InitValue = FontDataSettings.DefaultWidth)]
        [CommandPropertyTrigger(nameof(OutputPath), "", IsInequality = true)]
        public int TextureWidth { get; set; }

        [CommandProperty(InitValue = FontDataSettings.DefaultHeight)]
        [CommandPropertyTrigger(nameof(OutputPath), "", IsInequality = true)]
        public int TextureHeight { get; set; }

        [CommandProperty(InitValue = FontDataSettings.DefaultPadding)]
        [CommandPropertyTrigger(nameof(OutputPath), "", IsInequality = true)]
        public FontPadding Padding { get; set; }

        [CommandProperty(InitValue = FontDataSettings.DefaultSpacing)]
        [CommandPropertyTrigger(nameof(OutputPath), "", IsInequality = true)]
        public FontSpacing Spacing { get; set; }
    }
}
