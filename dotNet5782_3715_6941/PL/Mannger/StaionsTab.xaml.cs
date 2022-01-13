using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL
{
    /// <summary>
    /// Interaction logic for StaionsTab.xaml
    /// </summary>

    public partial class StaionsTab : Page
    {
        #region Fields 
        private readonly BlApi.Ibl dat;
        #endregion
        public Action reset;
        public void Reset()
        {
            #region List Initialize 
            ListOf.ItemsSource = dat.GetStations();
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ResultsOfSearch.ItemsSource = dat.SmartSearchStation(SmartTB.Text);
        }
    }
}
