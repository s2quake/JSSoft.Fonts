using Ntreev.Library.Commands;
using System;
using System.Collections.Generic;
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

                var font = new FontDescriptor(settings.FontPath, 72, 22);
                var writerSettings = new FontWriterSettings();
                var writer = new FontWriter(font, writerSettings);
                var characterList = new List<uint>(255);
                for (var i = 0u; i < 256; i++)
                {
                    characterList.Add(i);
                }
                writer.Generate(characterList.ToArray());

            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                Environment.Exit(1);
            }
        }
    }
}
