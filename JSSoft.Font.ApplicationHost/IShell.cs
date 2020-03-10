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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace JSSoft.Font.ApplicationHost
{
    public interface IShell
    {
        Task OpenAsync(string fontPath, int size, int dpi, int faceIndex);

        Task CloseAsync();

        Task ExportAsync(string filename);

        Task SaveSettingsAsync();

        Task SaveSettingsAsync(string filename);

        Task LoadSettingsAsync(string filename);

        Task<FontData> PreviewAsync();

        Task SelectCharactersAsync(params uint[] characters);

        bool IsOpened { get; }

        bool IsModified { get; }

        bool IsProgressing { get; }

        string DisplayName { get; }

        double ZoomLevel { get; set; }

        FontInfo FontInfo { get; }

        ExportSettings Settings { get; }

        string SettingsPath { get; }

        IEnumerable<ICharacterGroup> Groups { get; }

        uint[] CheckedCharacters { get; }

        ICharacterGroup SelectedGroup { get; set; }

        ICharacter SelectedCharacter { get; set; }

        IEnumerable<string> RecentSettings { get; }

        IEnumerable<string> RecentFonts { get; }

        IPropertyService PropertyService { get; }

        event EventHandler Opened;

        event EventHandler Closed;

        event EventHandler SelectedGroupChanged;

        event EventHandler SelectedcharacterChanged;
    }
}
