using PL.Mannger;
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
        public event StationEvent stationAdd;
        public event StationEvent stationRemove;
        public event StationEvent stationUpdate;

     
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
            StationsShow add = new StationsShow(dat);
            add.statAdd+=((stat) => stationAdd(stat));
            add.Closed += (sender, e) =>
            {
                Reset();
               
            };
            add.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you want delte that station? this action is ireverseable ",
"Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {

                try
                {

                    int id = (int)(sender as Button).Tag;
                    stationRemove(dat.GetStation(id));
                    dat.DeleteStation(id);
                    Reset();
                  

                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message, "Error");
                }
            }
    

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ResultsOfSearch.ItemsSource = dat.SmartSearchStation(SmartTB.Text);
        }

        private void WpfPlot2_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
