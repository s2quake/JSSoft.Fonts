using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font.ApplicationHost
{
    class AppBootstrapper : AppBootstrapperBase
    {
        public AppBootstrapper()
            : base(typeof(IShell))
        {

        }
    }
}
