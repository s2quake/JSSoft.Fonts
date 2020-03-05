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

                var inputPath = Path.GetFullPath(settings.FontPath);
                var outputPath = Path.GetFullPath(settings.FileName);
                var name = Path.GetFileNameWithoutExtension(outputPath);
                var font = new FontDescriptor(inputPath, (uint)settings.DPI, settings.Size, settings.Face);
                var dataSettings = new FontDataSettings()
                {
                    Name = name,
                    Width = settings.TextureWidth,
                    Height = settings.TextureHeight,
                    Characters = settings.Characters.ToArray(),
                };
                
                var data = font.CreateData(dataSettings);
                var directory = Path.GetDirectoryName(outputPath);
                data.Save(outputPath);
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
