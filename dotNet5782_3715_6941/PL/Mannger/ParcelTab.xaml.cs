using System;
using System.Collections.Generic;
using System.Drawing;
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
    /// Interaction logic for ParcelTab.xaml
    /// </summary>
    public partial class ParcelTab : Page
    {
        #region Fields 
        BlApi.Ibl dat;
        bool works = false; 
        #endregion
        public Action reset;
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




        }




        private void ListOfPackges_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new ParcelShow(dat, dat.GetParcel(((sender as ListView).SelectedItem as BO.ParcelList).Id)).Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window add = new ParcelShow(dat);
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
                dat.DeleteParcel(id);
                Reset();
                //reset();

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error");
            }

        }


            
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ResultsOfSearch.ItemsSource = dat.SmartSearchParcel(SmartTB.Text);
        }

        private void ListOf_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        #region Populates
        async void PopulateResetList()
        {
            ListOf.ItemsSource = await Task.Run(() => dat.GetParcels());
        }
        async  void PopulateResetScottPlot()
        {
            if (works)
                return;

            #region Plots Initialize 
            Task<double[]> task1_1 = Task.Run(() => dat.GetParcelsStatusesStats());
            Task<double[]> task2_1 = Task.Run(() => dat.GetParcelsWeightsStats());
            Task<double[]> task3_1 = Task.Run(() => dat.GetParcelsPrioretiesStats());
            Task task1_2 = Task.Run(() => ScottPlotHELP.ClearGraph(WpfPlotPack1));
            Task task2_2 = Task.Run(() => ScottPlotHELP.ClearGraph(WpfPlotPack2));
            Task task3_2 = Task.Run(() => ScottPlotHELP.ClearGraph(WpfPlotPack3));
            double[] parcelstat = await task1_1;
            double[] WeightStat =await task2_1 ;
            double[] PrioStat = await task3_1; //Get all the data
            await task1_2;
            await task2_2;
            await task3_2;
            if (works)
                return;
            Task ?  task1_3 = null ;
            Task ?  task2_3 =null ;
            Task ?  task3_3 =null ;
            if (parcelstat.Length != 0)
                task1_3 = Task.Run(() => ScottPlotHELP.CreateDountPie<BO.ParcelStatus>(WpfPlotPack1, parcelstat));
            if (WeightStat.Length != 0)
                task2_3 = Task.Run(() => ScottPlotHELP.CreateDountPie<BO.WeightCategories>(WpfPlotPack2, WeightStat));
            if (PrioStat.Length != 0)
                task3_3 = Task.Run(() => ScottPlotHELP.CreateDountPie<BO.Priorities>(WpfPlotPack3, PrioStat));
            if (!(task1_3 is null))
                await task1_3;
            if (!(task2_3 is null))
                await task2_3;
            if (!(task3_3 is null))
                await task3_3;
             if (works)
                return;
            
            if (!works)
            {
                works = true; 
                Dispatcher.Invoke(() =>
                {
                    WpfPlotPack1.Refresh();
                    WpfPlotPack2.Refresh();
                    WpfPlotPack3.Refresh();
                });
                works = false;
            }


            #endregion

        }
        #endregion

        private void ListOf_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
