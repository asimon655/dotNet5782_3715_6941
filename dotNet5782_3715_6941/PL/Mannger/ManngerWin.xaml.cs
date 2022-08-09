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
        private readonly BackgroundWorker MapandGraphTasker;
        private readonly Dictionary<string, PivotHeaderControl> RefreshMannger = new Dictionary<string, PivotHeaderControl>();
        private readonly DroneTab Drn;
        private readonly MapTab Map;
        private readonly ClientsTab Client;
        private readonly StaionsTab Stat;
        private readonly ParcelTab pcl;
        const int intervalMapHZ = 4; // 4 picks per sec ( 4 updates ) 
        #endregion

    
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
           
            dat = BlApi.BlFactory.GetBl(); 

            InitializeComponent();

            #region pages init
            Drn = new DroneTab(dat);
            Map = new MapTab(dat);
            Client = new ClientsTab(dat);
            Stat = new StaionsTab(dat);
            pcl = new ParcelTab(dat); 
            #endregion

            #region dict init
            RefreshMannger.Add("Parcels", ParcelsPHC);
            RefreshMannger.Add("Map", MapPHC);
            RefreshMannger.Add("Drones", DronesPHC);
            RefreshMannger.Add("Stations", StationsPHC);
            RefreshMannger.Add("Costumers", CostumersPHC);
            #endregion

            #region event sign

            Drn.droneRemove += (obj) => { };
            Drn.droneUpdate += (obj) => { ResetByWindow(); };
            Client.resetData += () => { ResetByWindow(); };
            pcl.parcelUpdate += (obj) => { ResetByWindow(); };
            Stat.stationRemove += (obj) => { ResetByWindow(); };
            Stat.stationAdd += (obj) => {  };
            Drn.droneAdd += (drn) => { Map.ResetLoct();  }; 
            Drn.droneRemove+=(drn) => { Map.ResetLoct();  };


            #endregion

            #region frames init
            DroneFrame.NavigationService.Navigate(Drn);
            ParcelFrame.NavigationService.Navigate(pcl);
            MapFrame.NavigationService.Navigate(Map);
            CostumerFrame.NavigationService.Navigate(Client);
            StationFrame.NavigationService.Navigate(Stat);
            #endregion

            #region Map and Graph  bgWorker
 
            MapandGraphTasker = new BackgroundWorker
            {
                WorkerReportsProgress = true
            };
            MapandGraphTasker.RunWorkerCompleted += (x, y) => { };
            MapandGraphTasker.DoWork += (x, y) =>
            {
                while (true)
                {
                    Thread.Sleep((int)(1.0f / intervalMapHZ * 1000));
                    Map.ResetLoct();
                    MapandGraphTasker.ReportProgress(3);
                    pcl.PopulateResetScottPlot().ContinueWith((x) => { MapandGraphTasker.ReportProgress(1); });
                    Drn.ResetPlots().ContinueWith((x) => { MapandGraphTasker.ReportProgress(2); });
                    
                }
            };
            MapandGraphTasker.ProgressChanged += (x, y) => { 
               

                if (y.ProgressPercentage == 1)
                {
                    try
                    {
                        pcl.WpfPlotPack1.Render();
                        pcl.WpfPlotPack2.Render();
                        pcl.WpfPlotPack3.Render();
                    }
                    catch
                    {
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
                    catch
                    {
                        MessageBox.Show("UnKnown error in plots happend!!!");
                    }
                }
                if (y.ProgressPercentage == 3) {
                    Map.MyMapControl.Refresh();

                }


            };
            MapandGraphTasker.RunWorkerAsync();

            #endregion

      

          
        }
       

    }
}
