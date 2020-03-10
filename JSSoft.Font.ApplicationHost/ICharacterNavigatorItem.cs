using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font.ApplicationHost
{
    public interface ICharacterNavigatorItem
    {
        ICharacter Character { get; }

        bool IsCurrent { get; }
    }
}
