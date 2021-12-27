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
    /// Interaction logic for ClientsTab.xaml
    /// </summary>
    /// 
    public partial class ClientsTab : Page
    {
        #region Fields 
        BlApi.Ibl dat;
        #endregion
        public ClientsTab(BlApi.Ibl dat)
        {

            this.dat = dat;
            InitializeComponent();

            #region List Initialize 
            ListOfClients.ItemsSource = dat.GetCustomers();
            #endregion

            #region ListView Grouping 
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListOfClients.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("ParcelGot");
            view.GroupDescriptions.Add(groupDescription);
            #endregion
  
        }

        private void ListOfClients_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new CostumerShow(dat, dat.GetCostumer(((sender as ListView).SelectedItem as BO.CustomerList).Id)).Show();
        }
    }
}
