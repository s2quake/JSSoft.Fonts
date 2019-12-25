using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace JSSoft.Font
{
    public interface IFontService
    {
        Task OpenAsync(string path);

        Task CloseAsync();

        bool Contains(uint id);

        BitmapSource GetBitmap(uint id);
    }
}
