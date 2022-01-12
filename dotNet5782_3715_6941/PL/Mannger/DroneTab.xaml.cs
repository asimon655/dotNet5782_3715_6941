﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for DroneTab.xaml
    /// </summary>
    public partial class DroneTab : Page
    {
        #region Predicts 
        public Predicate<T> combine<T>(Predicate<T>? a, Predicate<T>? b) => new Predicate<T>(x => ConvertorNullable<T>(a)(x) && ConvertorNullable<T>(b)(x));
        Predicate<T> OrGate<T>(Predicate<T>? a, Predicate<T>? b) => new Predicate<T>(x => ConvertorNullable<T>(a)(x) || ConvertorNullable<T>(b)(x));
        Predicate<T> ConvertorNullable<T>(Predicate<T>? a) => (a is null ? new Predicate<T>(x => true) : a);
        public IEnumerable<BO.WeightCategories> Weight;
        public IEnumerable<BO.DroneStatuses> Stat;
        public IEnumerable<BO.WeightCategories> WeightDefault = (BO.WeightCategories[])Enum.GetValues(typeof(BO.WeightCategories));
        public IEnumerable<BO.DroneStatuses> StatDefault = (BO.DroneStatuses[])Enum.GetValues(typeof(BO.DroneStatuses));
        #endregion

        #region Fields 
        BlApi.Ibl dat;
        List<CheckBoxStatus> predStat;
        #endregion
        public Action reset;

        public DroneTab(BlApi.Ibl dat )
        {
            InitializeComponent();
    
            this.dat = dat;

            #region Predicts Initialize 
            ListOf.ItemsSource = dat.GetDrones();
            Weight = WeightDefault;
            Stat = StatDefault;
            predStat = new List<CheckBoxStatus>();
            foreach (BO.DroneStatuses enm in StatDefault)
                predStat.Add(new CheckBoxStatus() { Checked = true, statusof = enm });
            #endregion

            #region ListView Initialize 
            Binding myBinding = new Binding();
            myBinding.Source = dat.GetDronesFiltered(Stat, Weight);
            BindingOperations.SetBinding(ListOf, ListView.ItemsSourceProperty, myBinding);

            predStat = new List<CheckBoxStatus>();
            foreach (BO.DroneStatuses enm in StatDefault)
                predStat.Add(new CheckBoxStatus() { Checked = true, statusof = enm });
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
        #region Buttons Functions


        public async Task ResetPlots() {

            #region Plots Initialize 
            BO.DronesModelsStats dronesStats = await Task.Run( () => dat.GetDronesModelsStats());
            if (dronesStats.names.Length != 0 && dronesStats.vals.Length != 0 && dronesStats.pos.Length != 0)
            {
                ScottPlotHELP.createModelsBar(WpfPlot2, dronesStats.pos, dronesStats.names, dronesStats.vals);
                ScottPlotHELP.CreateDountPie<BO.WeightCategories>(WpfPlot1, await Task.Run(() => dat.GetDronesWeightsStats()));
                ScottPlotHELP.CreateDountPie<BO.DroneStatuses>(WpfPlot3, await Task.Run(() => dat.GetDronesStatusesStats()));
            }
            #endregion

        }

        public  void Reset()
        {
            //ListOf.ItemsSource = dat.GetDronesFiltered(Stat, Weight);
            ListOf.Items.Refresh();
            //ResetPlots();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Reset();
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
                new Window2(dat, dat.GetDrone(((sender as ListView).SelectedItem as BO.DroneList).Id), ()=>{ reset(); Reset(); }).Show();
            //new Window2(a).Show();

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Window add = new Window2(dat, this);
            add.Closed += (sender, e) =>
            {
                Reset();
                reset();
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
            Dictionary<String, String> dict = new Dictionary<string, string>();
            dict.Add("Id", "Id");
            dict.Add("Model", "Model");
            dict.Add("Battery", "Battery");
            dict.Add("Location", "Loct");
            dict.Add("MaxWeight", "Weight");
            dict.Add("DroneStatus", "DroneStat");
            dict.Add("Binded Parcel Id", "ParcelId");



            if (!((e.OriginalSource as GridViewColumnHeader) is null))
            {
                ListOf.ItemsSource = ListOf.ItemsSource.Cast<BO.DroneList>().OrderBy(x => typeof(BO.DroneList).GetProperty(dict[(e.OriginalSource as GridViewColumnHeader).Column.Header.ToString()]).GetValue(x, null));
                ListOf.ItemsSource = ListOf.ItemsSource.Cast<BO.DroneList>().Reverse();
            }
        }

        #endregion




        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new Window2(dat,this ).Show();

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {

                int id = (int)(sender as Button).Tag;
                dat.DeleteDrone(id);
                ListOf.ItemsSource = dat.GetDronesFiltered(Stat, Weight);
                Reset();
                reset();

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error");
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            ResultsOfSearch.ItemsSource = dat.SmartSearchDrone(SmartTB.Text);
        }

        private void ListOf_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
