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
    /// Interaction logic for StationsShow.xaml
    /// </summary>

    public partial class StationsShow : Window
    {
        BlApi.Ibl dat;
        public StationsShow(BlApi.Ibl dat, BO.Station stat)
        {
            InitializeComponent();
            this.dat = dat;
            this.DataContext = stat;
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
            new Window2(dat,dat.GetDrone(((sender as ListView).SelectedItem as BO.DroneCharge).DroneId)).Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BO.Station add = new BO.Station()
                {
                    NumOfFreeOnes = Int32.Parse(NumOfFreeTB.Text),
                    Id = Int32.Parse(IDTB.Text),
                    Name =NameTB.Text,
                    LoctConstant = new BO.Location(Double.Parse(LONGTB.Text), Double.Parse(LATTB.Text))
                };
                dat.AddStation(add);
                this.Close(); 
            }
            catch (Exception err)
            {

                MessageBox.Show(err.Message, "Error"); 
            }
        }
    }
}
