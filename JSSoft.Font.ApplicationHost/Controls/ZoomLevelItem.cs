using Ntreev.ModernUI.Framework.DataGrid.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace JSSoft.Font.ApplicationHost.Controls
{
    public struct ZoomLevelItem
    {
        private string text;

        public double Level { get; set; }

        public string Text
        {
            get => this.text ?? $"{this.Level * 100} %";
            set => this.text = value;
        }

        public override string ToString()
        {
            return this.Text;
        }
    }
}
