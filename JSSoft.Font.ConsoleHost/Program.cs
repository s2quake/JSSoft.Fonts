using Ntreev.Library.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                parser.Parse(Environment.CommandLine);

                var font = new FontDescriptor(settings.FontPath, (uint)settings.DPI, settings.Size);
                var dataSettings = new FontDataSettings()
                {
                    Width = settings.TextureWidth,
                    Height = settings.TextureHeight,
                };
                var data = new FontData(font, dataSettings);
                var characterList = new List<uint>(255);
                var filename = Path.GetFullPath(settings.FileName);
                var directory = Path.GetDirectoryName(filename);
                for (var i = 0u; i < 256; i++)
                {
                    characterList.Add(i);
                }
                data.Generate(characterList.ToArray());
                data.Save(filename);
                data.SavePages(directory);

            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                Environment.Exit(1);
            }
        }
    }
}
