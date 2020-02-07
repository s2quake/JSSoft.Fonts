using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font.ApplicationHost.Serializations
{
    public struct CharacterRowInfo
    {
        public int Index { get; set; }

        public uint[] Characters { get; set; }
    }
}
