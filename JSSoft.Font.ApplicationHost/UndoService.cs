using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font.ApplicationHost
{
    [Export(typeof(IUndoService))]
    class UndoService : UndoServiceBase
    {

    }
}
