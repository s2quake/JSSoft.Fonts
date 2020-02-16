using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font.ApplicationHost.Dialogs.ViewModels
{
    public class SelectFaceViewModel : ModalDialogBase
    {
        private string face;
        public SelectFaceViewModel(string[] faces)
        {
            this.Faces = faces;
            this.face = faces.FirstOrDefault();
            this.DisplayName = "Select Face";
        }

        public void Select()
        {
            this.TryClose(true);
        }

        public string[] Faces { get; }

        public string Face
        {
            get => this.face;
            set
            {
                this.face = value;
                this.NotifyOfPropertyChange(nameof(Face));
            }
        }
    }
}
