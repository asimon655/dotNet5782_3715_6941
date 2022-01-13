using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL
{
    /// <summary>
    /// Interaction logic for StationsShow.xaml
    /// </summary>

    public partial class StationsShow : Window
    {
        private readonly BlApi.Ibl dat;
        public StationsShow(BlApi.Ibl dat, BO.Station stat)
        {
            InitializeComponent();
            this.dat = dat;
            DataContext = stat;
            Add.Visibility = Visibility.Hidden;
            Show.Visibility = Visibility.Visible;

        }
        public StationsShow(BlApi.Ibl dat)
        {
            InitializeComponent();
            this.dat = dat;
            Show.Visibility = Visibility.Hidden;
            Add.Visibility = Visibility.Visible;
        }


        private void ListOfPackgesFrom_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new Window2(dat, dat.GetDrone(((sender as ListView).SelectedItem as BO.DroneCharge).DroneId)).Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BO.Station add = new BO.Station()
                {
                    NumOfFreeOnes = int.Parse(NumOfFreeTB.Text),
                    Id = int.Parse(IDTB.Text),
                    Name = NameTB.Text,
                    LoctConstant = new BO.Location(double.Parse(LONGTB.Text), double.Parse(LATTB.Text))
                };
                dat.AddStation(add);
                Close();
            }
            catch (Exception err)
            {

                MessageBox.Show(err.Message, "Error");
            }
        }
    }
}
