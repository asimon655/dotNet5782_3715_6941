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
        internal void MetaDataCstReset(System.Windows.Controls.Image Photo1, System.Windows.Controls.Image Photo2, int SenderId, int TargetId)
        {
            if (!File.Exists(PhotoAsync.makePath(TargetId)))
                PhotoAsync.SaveImageAsync(PhotoAsync.FaceAIURL, PhotoAsync.makePath(TargetId), PhotoAsync.fileEndEnum).ContinueWith(x => {
                    if (x.Result)
                        Dispatcher.Invoke(() =>
                        {
                            Photo1.Source = new BitmapImage(new Uri(PhotoAsync.makePath(TargetId)));
                        });
                });
            else
                Photo1.Source = new BitmapImage(new Uri(PhotoAsync.makePath(TargetId)));
            if (!File.Exists(PhotoAsync.makePath(SenderId)))
                PhotoAsync.SaveImageAsync(PhotoAsync.FaceAIURL, PhotoAsync.makePath(SenderId), PhotoAsync.fileEndEnum, PhotoAsync.makePath(TargetId)).ContinueWith(x => {
                    if (x.Result)

                        Dispatcher.Invoke(() =>
                        {
                            Photo2.Source = new BitmapImage(new Uri(PhotoAsync.makePath(SenderId)));
                        });
                });
            else
                Photo2.Source = new BitmapImage(new Uri(PhotoAsync.makePath(SenderId)));


        }
  
        BlApi.Ibl dat;
        static internal string TMP = System.IO.Path.GetTempPath();
        public ParcelShow(BlApi.Ibl dat)
            {
            this.dat = dat;
            InitializeComponent();
            Add.Visibility = Visibility.Visible;
            Show.Visibility = Visibility.Hidden;
            Array SenderIds = (from sender  in dat.GetCustomers() select sender.Id).ToArray();
            Array TargetIds = (from sender in dat.GetCustomers() select sender.Id).ToArray();
            Array WeightVals = Enum.GetValues(typeof(BO.WeightCategories));
            Array PrioVals = Enum.GetValues(typeof(BO.Priorities));
            SIdCB.ItemsSource = SenderIds;
            TIdCB.ItemsSource = TargetIds;
            WeightCB.ItemsSource = WeightVals;
            PrioCB.ItemsSource = PrioVals; 



        }
            public ParcelShow(BlApi.Ibl dat, BO.Parcel parcely)
        {
     
            this.dat = dat;
            InitializeComponent();
            Add.Visibility = Visibility.Hidden;
            Show.Visibility = Visibility.Visible;
            this.DataContext = parcely;
            if (!(parcely.ParcelDrone is null ))
            {
                BO.Drone drn = dat.GetDrone(parcely.ParcelDrone.Id);
            

                if (!File.Exists(TMP + @"image" + drn.Model.Replace(" ", "_") + ".png"))
                    PhotoAsync.SaveFirstImageAsync(drn.Model).ContinueWith(x =>
                    {
                        if (x.Result)

                            Dispatcher.Invoke(() =>
                            {
                                Drone.Source = new BitmapImage(new Uri(PhotoAsync.makePath(drn.Model)));
                            });
                    });
                else
                {
                    Drone.Source = new BitmapImage(new Uri(TMP + @"image" + drn.Model.Replace(" ", "_") + ".png"));
                }
            }
            MetaDataCstReset(  Sender, Target, parcely.GetterParcelToCostumer.id, parcely.SenderParcelToCostumer.id);


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

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                BO.Parcel add = new BO.Parcel()
                {
                    Priority = (BO.Priorities)PrioCB.SelectedItem,
                    Weight = (BO.WeightCategories)WeightCB.SelectedItem,
                    SenderParcelToCostumer = new BO.CustomerInParcel()
                    {
                        id = (int)SIdCB.SelectedItem
                    },
                    GetterParcelToCostumer = new BO.CustomerInParcel()
                    {
                        id = (int)TIdCB.SelectedItem
                    },
                };

                dat.AddParcel(add);
                this.Close();


            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error"); 
            
            }
        }
    }
}
