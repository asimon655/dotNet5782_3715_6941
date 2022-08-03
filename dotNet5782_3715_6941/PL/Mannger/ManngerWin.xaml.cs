using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WPFSpark;


namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ManngerWin : Window
    {

        #region Fields

        private readonly BlApi.Ibl dat;
        private readonly BackgroundWorker MapTasker;
        private readonly BackgroundWorker GraphTasker;
        private readonly Dictionary<string, PivotHeaderControl> RefreshMannger = new Dictionary<string, PivotHeaderControl>();
        private readonly DroneTab Drn;
        private readonly MapTab Map;
        private readonly ClientsTab Client;
        private readonly StaionsTab Stat;
        private readonly ParcelTab pcl;
        #endregion

        #region Ctor 
        private async Task ResetByWindow()
        {

            if (RefreshMannger["Drones"].IsActive)
            {
                await Task.Run(() => Dispatcher.Invoke(() =>
               {
                   Drn.Reset();
               }));
            }
            if (RefreshMannger["Map"].IsActive)
            {
                await Task.Run(async () => await Dispatcher.Invoke(async () =>
                {
                    await Map.ResetLoct();
                }));

            }
            if (RefreshMannger["Parcels"].IsActive)
            {
                await Task.Run(async () =>
                {
                    await Task.Delay(600);
                    Dispatcher.Invoke(() =>
                    {
                        pcl.Reset();
                    });
                });

            }
            if (RefreshMannger["Stations"].IsActive)
            {
                await Task.Run(async () =>
                {
                    await Task.Delay(600);
                    Dispatcher.Invoke(() =>
                    {
                        Stat.Reset();
                    });
                });

            }
            if (RefreshMannger["Costumers"].IsActive)
            {
                await Task.Run(async () =>
                {
                    await Task.Delay(600);
                    Dispatcher.Invoke(() =>
                    {
                        Client.Reset();
                    });
                });
            }



        }
        public ManngerWin()
        {

            dat = BlApi.BlFactory.GetBl(); ;
            InitializeComponent();
            #region Framses-Initialize 
            Drn = new DroneTab(dat);
            Map = new MapTab(dat);
            Client = new ClientsTab(dat);
            Stat = new StaionsTab(dat);
            pcl = new ParcelTab(dat);
            RefreshMannger.Add("Parcels", ParcelsPHC);
            RefreshMannger.Add("Map", MapPHC);
            RefreshMannger.Add("Drones", DronesPHC);
            RefreshMannger.Add("Stations", StationsPHC);
            RefreshMannger.Add("Costumers", CostumersPHC);



            Drn.Deltereset = () => { Map.Reset(); };
            Drn.reset = () => { ResetByWindow(); };
            Client.reset = () => { ResetByWindow(); };
            pcl.reset = () => { ResetByWindow(); };
            Stat.reset = () => { ResetByWindow(); };
            DroneFrame.NavigationService.Navigate(Drn);
            ParcelFrame.NavigationService.Navigate(pcl);
            MapFrame.NavigationService.Navigate(Map);
            CostumerFrame.NavigationService.Navigate(Client);
            StationFrame.NavigationService.Navigate(Stat);
            MapTasker = new BackgroundWorker
            {
                WorkerReportsProgress = true
            };
            MapTasker.RunWorkerCompleted += (x, y) => { };

            MapTasker.DoWork += (x, y) =>
            {
                while (true)
                {
                    Thread.Sleep(100);
                    Map.ResetLoct();
                    MapTasker.ReportProgress(1);
                }
            };
            MapTasker.ProgressChanged += (x, y) => { Map.MyMapControl.Refresh(); };
            MapTasker.RunWorkerAsync();
            GraphTasker = new BackgroundWorker
            {
                WorkerReportsProgress = true
            };
            GraphTasker.RunWorkerCompleted += (x, y) => { };

            GraphTasker.DoWork += (x, y) =>
            {
                while (true)
                {
                    Thread.Sleep(1000);
                    pcl.PopulateResetScottPlot().ContinueWith((x) => { GraphTasker.ReportProgress(1); });
                    Drn.ResetPlots().ContinueWith((x) => { GraphTasker.ReportProgress(2); });

                }
            };
            GraphTasker.ProgressChanged += (x, y) =>
            {
                if (y.ProgressPercentage == 1)
                {
                    try
                    {
                        pcl.WpfPlotPack1.Render();
                        pcl.WpfPlotPack2.Render();
                        pcl.WpfPlotPack3.Render();
                    }
                    catch {
                        MessageBox.Show("UnKnown error in plots happend!!!");
                    }
                }
                if (y.ProgressPercentage == 2)
                {
                    try
                    {
                        Drn.WpfPlot1.Render();
                        Drn.WpfPlot2.Render();
                        Drn.WpfPlot3.Render();
                    }
                    catch {
                        MessageBox.Show("UnKnown error in plots happend!!!");
                    }
                }
            };
            GraphTasker.RunWorkerAsync();

            #endregion
        }
        #endregion

        #region Buttons Functions
        // reset button action


        #endregion
    }
}
