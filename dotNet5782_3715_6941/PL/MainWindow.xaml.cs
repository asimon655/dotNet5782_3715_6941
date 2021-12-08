﻿using System;
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
        public Predicate<T> combine<T>(Predicate<T> ? a, Predicate<T> ? b) =>  new Predicate<T>(x => ConvertorNullable<T>(a)(x) && ConvertorNullable<T>(b)(x));
        Predicate<T> OrGate<T>(Predicate<T>? a, Predicate<T>? b) => new Predicate<T>(x => ConvertorNullable<T>(a)(x) || ConvertorNullable<T>(b)(x));
        Predicate<T> ConvertorNullable<T>(Predicate<T>? a) => (a is null ? new Predicate<T>(x => true) : a); 
        public IEnumerable<IBL.BO.WeightCategories> Weight;
        public IEnumerable<IBL.BO.DroneStatuses> Stat;
        public IEnumerable<IBL.BO.WeightCategories> WeightDefault = (IBL.BO.WeightCategories[])Enum.GetValues(typeof(IBL.BO.WeightCategories));
        public IEnumerable<IBL.BO.DroneStatuses> StatDefault = (IBL.BO.DroneStatuses[])Enum.GetValues(typeof(IBL.BO.DroneStatuses));

        public IBL.Ibl a { set; get;  }
        public Stat status; 
        public MainWindow(IBL.Ibl x ,Stat gets )
        {
            a = x;
            status = gets;
            InitializeComponent();
            ListOf.ItemsSource = a.DronesPrint();
            Weight = WeightDefault;
            Stat = StatDefault;
            
            predStat = new List<CheckBoxStatus>();
            foreach (IBL.BO.DroneStatuses enm in StatDefault)
                predStat.Add(new CheckBoxStatus() { Checked = true, statusof = enm });
            StatusSelectorDrnStat.ItemsSource = predStat; 
            StatusSelectorWeigthStat.ItemsSource = Enum.GetValues(typeof(IBL.BO.WeightCategories));
            

            ListOf.ItemsSource = a.DronesPrintFiltered(Stat, Weight);

            
        }
        // reset button action
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Weight = WeightDefault;
            Stat = StatDefault;
            StatusSelectorWeigthStat.SelectedIndex = -1;
            StatusSelectorDrnStat.SelectedIndex = -1;
            foreach (CheckBoxStatus item in predStat)
            {
                item.Checked = true;
            }
            ListOf.ItemsSource =a.DronesPrintFiltered(Stat, Weight);
        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(( sender as ComboBox).SelectedItem  is null))
            {
                Weight = new List<IBL.BO.WeightCategories>() {(IBL.BO.WeightCategories)(sender as ComboBox).SelectedItem};
            }
            ListOf.ItemsSource = a.DronesPrintFiltered(Stat, Weight);
        }

        private void ListOf_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new Window2(a.PullDataDrone(((sender as ListView).SelectedItem as IBL.BO.DroneToList).Id)).Show( ) ;
            //new Window2(a).Show();

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            new Window2(a ,this ).Show();
        }

        private void chkCountry_Checked(object sender, RoutedEventArgs e)
        {
            if (!(StatusSelectorDrnStat.ItemsSource is null))
            {
                Stat = predStat.Where(x => x.Checked).Select(x => x.statusof);
            } 
            ListOf.ItemsSource = a.DronesPrintFiltered(Stat, Weight);


        }
        static bool IsSorted<T>(IEnumerable<T> enumerable) where T : IComparable<T>
        {
            T prev = default(T);
            bool prevSet = false;
            foreach (var item in enumerable)
            {
                if (prevSet && (prev == null || prev.CompareTo(item) > 0))
                    return false;
                prev = item;
                prevSet = true;
            }
            return true;
        }

        private void Battery_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<String, String> dict = new Dictionary<string, string>() ;
            dict.Add("Id", "Id");
            dict.Add("Model", "Model");
            dict.Add("Battery", "BatteryStat");
            dict.Add("Location", "Current");
            dict.Add("MaxWeight", "Weight");
            dict.Add("DroneStatus", "DroneStat");
            dict.Add("Binded Parcel Id", "ParcelIdTransfer");
            object IdLst = (object)
            MessageBox.Show((e.OriginalSource as GridViewColumnHeader).Column.Header.ToString());
      
                ListOf.ItemsSource = ListOf.ItemsSource.Cast<IBL.BO.DroneToList>().OrderBy(x => typeof(IBL.BO.DroneToList).GetProperty(dict[(e.OriginalSource as GridViewColumnHeader).Column.Header.ToString()]).GetValue(x, null));
            ListOf.ItemsSource = ListOf.ItemsSource.Cast<IBL.BO.DroneToList>().Reverse();
        }
    }
}
