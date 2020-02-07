using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font.ApplicationHost.Serializations
{
    public struct CharacterGroupInfo
    {
        public string Name { get; set; }

        public CharacterRowInfo[] Rows { get; set; }
    }
}
