using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL
{
    /// <summary>
    /// Interaction logic for ClientsTab.xaml
    /// </summary>
    /// 
    public partial class ClientsTab : Page
    {
        #region Fields 
        private readonly BlApi.Ibl dat;
        #endregion
        public Action reset;
        public void Reset()
        {
            #region List Initialize 
            ListOf.ItemsSource = dat.GetCustomers();
            #endregion


        }
        public ClientsTab(BlApi.Ibl dat)
        {

            this.dat = dat;
            InitializeComponent();
            Reset();




        }

        private void ListOfClients_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new CostumerShow(dat, dat.GetCostumer(((sender as ListView).SelectedItem as BO.CustomerList).Id)).Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window add = new CostumerShow(dat);
            add.Closed += (sender, e) =>
            {
                Reset();
                reset();
            };
            add.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {

                int id = (int)(sender as Button).Tag;
                dat.DeleteCustomer(id);
                Reset();
                reset();

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error");
            }
        }



        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ResultsOfSearch.ItemsSource = dat.SmartSearchCostumer(SmartTB.Text);
        }
        #region Populates 
        private async Task<IEnumerable<int>> GetHowManyReached()
        {
            return await Task.Run(() => (from cl in dat.GetCustomers() select cl.ParcelDeliveredAndGot).Distinct());

        }
        private async Task<IEnumerable<int>> GetHowManyUnReached()
        {
            return await Task.Run(() => (from cl in dat.GetCustomers() select cl.ParcelDeliveredAndNotGot).Distinct());

        }
        private async Task<IEnumerable<int>> GetHowManyParcelGot()
        {
            return await Task.Run(() => (from cl in dat.GetCustomers() select cl.ParcelGot).Distinct());

        }
        private async Task<IEnumerable<int>> GetHowManyInTheWay()
        {
            return await Task.Run(() => (from cl in dat.GetCustomers() select cl.InTheWay).Distinct());

        }

        private async Task search()
        {



            ListOf.ItemsSource = await Task.Run(() => dat.GetCustomers());

        }
        #endregion

        private async void ReachedCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await search();
        }

        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {

            await search();
        }
    }
}
