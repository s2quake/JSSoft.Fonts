using Ntreev.Library;
using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font.ApplicationHost.Dialogs.ViewModels
{
    public class AboutViewModel : ModalDialogBase
    {
        public AboutViewModel()
        {
            this.DisplayName = "About";
            this.Version = AppUtility.ProductVersion;
        }

        public string Version { get; }
    }
}
