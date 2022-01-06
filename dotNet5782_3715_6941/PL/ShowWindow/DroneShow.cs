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

        #region DroneShow
        BO.Drone drn;
        public Window2(BlApi.Ibl log, BO.Drone drn)
        {
            bool valid = true;
            InitializeComponent();
            this.drn = drn;
            this.log = log; 
            Add.Visibility = Visibility.Hidden;
            Show.Visibility = Visibility.Visible;
            this.DataContext = drn;
            if (!(drn.ParcelTransfer is null))
            {

                valid = true;
                if (!File.Exists(TMP + @"image" +drn.ParcelTransfer.Target.id + ".png"))
                    valid = Window2.SaveImage("https://thispersondoesnotexist.com/image", TMP + @"image" + drn.ParcelTransfer.Target.id + ".png", ImageFormat.Png);
                if (valid)
                    Photo1.Source = new BitmapImage(new Uri(TMP + @"image" + drn.ParcelTransfer.Target.id + ".png"));
                valid = true;
                if (!File.Exists(TMP + @"image" + drn.ParcelTransfer.Sender.id + ".png"))
                    valid = Window2.SaveImage("https://thispersondoesnotexist.com/image", TMP + @"image" + drn.ParcelTransfer.Sender.id + ".png", ImageFormat.Png, TMP + @"image" + drn.ParcelTransfer.Target.id + ".png");
                if (valid)
                    Photo2.Source = new BitmapImage(new Uri(TMP + @"image" + drn.ParcelTransfer.Sender.id + ".png"));

            }
                if (!File.Exists(TMP + @"image" + drn.Model.Replace(" ", "_") + ".png"))
                    valid = Window2.SaveFirstImage(drn.Model);
                if (valid)
                {
                   Photo0.Source = new BitmapImage(new Uri(TMP + @"image" + drn.Model.Replace(" ", "_") + ".png"));
                }



        }



        #endregion







    }





}