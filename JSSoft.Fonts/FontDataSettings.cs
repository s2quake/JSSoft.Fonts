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

namespace JSSoft.Fonts
{
    public class FontDataSettings
    {
        public const int DefaultWidth = 512;
        public const int DefaultHeight = 512;
        public const int DefaultPadding = 1;
        public const int DefaultSpacing = 1;

        public string Name { get; set; } = string.Empty;

        public int Width { get; set; } = DefaultWidth;

        public int Height { get; set; } = DefaultHeight;

        public FontPadding Padding { get; set; } = new FontPadding(DefaultPadding);

        public FontSpacing Spacing { get; set; } = new FontSpacing(DefaultSpacing);

        public uint[] Characters { get; set; }

        public static FontDataSettings Default { get; } = new FontDataSettings();

        internal int Capacity => this.Characters != null ? this.Characters.Length : 0;
    }
}
