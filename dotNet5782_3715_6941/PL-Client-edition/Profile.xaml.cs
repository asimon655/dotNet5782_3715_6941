using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace PL_Client_edition
{
    /// <summary>
    /// Interaction logic for Profile.xaml
    /// </summary>
    public partial class Profile : Page
    {
        private readonly BlApi.Ibl dat;
        private readonly BO.Customer cst;
        internal void MetaDataCstReset(System.Windows.Controls.Image Photo2, int SenderId)
        {

            if (!File.Exists(PhotoAsync.makePath(SenderId)))
            {
                PhotoAsync.SaveImageAsync(PhotoAsync.FaceAIURL, PhotoAsync.makePath(SenderId), PhotoAsync.fileEndEnum).ContinueWith(x =>
                {
                    if (x.Result)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            Photo2.Source = new BitmapImage(new Uri(PhotoAsync.makePath(SenderId)));
                        });
                    }
                });
            }
            else
            {
                Photo2.Source = new BitmapImage(new Uri(PhotoAsync.makePath(SenderId)));
            }
        }
        public Profile(BlApi.Ibl dat, BO.Customer cst)
        {
            InitializeComponent();
            this.dat = dat;
            this.cst = cst;
            DataContext = cst;
            MetaDataCstReset(CostumerPhoto, cst.Id);

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => { dat.DeleteCustomer(cst.Id); });

        }
    }
}
