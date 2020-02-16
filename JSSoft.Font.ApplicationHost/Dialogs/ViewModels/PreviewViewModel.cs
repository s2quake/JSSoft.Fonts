using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace JSSoft.Font.ApplicationHost.Dialogs.ViewModels
{
    class PreviewViewModel : ModalDialogBase
    {
        private PreviewItemViewModel image;

        public PreviewViewModel(ImageSource[] images)
        {
            var itemList = new List<PreviewItemViewModel>(images.Length);
            for (var i = 0; i < images.Length; i++)
            {
                var image = images[i];
                itemList.Add(new PreviewItemViewModel(i, image));
            }
            this.Items = itemList.ToArray();
            this.image = itemList.First();
            this.DisplayName = "Preview";
        }

        public PreviewItemViewModel[] Items { get; }

        public PreviewItemViewModel Item
        {
            get => this.image;
            set
            {
                this.image = value;
                this.NotifyOfPropertyChange(nameof(Item));
            }
        }
    }
}
