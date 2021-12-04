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

namespace PL
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        IBL.Ibl dat; 
        public Page1(IBL.Ibl dat)
        {
            this.dat = dat; 
            InitializeComponent();
            ListOf.ItemsSource = dat.StaionsPrint();
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            MediaElement media = (MediaElement)sender;
            media.LoadedBehavior = MediaState.Manual;
            media.Position = TimeSpan.FromMilliseconds(1);
            media.Play();


        }
    }
}
