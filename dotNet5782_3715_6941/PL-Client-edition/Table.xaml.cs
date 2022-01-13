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

namespace PL_Client_edition
{
    /// <summary>
    /// Interaction logic for Table.xaml
    /// </summary>
    public partial class Table : Page
    {
        async Task ResetTables()
        {
            cst = await Task.Run(() => {
                return dat.GetCostumer(cst.Id);
            });
            this.DataContext = cst;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(cst.ToClient);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Priority");
            view.GroupDescriptions.Add(groupDescription);
            view = (CollectionView)CollectionViewSource.GetDefaultView(cst.FromClient);
            groupDescription = new PropertyGroupDescription("Priority");
            view.GroupDescriptions.Add(groupDescription);
        }
        BO.Customer cst;
        BlApi.Ibl dat;
        public Table(BlApi.Ibl dat, BO.Customer cst)
        {
            InitializeComponent();
            this.dat = dat;
            this.cst = cst;
            this.DataContext = cst;
            ResetTables();
        }

        private void ListOfPackges_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void ListOfPackgesFrom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(!((sender as ListView).SelectedItem is null))
                new ParcelShow(dat, dat.GetParcel(((sender as ListView).SelectedItem as BO.ParcelInCustomer).Id)).Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
          ParcelShow pclWin =   new ParcelShow(dat , cst.Id);
            pclWin.Closed +=  (async (x, y) => {

                await ResetTables();


            });
            pclWin.Show();

        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
           await ResetTables();
        }
    }
}
