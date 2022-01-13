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

namespace PL_Client_edition
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Main : Window
    {
        BlApi.Ibl dat;
        BO.Customer cst;
        Profile? prf = null; 
        Table ?  tbl =null ; 
        void ResetProfile()
        {
            Profile.Visibility = Visibility.Visible;
            Parcels.Visibility = Visibility.Hidden;
            if (prf is null)
            {
                prf = new Profile(dat, cst);
                prf.DelteAccount.Click += (x, y) => { new LoginWindow(dat).Show(); this.Close(); };
            }
            Profile.NavigationService.Navigate(prf);
        }
        void ResetPrcales()
        {
            Profile.Visibility = Visibility.Hidden;
            Parcels.Visibility = Visibility.Visible;
            if(tbl is null )
                tbl = new Table(dat, cst);
            Parcels.NavigationService.Navigate(tbl);
        }
        public Main(BlApi.Ibl dat , int costumerId)
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
