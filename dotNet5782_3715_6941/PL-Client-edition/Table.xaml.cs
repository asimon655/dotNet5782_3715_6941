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
        BO.Customer cst;
        BlApi.Ibl dat;
        public Table(BlApi.Ibl dat, BO.Customer cst)
        {
            InitializeComponent();
            this.dat = dat;
            this.cst = cst;
            this.DataContext = cst;
            #region ListView Grouping 
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(cst.ToClient);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Priority");
            view.GroupDescriptions.Add(groupDescription);
            view = (CollectionView)CollectionViewSource.GetDefaultView(cst.FromClient);
            groupDescription = new PropertyGroupDescription("Priority");
            view.GroupDescriptions.Add(groupDescription);
            #endregion
        }

        private void ListOfPackges_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void ListOfPackgesFrom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            new ParcelShow(dat, dat.GetParcel(((sender as ListView).SelectedItem as BO.ParcelInCustomer).Id)).Show();
        }
    }
}
