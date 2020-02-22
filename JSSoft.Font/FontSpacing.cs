using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font
{
    public struct FontSpacing
    {
        public FontSpacing(int horizontal, int vertical)
        {
            this.Horizontal = horizontal;
            this.Vertical = vertical;
        }

        public int Horizontal { get; set; }

        public int Vertical { get; set; }
    }
}
