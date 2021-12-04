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
    public class CheckBoxStatus
    {
        public bool Checked { get; set; }
        public IBL.BO.DroneStatuses statusof{get ; set ; }
    
    
    }
    public partial class MainWindow : Page
    {
       List <CheckBoxStatus> predStat;
        Predicate<T> combine<T>(Predicate<T> ? a, Predicate<T> ? b) =>  new Predicate<T>(x => ConvertorNullable<T>(a)(x) && ConvertorNullable<T>(b)(x));
        Predicate<T> OrGate<T>(Predicate<T>? a, Predicate<T>? b) => new Predicate<T>(x => ConvertorNullable<T>(a)(x) || ConvertorNullable<T>(b)(x));
        Predicate<T> ConvertorNullable<T>(Predicate<T>? a) => (a is null ? new Predicate<T>(x => true) : a); 
        Predicate<IBL.BO.DroneToList>? Weight;
         Predicate<IBL.BO.DroneToList>? Stat;
        public IBL.Ibl a { set; get;  }
        public Stat status; 
        public MainWindow(IBL.Ibl x ,Stat gets )
        {
            a = x;
            status = gets;
            InitializeComponent();
            ListOf.ItemsSource = a.DronesPrint();
            Weight= null; 
           Stat = null;
            
            predStat = new List<CheckBoxStatus>();
            foreach (var enm in (IBL.BO.DroneStatuses[])Enum.GetValues(typeof(IBL.BO.WeightCategories)))
                predStat.Add(new CheckBoxStatus() { Checked = true, statusof = enm });
            StatusSelectorDrnStat.ItemsSource = predStat; 
            StatusSelectorWeigthStat.ItemsSource = Enum.GetValues(typeof(IBL.BO.WeightCategories));
            ListOf.ItemsSource = a.DronesPrintFiltered(combine<IBL.BO.DroneToList>(Weight, Stat));
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Weight = null;
            Stat = null;
            StatusSelectorWeigthStat.SelectedIndex = -1;
            StatusSelectorDrnStat.SelectedIndex = -1; 
            ListOf.ItemsSource =a.DronesPrintFiltered(combine<IBL.BO.DroneToList>(Weight, Stat));

        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Weight = (x => (IBL.BO.WeightCategories)(( sender as ComboBox).SelectedItem  is null ? x.Weight : (sender as ComboBox).SelectedItem) == x.Weight);
            ListOf.ItemsSource = a.DronesPrintFiltered(combine<IBL.BO.DroneToList>(Weight, Stat));
        } 

 



        private void StatusSelectorDrnStat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            Stat = (x =>   (IBL.BO.DroneStatuses)( (sender as ComboBox).SelectedItem is null ? x.DroneStat: (sender as ComboBox).SelectedItem )  == x.DroneStat);
            ListOf.ItemsSource = a.DronesPrintFiltered(combine<IBL.BO.DroneToList>(Weight, Stat));
  

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void ListOf_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new Window2(a.PullDataDrone(((sender as ListView).SelectedItem as IBL.BO.DroneToList).Id)).Show( ) ;
            //new Window2(a).Show();

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            new Window2(a, ListOf ,this ).Show();
        }

        private void chkCountry_Checked(object sender, RoutedEventArgs e)
        {
            Stat = (x => false);
            if (StatusSelectorDrnStat.ItemsSource is null)
            {
                Stat = (x => false);

            }
            else
            {
                foreach (var predi  in predStat)
                {
                    if (predi.Checked )
                    {
                        Stat = OrGate(Stat, (x => x.DroneStat == predi.statusof));
                    }
                }
            } 
            ListOf.ItemsSource = a.DronesPrintFiltered(combine<IBL.BO.DroneToList>(Weight, Stat));


        }
    }
}
