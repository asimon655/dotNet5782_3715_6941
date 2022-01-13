using System.Windows;

namespace PL_Client_edition
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Main : Window
    {
        private readonly BlApi.Ibl dat;
        private readonly BO.Customer cst;
        private Profile? prf = null;
        private Table? tbl = null;

        private void ResetProfile()
        {
            Profile.Visibility = Visibility.Visible;
            Parcels.Visibility = Visibility.Hidden;
            if (prf is null)
            {
                prf = new Profile(dat, cst);
                prf.DelteAccount.Click += (x, y) => { new LoginWindow(dat).Show(); Close(); };
            }
            Profile.NavigationService.Navigate(prf);
        }

        private void ResetPrcales()
        {
            Profile.Visibility = Visibility.Hidden;
            Parcels.Visibility = Visibility.Visible;
            if (tbl is null)
            {
                tbl = new Table(dat, cst);
            }

            Parcels.NavigationService.Navigate(tbl);
        }
        public Main(BlApi.Ibl dat, int costumerId)
        {
            InitializeComponent();
            this.dat = dat;
            cst = dat.GetCostumer(costumerId);
            ResetPrcales();


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ResetProfile();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ResetPrcales();
        }
    }
}
