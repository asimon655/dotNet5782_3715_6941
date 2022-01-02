using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
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
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for ParcelShow.xaml
    /// </summary>
    public partial class ParcelShow : Window
    {
        BlApi.Ibl dat;
        static internal string TMP = System.IO.Path.GetTempPath();
        public ParcelShow(BlApi.Ibl dat, BO.Parcel parcely)
        {
     
            this.dat = dat;
            InitializeComponent();
            this.DataContext = parcely;
            bool valid = true;
            if (!File.Exists(TMP + @"image" + parcely.SenderParcelToCostumer.id + ".png"))
                valid = Window2.SaveImage("https://thispersondoesnotexist.com/image", TMP + @"image" + parcely.SenderParcelToCostumer.id + ".png", ImageFormat.Png);
            if (valid)
                Sender.Source = new BitmapImage(new Uri(TMP + @"image" + parcely.SenderParcelToCostumer.id + ".png"));
            valid = true;
            if (!File.Exists(TMP + @"image" + parcely.GetterParcelToCostumer.id + ".png"))
                valid = Window2.SaveImage("https://thispersondoesnotexist.com/image", TMP + @"image" + parcely.GetterParcelToCostumer.id + ".png", ImageFormat.Png, TMP + @"image" + parcely.SenderParcelToCostumer.id + ".png");
            if (valid)
                Target.Source = new BitmapImage(new Uri(TMP + @"image" + parcely.GetterParcelToCostumer.id + ".png"));
            valid = true;
            if (!(parcely.ParcelDrone is null))
            {
                BO.Drone drn = dat.GetDrone(parcely.ParcelDrone.Id);
                if (!File.Exists(TMP + @"image" + drn.Model.Replace(" ", "_") + ".png"))
                    valid = Window2.SaveFirstImage(drn.Model);
                if (valid)
                {
                    Drone.Source = new BitmapImage(new Uri(TMP + @"image" + drn.Model.Replace(" ", "_") + ".png"));
                }
            }


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new CostumerShow(dat, dat.GetCostumer((DataContext as BO.Parcel).GetterParcelToCostumer.id)).Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new CostumerShow(dat, dat.GetCostumer((DataContext as BO.Parcel).SenderParcelToCostumer.id)).Show();

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if ((DataContext as BO.Parcel).ParcelDrone is null)
                MessageBox.Show("There is no drone that binded", "Alert");
            else
                new Window2(dat, dat.GetDrone((DataContext as BO.Parcel).ParcelDrone.Id)).Show();
        }




    }
}
