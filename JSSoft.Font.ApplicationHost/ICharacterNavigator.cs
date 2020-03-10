using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font.ApplicationHost
{
    public interface ICharacterNavigator
    {
        void Forward();

        void Backward();

        void Add(ICharacter character);

        void Clear();

        IEnumerable<ICharacterNavigatorItem> Items { get; }

        ICharacterNavigatorItem Current { get; set; }

        int Count { get; }

        ICharacterNavigatorItem this[int index] { get; }

        bool CanForward { get; }

        bool CanBackward { get; }

        int MaximumCount { get; set; }
    }
}
