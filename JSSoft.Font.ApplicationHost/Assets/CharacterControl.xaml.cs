using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JSSoft.Font.ApplicationHost.Assets
{
    partial class CharacterControl : ResourceDictionary
    {
        public CharacterControl()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.MenuItem menuItem && menuItem.Tag is JSSoft.Font.ApplicationHost.Controls.CharacterControl control)
            {
                control.Save();
            }
        }
    }
}
