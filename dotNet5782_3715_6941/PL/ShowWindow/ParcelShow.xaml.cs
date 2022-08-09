using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
namespace PL
{
    /// <summary>
    /// Interaction logic for ParcelShow.xaml
    /// </summary>
    public partial class ParcelShow : Window
    {
        internal void MetaDataCstReset(System.Windows.Controls.Image PhotoSender, System.Windows.Controls.Image PhotoTarget, int SenderId, int TargetId) //Apply   PhotoAsync.SaveImageAsync when the Task (Like Promise in js) returned and if the file Already Exsists it Apply it directly 
        {
            if (!File.Exists(PhotoAsync.makePath(TargetId)))
            {
                PhotoAsync.SaveImageAsync(PhotoAsync.FaceAIURL, PhotoAsync.makePath(TargetId), PhotoAsync.fileEndEnum).ContinueWith(x =>
                {
                    if (x.Result)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            try
                            {
                                PhotoTarget.Source = new BitmapImage(new Uri(PhotoAsync.makePath(TargetId)));
                            }
                            catch { }
                        });
                    }
                    else
                        Dispatcher.Invoke(() => PhotoTarget.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\PL\Images\NoImage.jpg")));

                    #region Only when the First Request Returned - to prevent Double smae photo to the clients 
                    if (!File.Exists(PhotoAsync.makePath(SenderId)))
                    {
                        PhotoAsync.SaveImageAsync(PhotoAsync.FaceAIURL, PhotoAsync.makePath(SenderId), PhotoAsync.fileEndEnum, PhotoAsync.makePath(TargetId)).ContinueWith(x =>
                        {
                            if (x.Result)
                            {
                                Dispatcher.Invoke(() =>
                                {
                                    try
                                    {
                                        PhotoSender.Source = new BitmapImage(new Uri(PhotoAsync.makePath(SenderId)));
                                    }
                                    catch { }
                                });
                            }
                            else
                                Dispatcher.Invoke(() => PhotoSender.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\PL\Images\NoImage.jpg")));
                        });
                    }
                    else
                    {
                        try
                        {
                            PhotoSender.Source = new BitmapImage(new Uri(PhotoAsync.makePath(SenderId)));
                        }
                        catch { }
                    } 
                    #endregion
                });
            }
            else
            {
                PhotoTarget.Source = new BitmapImage(new Uri(PhotoAsync.makePath(TargetId)));
                if (!File.Exists(PhotoAsync.makePath(SenderId))) // if file 1 is alrdeay exsists try for file 2 
                {
                    PhotoAsync.SaveImageAsync(PhotoAsync.FaceAIURL, PhotoAsync.makePath(SenderId), PhotoAsync.fileEndEnum, PhotoAsync.makePath(TargetId)).ContinueWith(x =>
                    {
                        if (x.Result)
                        {
                            Dispatcher.Invoke(() =>
                            {
                                try
                                {
                                    PhotoSender.Source = new BitmapImage(new Uri(PhotoAsync.makePath(SenderId)));
                                }
                                catch { }
                            });
                        }
                        else
                            Dispatcher.Invoke(() => PhotoSender.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\PL\Images\NoImage.jpg")));
                    });
                }
                else
                {
                    PhotoSender.Source = new BitmapImage(new Uri(PhotoAsync.makePath(SenderId)));
                }
            }
        } 

        private readonly BlApi.Ibl dat;
        BO.Parcel pcl; 
        internal static string TMP = System.IO.Path.GetTempPath();
        public ParcelShow(BlApi.Ibl dat)
        {
            this.dat = dat;
            InitializeComponent();
            Add.Visibility = Visibility.Visible;
            Show.Visibility = Visibility.Hidden;
            Array SenderIds = (from sender in dat.GetCustomers() select sender.Id).ToArray();
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
            this.pcl = parcely;
            Add.Visibility = Visibility.Hidden;
            Show.Visibility = Visibility.Visible;
            DataContext = parcely;
            if (!(parcely.ParcelDrone is null))
            {
                BO.Drone drn = dat.GetDrone(parcely.ParcelDrone.Id);


                if (!File.Exists(TMP + @"image" + drn.Model.Replace(" ", "_") + ".png"))
                {
                    PhotoAsync.SaveFirstImageAsync(drn.Model).ContinueWith(x =>
                    {
                        if (x.Result)
                        {
                            Dispatcher.Invoke(() =>
                            {
                                Drone.Source = new BitmapImage(new Uri(PhotoAsync.makePath(drn.Model)));
                            });
                        }
                        else
                            Dispatcher.Invoke(() => Drone.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\PL\Images\NoImage.jpg")));
                    });
                }
                else
                {
                    Drone.Source = new BitmapImage(new Uri(TMP + @"image" + drn.Model.Replace(" ", "_") + ".png"));
                }
            }
            MetaDataCstReset(Sender, Target, parcely.SenderParcelToCostumer.id, parcely.GetterParcelToCostumer.id);


        }

        private void CstShowTarget(object sender, RoutedEventArgs e)
        {
            new CostumerShow(dat, dat.GetCostumer((DataContext as BO.Parcel).GetterParcelToCostumer.id)).Show();
        }

        private void CstShowSender(object sender, RoutedEventArgs e)
        {
            new CostumerShow(dat, dat.GetCostumer((DataContext as BO.Parcel).SenderParcelToCostumer.id)).Show();

        }

        private void DroneShow(object sender, RoutedEventArgs e)
        {
            if ((DataContext as BO.Parcel).ParcelDrone is null)
            {
                MessageBox.Show("There is no drone that binded", "Alert");
            }
            else
            {
                new Window2(dat, dat.GetDrone((DataContext as BO.Parcel).ParcelDrone.Id)).Show();
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
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
                Close();


            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error");

            }
        }

 

   
    }
}
