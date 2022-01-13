using System;
using System.Collections.Generic;
using System.IO;
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

namespace PL_Client_edition
{
    /// <summary>
    /// Interaction logic for Profile.xaml
    /// </summary>
    public partial class Profile : Page
    {
        BlApi.Ibl dat;
        BO.Customer cst;
        internal void MetaDataCstReset(System.Windows.Controls.Image Photo2, int SenderId)
        {

            if (!File.Exists(PhotoAsync.makePath(SenderId)))
                PhotoAsync.SaveImageAsync(PhotoAsync.FaceAIURL, PhotoAsync.makePath(SenderId), PhotoAsync.fileEndEnum).ContinueWith(x => {
                    if (x.Result)

                        Dispatcher.Invoke(() =>
                        {
                            Photo2.Source = new BitmapImage(new Uri(PhotoAsync.makePath(SenderId)));
                        });
                });
            else
                Photo2.Source = new BitmapImage(new Uri(PhotoAsync.makePath(SenderId)));


        }
        public Profile(BlApi.Ibl dat, BO.Customer cst)
        {
            InitializeComponent();
            this.dat = dat;
            this.cst = cst;
            this.DataContext = cst;
            MetaDataCstReset(CostumerPhoto, cst.Id);

        }

        private async void  Button_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(()=> { dat.DeleteCustomer(cst.Id); } );
           
        }
    }
}
