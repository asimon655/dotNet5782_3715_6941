﻿using PL.Mannger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneTab.xaml
    /// </summary>
    public partial class DroneTab : Page
    {
        #region Predicts 
        public Predicate<T> combine<T>(Predicate<T>? a, Predicate<T>? b)
        {
            return new Predicate<T>(x => ConvertorNullable<T>(a)(x) && ConvertorNullable<T>(b)(x));
        }

        private Predicate<T> OrGate<T>(Predicate<T>? a, Predicate<T>? b)
        {
            return new Predicate<T>(x => ConvertorNullable<T>(a)(x) || ConvertorNullable<T>(b)(x));
        }

        private Predicate<T> ConvertorNullable<T>(Predicate<T>? a)
        {
            return (a is null ? new Predicate<T>(x => true) : a);
        }

        public IEnumerable<BO.WeightCategories> Weight;
        public IEnumerable<BO.DroneStatuses> Stat;
        public IEnumerable<BO.WeightCategories> WeightDefault = (BO.WeightCategories[])Enum.GetValues(typeof(BO.WeightCategories));
        public IEnumerable<BO.DroneStatuses> StatDefault = (BO.DroneStatuses[])Enum.GetValues(typeof(BO.DroneStatuses));
        #endregion

        #region Fields 
        private readonly BlApi.Ibl dat;
        private readonly List<CheckBoxStatus> predStat;
        #endregion

        #region events
    


        public DroneEvent droneAdd;
        public DroneEvent droneRemove;
        public DroneEvent droneUpdate;


        #endregion



        object Lock;
        public DroneTab(BlApi.Ibl dat)
        {
            InitializeComponent();
            this.droneRemove += (drn) => { Task.Run(() => { Task.Delay(1);  Dispatcher.Invoke(() => SmartSearch(null, null));  }); }; // delay 1 for the time that tike the xml revive 
            this.droneUpdate += (drn) => { Task.Run(() => { Task.Delay(1); Dispatcher.Invoke(() => SmartSearch(null, null)); }); }; // delay 1 for the time that tike the xml revive 
            this.droneAdd += (drn) => { Task.Run(() => { Task.Delay(1); Dispatcher.Invoke(() => SmartSearch(null, null)); }); };    // delay 1 for the time that tike the xml revive 
            this.dat = dat;
            Lock = new object();
            #region Predicts Initialize 
            ListOf.ItemsSource = dat.GetDrones();
            Weight = WeightDefault;
            Stat = StatDefault;
            predStat = new List<CheckBoxStatus>();
            predStat = StatDefault.Select(enm => new CheckBoxStatus() { Checked = true, statusof = enm }).ToList();
            #endregion

            #region ListView Initialize 
            Binding myBinding = new Binding
            {
                Source = dat.GetDronesFiltered(Stat, Weight)
            };
            BindingOperations.SetBinding(ListOf, ListView.ItemsSourceProperty, myBinding);

            predStat = StatDefault.Select(enm => new CheckBoxStatus() { Checked = true, statusof = enm }).ToList();


            StatusSelectorDrnStat.ItemsSource = predStat;
            StatusSelectorWeigthStat.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            #endregion

            Weight = WeightDefault;
            Stat = StatDefault;
            StatusSelectorWeigthStat.SelectedIndex = -1;
            StatusSelectorDrnStat.SelectedIndex = -1;
            predStat.ForEach(x => x.Checked = true);
            ResetPlots();





        }
        #region Buttons Functions


        public async Task ResetPlots()
        {

            #region Plots Initialize 
            BO.DronesModelsStats dronesStats = await Task.Run(() => dat.GetDronesModelsStats());
            if (dronesStats.names.Length != 0 && dronesStats.vals.Length != 0 && dronesStats.pos.Length != 0)
            {
                WpfPlot2.createModelsBar(dronesStats.pos, dronesStats.names, dronesStats.vals);
                WpfPlot1.CreateDountPie<BO.WeightCategories>(await Task.Run(() => dat.GetDronesWeightsStats()));
                WpfPlot3.CreateDountPie<BO.DroneStatuses>(await Task.Run(() => dat.GetDronesStatusesStats()));
            }
            #endregion

        }

        public void fullReset()
        {
            Binding myBinding = new Binding
            {
                Source = dat.GetDronesFiltered(Stat, Weight)
            };
            BindingOperations.SetBinding(ListOf, ListView.ItemsSourceProperty, myBinding);

           
            foreach (BO.DroneStatuses enm in StatDefault)
            {
                predStat.Add(new CheckBoxStatus() { Checked = true, statusof = enm });
            }

            StatusSelectorDrnStat.ItemsSource = predStat;
            StatusSelectorWeigthStat.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            #endregion
            Weight = WeightDefault;
            Stat = StatDefault;
            StatusSelectorWeigthStat.SelectedIndex = -1;
            StatusSelectorDrnStat.SelectedIndex = -1;
            foreach (CheckBoxStatus item in predStat)
            {
                item.Checked = true;
            }
            ResetPlots();



        }

        #region Clicks
        public void Reset()
        {
            //ListOf.ItemsSource = dat.GetDronesFiltered(Stat, Weight);
            ListOf.Items.Refresh();
            //ResetPlots();

        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            fullReset();
        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!((sender as ComboBox).SelectedItem is null))
            {
                Weight = new List<BO.WeightCategories>() { (BO.WeightCategories)(sender as ComboBox).SelectedItem };
            }
            ListOf.ItemsSource = dat.GetDronesFiltered(Stat, Weight);
        }

        private void ListOf_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!((sender as ListView).SelectedItem is null))
            {

                Window2 pg = new Window2(dat, dat.GetDrone(((sender as ListView).SelectedItem as BO.DroneList).Id));
                pg.reset += (obj) => { droneUpdate(obj); Reset(); };
                pg.Lock = this.Lock;
                pg.Show();
            }
            //new Window2(a).Show();

        }

        private void Add(object sender, RoutedEventArgs e)
        {
            Window2 add = new Window2(dat, this);
            add.Closed += (sender, e) =>
            {
                fullReset();
                droneAdd(add.drn);
  
            };
            add.Show();
        }

        private void chkCountry_Checked(object sender, RoutedEventArgs e)
        {
            if (!(StatusSelectorDrnStat.ItemsSource is null))
            {
                Stat = predStat.Where(x => x.Checked).Select(x => x.statusof);
            }
            ListOf.ItemsSource = dat.GetDronesFiltered(Stat, Weight);


        }

        private static bool IsSorted<T>(IEnumerable<T> enumerable) where T : IComparable<T>
        {
            T prev = default(T);
            bool prevSet = false;
            foreach (var item in enumerable)
            {
                if (prevSet && (prev == null || prev.CompareTo(item) > 0))
                {
                    return false;
                }

                prev = item;
                prevSet = true;
            }
            return true;
        }

        private void Battery_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>
            {
                { "Id", "Id" },
                { "Model", "Model" },
                { "Battery", "Battery" },
                { "Location", "Loct" },
                { "MaxWeight", "Weight" },
                { "DroneStatus", "DroneStat" },
                { "Binded Parcel Id", "ParcelId" }
            };



            if (!((e.OriginalSource as GridViewColumnHeader) is null))
            {
                ListOf.ItemsSource = ListOf.ItemsSource.Cast<BO.DroneList>().OrderBy(x => typeof(BO.DroneList).GetProperty(dict[(e.OriginalSource as GridViewColumnHeader).Column.Header.ToString()]).GetValue(x, null));
                ListOf.ItemsSource = ListOf.ItemsSource.Cast<BO.DroneList>().Reverse();
            }
        }

        private void Delte_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delte this drone ? this action is ireverseable ",
                "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {

                    int id = (int)(sender as Button).Tag;
                    droneRemove(dat.GetDrone(id));
                    dat.DeleteDrone(id);
                    ListOf.ItemsSource = dat.GetDronesFiltered(Stat, Weight);
                    fullReset();
                    



                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message, "Error");
                }
            }
    
        }

       

        private void SmartSearch(object sender, RoutedEventArgs e)
        {
            ResultsOfSearch.ItemsSource = dat.SmartSearchDrone(SmartTB.Text);
        }


        #endregion
    }
}
