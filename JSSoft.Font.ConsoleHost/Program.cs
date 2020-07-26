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

using Ntreev.Library.Commands;
using System;
using System.IO;
using System.Linq;

namespace JSSoft.Font.ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var settings = new Settings();
            var parser = new CommandLineParser(settings);
            try
            {
                if (parser.TryParse(Environment.CommandLine) == true)
                {
                    var inputPath = Path.GetFullPath(settings.FontPath);
                    var outputPath = Path.GetFullPath(settings.OutputPath);
                    var name = Path.GetFileNameWithoutExtension(outputPath);
                    var font = new FontDescriptor(inputPath, (uint)settings.DPI, settings.Size, settings.Face);
                    var dataSettings = new FontDataSettings()
                    {
                        Name = name,
                        Width = settings.TextureWidth,
                        Height = settings.TextureHeight,
                        Padding = settings.Padding,
                        Spacing = settings.Spacing,
                        Characters = settings.Characters.ToArray(),
                    };

                    var data = font.CreateData(dataSettings);
                    var directory = Path.GetDirectoryName(outputPath);
                    data.Save(outputPath);
                    data.SavePages(directory);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                Environment.Exit(1);
            }
        }
    }
}
