using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font
{
    class Reservator : IReservator
    {
        private IReservator reservator;
        private Action action;

        public Reservator(Action action)
        {
            this.action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public Reservator(IReservator reservator, Action action)
            : this(action)
        {
            this.reservator = reservator;
        }

        public void Reserve()
        {
            if (this.action == null)
                throw new NotImplementedException();
            this.reservator?.Reserve();
            this.action();
            this.action = null;
        }

        public void Reject()
        {
            this.reservator?.Reject();
            this.action = null;
        }
    }
}
