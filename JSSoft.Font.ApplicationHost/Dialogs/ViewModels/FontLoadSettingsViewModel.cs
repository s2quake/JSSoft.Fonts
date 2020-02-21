using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font.ApplicationHost.Dialogs.ViewModels
{
    public class FontLoadSettingsViewModel : ModalDialogBase
    {
        private string face;
        private int size = 14;
        private int dpi = 72;

        public FontLoadSettingsViewModel(string path)
        {
            var faces = FontDescriptor.GetFaces(path);
            this.Faces = faces;
            this.face = faces.FirstOrDefault();
            this.DisplayName = "Font Options";
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

        public int FaceIndex
        {
            get
            {
                for (var i = 0; i < this.Faces.Length; i++)
                {
                    if (this.Faces[i] == this.Face)
                        return i;
                }
                return -1;
            }
        }

        public int Size
        {
            get => this.size;
            set
            {
                this.size = value;
                this.NotifyOfPropertyChange(nameof(Size));
            }
        }

        public int DPI
        {
            get => this.dpi;
            set
            {
                this.dpi = value;
                this.NotifyOfPropertyChange(nameof(DPI));
            }
        }
    }
}
