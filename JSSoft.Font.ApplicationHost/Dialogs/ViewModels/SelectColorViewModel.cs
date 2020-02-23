using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace JSSoft.Font.ApplicationHost.Dialogs.ViewModels
{
    public class SelectColorViewModel : ModalDialogBase
    {
        private Color currentColor;

        public SelectColorViewModel()
        {
            this.DisplayName = "Select Color";
        }

        public void Select()
        {
            this.TryClose(true);
        }

        public Color CurrentColor
        {
            get => this.currentColor;
            set
            {
                this.currentColor = value;
                this.NotifyOfPropertyChange(nameof(this.CurrentColor));
            }
        }
    }
}
