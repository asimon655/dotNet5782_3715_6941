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
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public IBL.Ibl dat { set; get; }
        public Window1()
        {
            InitializeComponent();
            dat= new BL.Bl();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GifLoader.LoadedBehavior = MediaState.Play;
            Task.Delay(1000);
            new MainWindow(dat).ShowDialog();
        }
    }
}
