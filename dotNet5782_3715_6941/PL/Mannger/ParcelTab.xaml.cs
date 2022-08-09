using PL.Mannger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL
{
    /// <summary>
    /// Interaction logic for ParcelTab.xaml
    /// </summary>
    public partial class ParcelTab : Page
    {
        #region Fields 
        private readonly BlApi.Ibl dat;
        private readonly bool works = false;
        #endregion

        public event ParcelEvent parcelAdd;
        public event ParcelEvent parcelRemove;
        public event ParcelEvent parcelUpdate; 
        
        public void Reset()
        {

            PopulateResetList();
            // PopulateResetScottPlot();



        }

        public ParcelTab(BlApi.Ibl dat)
        {
            InitializeComponent();
            this.dat = dat;
            Reset();
            WeightCB.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            PriCB.ItemsSource = Enum.GetValues(typeof(BO.Priorities));



        }




        private void ListOfPackges_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new ParcelShow(dat, dat.GetParcel(((sender as ListView).SelectedItem as BO.ParcelList).Id)).Show();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            ParcelShow add = new ParcelShow(dat);
            add.Closed += (sender, e) =>
            {
                Reset();
                parcelAdd(add.pcl);
            };
            add.Show();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delte this parcel ? this action is ireverseable",
"Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {

                    int id = (int)(sender as Button).Tag;
                    parcelRemove(dat.GetParcel(id));
                    dat.DeleteParcel(id);
                    Reset();

                    //reset();

                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message, "Error");
                }
            }

     

        }



        private void Smart_Search(object sender, RoutedEventArgs e)
        {
            ResultsOfSearch.ItemsSource = dat.SmartSearchParcel(SmartTB.Text);
        }

        private void ListOf_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        #region Populates
        private async void PopulateResetList()
        {
            ListOf.ItemsSource = await Task.Run(() => dat.GetParcels());
        }

        private async Task PopulateResetListFilter()
        {
            DateTime? F11 = F1.SelectedDate;
            DateTime? T11 = T1.SelectedDate;
            DateTime? F21 = F2.SelectedDate;
            DateTime? T21 = T2.SelectedDate;
            DateTime? F31 = F3.SelectedDate;
            DateTime? T31 = T3.SelectedDate;
            DateTime? F41 = F4.SelectedDate;
            DateTime? T41 = T4.SelectedDate;
            List<BO.WeightCategories> W = new List<BO.WeightCategories>();
            List<BO.Priorities> P = new List<BO.Priorities>();
            if (!(WeightCB.SelectedItem is null))
            {
                W.Add((BO.WeightCategories)WeightCB.SelectedItem);
            }

            if (!(PriCB.SelectedItem is null))
            {
                P.Add((BO.Priorities)PriCB.SelectedItem);
            }

            ListOf.ItemsSource = await Task.Run(() => dat.GetParcelsFiltered(
                (W.Count() == 0 ? (BO.WeightCategories[])Enum.GetValues(typeof(BO.WeightCategories)) : W)
                , (P.Count == 0 ? (BO.Priorities[])Enum.GetValues(typeof(BO.Priorities)) : P),
                F11, T11,
                 F21, T21,
                  F31, T31,
                   F41, T41

                )); ;
        }
        internal async Task PopulateResetScottPlot()
        {
            if (works)
            {
                return;
            }

            #region Plots Initialize 
            Task<double[]> task1_1 = Task.Run(() => dat.GetParcelsStatusesStats());
            Task<double[]> task2_1 = Task.Run(() => dat.GetParcelsWeightsStats());
            Task<double[]> task3_1 = Task.Run(() => dat.GetParcelsPrioretiesStats());
            double[] parcelstat = await task1_1;
            double[] WeightStat = await task2_1;
            double[] PrioStat = await task3_1; //Get all the data

            if (works)
            {
                return;
            }
            // launch all and only after wait for results  => much faster 
            Task? task1_3 = null;
            Task? task2_3 = null;
            Task? task3_3 = null;
            if (parcelstat.Length != 0)
            {
                task1_3 = Task.Run(() => WpfPlotPack1.CreateDountPie<BO.ParcelStatus>(parcelstat));
            }

            if (WeightStat.Length != 0)
            {
                task2_3 = Task.Run(() => WpfPlotPack2.CreateDountPie<BO.WeightCategories>(WeightStat));
            }

            if (PrioStat.Length != 0)
            {
                task3_3 = Task.Run(() => WpfPlotPack3.CreateDountPie<BO.Priorities>(PrioStat));
            }

            if (!(task1_3 is null))
            {
                await task1_3;
            }

            if (!(task2_3 is null))
            {
                await task2_3;
            }

            if (!(task3_3 is null))
            {
                await task3_3;
            }

            if (works)
            {
                return;
            }



            #endregion

        }
        #endregion

        private void ListOf_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void F1_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            await PopulateResetListFilter();
        }

        private async void Reset_Click(object sender, RoutedEventArgs e)
        {
            F1.SelectedDate = null;
            T1.SelectedDate = null;
            F2.SelectedDate = null;
            T2.SelectedDate = null;
            F3.SelectedDate = null;
            T3.SelectedDate = null;
            F4.SelectedDate = null;
            T4.SelectedDate = null;
            WeightCB.SelectedItem = null;
            PriCB.SelectedItem = null;
            await PopulateResetListFilter();
        }
    }
}
