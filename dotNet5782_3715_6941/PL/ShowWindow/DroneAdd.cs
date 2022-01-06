using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
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
using System.Windows.Shapes;
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
            this.log = x; 
            InitializeComponent();
            Add.Visibility = Visibility.Visible;
            Show.Visibility = Visibility.Hidden;
            Array WeightVals = Enum.GetValues(typeof(BO.WeightCategories));
            Array StatIds = (from Stat in log.GetStations() select Stat.Id).ToArray();
            WeightCB.ItemsSource = WeightVals;
            StatCB.ItemsSource = StatIds;

        }
    }
    #endregion
}