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
    public partial class ManngerWin : SparkWindow
    {

        #region Fields
        public Stat status;
        BlApi.Ibl dat;
        #endregion

        #region Ctor 
        public ManngerWin(BlApi.Ibl dat)
        {
            this.dat = dat;
            InitializeComponent();
            #region Framses-Initialize 
            DroneFrame.NavigationService.Navigate(new DroneTab(dat));
            ParcelFrame.NavigationService.Navigate(new ParcelTab(dat));
            MapFrame.NavigationService.Navigate(new MapTab(dat));
            CostumerFrame.NavigationService.Navigate(new ClientsTab(dat));
            StationFrame.NavigationService.Navigate(new StaionsTab(dat));
            #endregion
        }
        #endregion

        #region Buttons Functions
        // reset button action


        #endregion
    }
}
