using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font
{
    public sealed class FontPage
    {
        public int ID { get; internal set; }

        public string Name { get; internal set; }

        public Bitmap Bitmap { get; internal set; }
    }
}
