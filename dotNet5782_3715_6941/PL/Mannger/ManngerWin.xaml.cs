using ScottPlot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using WPFSpark;
using Mapsui.Utilities;
using Mapsui.Layers;
using HarfBuzzSharp;
using Mapsui.Styles;
using Mapsui.Providers;


namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ManngerWin :Window
    {

        #region Fields
        public Stat status;
        BlApi.Ibl dat;
        BackgroundWorker MapTasker;
        BackgroundWorker GraphTasker;
        #endregion

        #region Ctor 
        public ManngerWin(BlApi.Ibl dat)
        {
           
            this.dat = dat;
            InitializeComponent(); 
            #region Framses-Initialize 
            DroneTab Drn = new DroneTab(dat);
            MapTab Map = new MapTab(dat) ;
            ClientsTab Client = new ClientsTab(dat);
            StaionsTab Stat = new StaionsTab(dat);
            ParcelTab pcl = new ParcelTab(dat);
            Drn.reset = ()=> { /*pcl.Reset(); Map.ResetLoct(); Stat.Reset(); ;*/ };
            Client.reset = () => { };//Map.Reset(); };
            pcl.reset = () => { };
            Stat.reset = () => { /*Map.Reset();*/ };
            DroneFrame.NavigationService.Navigate(Drn);
            ParcelFrame.NavigationService.Navigate(pcl);
            MapFrame.NavigationService.Navigate(Map);
            CostumerFrame.NavigationService.Navigate(Client);
            StationFrame.NavigationService.Navigate(Stat);
            MapTasker = new BackgroundWorker();
            MapTasker.WorkerReportsProgress = true ;
            MapTasker.RunWorkerCompleted += (x,y) => { };
            
            MapTasker.DoWork += (x,y) => {
                while (true)
                {
                    Thread.Sleep(100);
                    Map.ResetLoct();
                    MapTasker.ReportProgress(1);
                }
            };
            MapTasker.ProgressChanged += (x, y) => { Map.MyMapControl.Refresh(); };
            MapTasker.RunWorkerAsync();
            GraphTasker = new BackgroundWorker();
            GraphTasker.WorkerReportsProgress = true;
            GraphTasker.RunWorkerCompleted += (x, y) => { };

            GraphTasker.DoWork += (x, y) => {
                while (true)
                {
                    Thread.Sleep(1000);
                    pcl.PopulateResetScottPlot().ContinueWith( (x)=> { GraphTasker.ReportProgress(1); }) ;
                    Drn.ResetPlots().ContinueWith((x)=> { GraphTasker.ReportProgress(2); });
                   
                }
            };
            GraphTasker.ProgressChanged += (x, y) => {
                if (y.ProgressPercentage== 1)
                {
                    pcl.WpfPlotPack1.Render();
                    pcl.WpfPlotPack2.Render();
                    pcl.WpfPlotPack3.Render();
                }
                if (y.ProgressPercentage == 2)
                {
                    Drn.WpfPlot1.Render();
                    Drn.WpfPlot2.Render();
                    Drn.WpfPlot3.Render();
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
