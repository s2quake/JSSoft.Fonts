using Ntreev.Library.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font.ConsoleHost
{
    class Settings
    {
        [CommandProperty(IsRequired = true)]
        public string FontPath { get; set; }

        [CommandProperty(IsRequired = true)]
        public string OutputPath { get; set; }
    }
}
