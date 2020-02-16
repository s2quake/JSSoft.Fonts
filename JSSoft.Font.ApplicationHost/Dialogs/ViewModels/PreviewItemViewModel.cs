using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace JSSoft.Font.ApplicationHost.Dialogs.ViewModels
{
    class PreviewItemViewModel : PropertyChangedBase
    {
        public PreviewItemViewModel(int index, ImageSource image)
        {
            this.Index = index;
            this.Image = image;
        }

        public int Index { get; }

        public ImageSource Image { get; }
    }
}
