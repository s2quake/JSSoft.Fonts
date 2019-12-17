using SharpFont;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JSSoft.Font
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var lib = new Library();
            var face = new Face(lib, @"SF-Mono-Semibold.otf");
            face.SetCharSize(0, 11, 0, 72);

            byte[] buffer = new byte[100];
            foreach (var item in NamesList.items)
            {
                for (var i=item.min; i<=item.max; i++)
                {
                    var index = face.GetCharIndex(i);
                    if (index != 0)
                    {
                        var name = face.GetGlyphName(index, buffer);
                        System.Diagnostics.Debug.WriteLine((char)i);
                    }
                }
                
            }
        }
    }
}
