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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public IBL.Ibl a { set; get;  }
        public MainWindow(IBL.Ibl x )
        {
            a = x; 
            InitializeComponent();
            ListOf.ItemsSource = a.DronesPrint();
  
            StatusSelectorDrnStat.ItemsSource = Enum.GetValues(typeof(IBL.BO.DroneStatuses));
            StatusSelectorWeigthStat.ItemsSource = Enum.GetValues(typeof(IBL.BO.WeightCategories));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
            ListOf.ItemsSource =a.DronesPrint();
           
        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListOf.ItemsSource = a.DronesPrintFiltered((x => (IBL.BO.DroneStatuses) (sender as ComboBox).SelectedItem == x.DroneStat) , ListOf.ItemsSource.Cast<IBL.BO.DroneToList>());

        }

        private void ListOf_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListOf.ItemsSource = a.DronesPrintFiltered((x => (IBL.BO.WeightCategories)(sender as ComboBox).SelectedItem == x.Weight), ListOf.ItemsSource.Cast<IBL.BO.DroneToList>());
        }

        private void StatusSelectorDrnStat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            ListOf.ItemsSource = a.DronesPrintFiltered((x => (IBL.BO.DroneStatuses)(sender as ComboBox).SelectedItem == x.DroneStat), ListOf.ItemsSource.Cast<IBL.BO.DroneToList>());

        }
    }
}
