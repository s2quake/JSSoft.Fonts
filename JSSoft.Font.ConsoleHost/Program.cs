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
                    Characters = settings.GetCharacters(),
                };
                var data = font.CreateData(dataSettings);
                var filename = Path.GetFullPath(settings.FileName);
                var directory = Path.GetDirectoryName(filename);
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
