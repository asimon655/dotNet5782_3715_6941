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
    /// Interaction logic for StaionsTab.xaml
    /// </summary>

    public partial class StaionsTab : Page
    {
        #region Fields 
        BlApi.Ibl dat;
        #endregion
        public Action reset;
        public void Reset()
        {
            #region List Initialize 
            ListOf.ItemsSource = dat.GetStations();
            #endregion

            #region ListView Grouping 
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListOf.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("BusyPorts");
            view.GroupDescriptions.Add(groupDescription);
            #endregion

        }
        public StaionsTab(BlApi.Ibl dat)
        {
            this.dat = dat;
            InitializeComponent();
            Reset();
        }

        private void ListOfStations_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new StationsShow(dat, dat.GetStation(((sender as ListView).SelectedItem as BO.StationList).Id)).Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window add = new StationsShow(dat);
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
                dat.DeleteStation(id);
                Reset();
                reset();

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error");
            }
        }
    }
}
