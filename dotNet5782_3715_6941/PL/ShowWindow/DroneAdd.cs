using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
namespace PL
{

    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        #region addDrone
        public Window2(BlApi.Ibl x, Page pg)
        {
            pageof = pg;
            log = x;

            windowType = WindowType.add;
            InitializeComponent();
            Add.Visibility = Visibility.Visible;
            Show.Visibility = Visibility.Hidden;
            Array WeightVals = Enum.GetValues(typeof(BO.WeightCategories));
            Array StatIds = (from Stat in log.GetStationsWithFreePorts() select Stat.Id).ToArray();
            WeightCB.ItemsSource = WeightVals;
            StatCB.ItemsSource = StatIds;

        }
    }
    #endregion
}