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
                var dataSettings = new FontDataSettings();
                var data = new FontData(font, dataSettings);
                var characterList = new List<uint>(255);
                for (var i = 0u; i < 256; i++)
                {
                    characterList.Add(i);
                }
                data.Generate(characterList.ToArray());
                data.Save(@"C:\Users\s2quake\Desktop\test.fnt");
                data.SavePages(@"C:\Users\s2quake\Desktop");

            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                Environment.Exit(1);
            }
        }
    }
}
