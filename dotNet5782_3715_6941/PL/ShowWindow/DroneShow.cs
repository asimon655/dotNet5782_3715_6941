using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        #region MetaDataReset
        void Reset()
        {
            this.drn = log.GetDrone(drn.Id);
            this.DataContext = drn;
            if (!(drn.ParcelTransfer is null))
                ParcelOpsRFS(log.GetParcel(drn.ParcelTransfer.Id));
            else
                ParcelOpsRFS(null);


        }
        void MetaDataCstReset(BO.Drone drn = null )
        {
            if (drn is null) {
                Photo1.Source = new BitmapImage(new Uri("https://thumbs.dreamstime.com/b/default-avatar-profile-icon-vector-social-media-user-photo-183042379.jpg"));
                Photo2.Source = new BitmapImage(new Uri("https://thumbs.dreamstime.com/b/default-avatar-profile-icon-vector-social-media-user-photo-183042379.jpg"));

            }
            else
            {
                if (!(drn.ParcelTransfer is null))
                {
                    bool valid = true;
                    if (!File.Exists(TMP + @"image" + drn.ParcelTransfer.Target.id + ".png"))
                        valid = Window2.SaveImage("https://thispersondoesnotexist.com/image", TMP + @"image" + drn.ParcelTransfer.Target.id + ".png", ImageFormat.Png);
                    if (valid)
                        Photo1.Source = new BitmapImage(new Uri(TMP + @"image" + drn.ParcelTransfer.Target.id + ".png"));
                    valid = true;
                    if (!File.Exists(TMP + @"image" + drn.ParcelTransfer.Sender.id + ".png"))
                        valid = Window2.SaveImage("https://thispersondoesnotexist.com/image", TMP + @"image" + drn.ParcelTransfer.Sender.id + ".png", ImageFormat.Png, TMP + @"image" + drn.ParcelTransfer.Target.id + ".png");
                    if (valid)
                        Photo2.Source = new BitmapImage(new Uri(TMP + @"image" + drn.ParcelTransfer.Sender.id + ".png"));

                }
            }
   


        }

        #endregion
        #region DroneShow
        BO.Drone drn;
        Action reset;
        BackgroundWorker backgroundWorker1;
        public Window2(BlApi.Ibl log, BO.Drone drn , Action  ? action = null 
        )
        {
            InitializeComponent();
            this.backgroundWorker1 = new BackgroundWorker();
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerAsync(1);
            bool valid = true;
            this.drn = drn;
            MetaDataCstReset(drn);
            this.log = log;
            Add.Visibility = Visibility.Hidden;
            Show.Visibility = Visibility.Visible;
            this.DataContext = drn;
            reset = action; 

            if (!(drn.ParcelTransfer is null))
                ParcelOpsRFS(log.GetParcel(drn.ParcelTransfer.Id));
            else
                ParcelOpsRFS(null);
            if (!File.Exists(TMP + @"image" + drn.Model.Replace(" ", "_") + ".png"))
                valid = Window2.SaveFirstImage(drn.Model);
            if (valid)
            {
                Photo0.Source = new BitmapImage(new Uri(TMP + @"image" + drn.Model.Replace(" ", "_") + ".png"));
            }

        }



        #endregion


        #region Operations

        private void PopupShiwiw(object sender, RoutedEventArgs e)
        {
            
            if (!(drn.ParcelTransfer is null))
                ParcelOpsRFS(log.GetParcel(drn.ParcelTransfer.Id));
            else
                ParcelOpsRFS(null);

        }
        
        private ParcelO ParcelC(BO.Parcel parcel)
        {

            int caseNum = -1;
            if (!(parcel.ParcelCreation is null))
            {
                caseNum++;
                if (!(parcel.ParcelBinded is null))
                {
                    caseNum++;
                    if (!(parcel.ParcelPickedUp is null))
                    {
                        caseNum++;
                        if (!(parcel.ParcelDelivered is null))
                        {
                            caseNum++;
                        }
                    }
                }
            }
            if (caseNum < 1)
                throw new Exception("the parcel is not even decleared ");
            return (ParcelO)caseNum;

        }


        private void ParcelOpsRFS(BO.Parcel? pcl)
        {

            /// operation0 - Bind 
            /// operation 3 deliver
            /// operation2 pickup
            /// operation 1 charge 
            /// operation4 realse 
            /// 

            Opeation0.IsEnabled = false;
            Opeation1.IsEnabled = false;
            Opeation2.IsEnabled = false;
            Opeation3.IsEnabled = false;
            Opeation4.IsEnabled = false;
           
            if (pcl is null) /// parcel only created and never binded 
            {
                if (drn.DroneStat == BO.DroneStatuses.Free)
                {
                    Opeation0.Background = System.Windows.Media.Brushes.Green;
                    Opeation0.IsEnabled = true;
                    Opeation1.IsEnabled = true;
                    Opeation1.Background = System.Windows.Media.Brushes.LightPink;
                }
                if (drn.DroneStat == BO.DroneStatuses.Matance)
                {
                    Opeation4.IsEnabled = true;
                    Opeation4.Background = System.Windows.Media.Brushes.Red;
                }

            }
            else
            {
                ParcelO Stat = ParcelC(pcl);


                if (Stat == ParcelO.Deliver)
                {
                    //No further more operations
                }
                if (Stat == ParcelO.Bind)
                {
                    Opeation2.Background = System.Windows.Media.Brushes.LightBlue;
                    Opeation2.IsEnabled = true;

                }
                if (Stat == ParcelO.PickUp)
                {
                    Opeation3.Background = System.Windows.Media.Brushes.MediumVioletRed;
                    Opeation3.IsEnabled = true;
                }
           
            }
            if(!(reset is null))
                reset();

        }
        private void Bind(object sender, RoutedEventArgs e)
        {
            try
            {
                log.BindParcelToDrone(drn.Id);
                drn = log.GetDrone(drn.Id);
               this.DataContext = drn;
                MetaDataCstReset(drn);
                ParcelOpsRFS(log.GetParcel(drn.ParcelTransfer.Id));
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error");
            }
            finally
            {
                myPopup0.IsOpen = false;


            }

        }
        private void PickUp(object sender, RoutedEventArgs e)
        {
            try
            {
                log.DronePickUp(drn.Id);
                drn = log.GetDrone(drn.Id);
               this.DataContext = drn;
                ParcelOpsRFS(log.GetParcel(drn.ParcelTransfer.Id));
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error");
            }
            finally
            {
                myPopup2.IsOpen = false;


            }
        }
        private void Deliver(object sender, RoutedEventArgs e)
        {
            try
            {
                log.DroneDelivere(drn.Id);
               drn = log.GetDrone(drn.Id);
               this.DataContext = drn;
                MetaDataCstReset();
                ParcelOpsRFS(null);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error");
            }
            finally
            {
                myPopup3.IsOpen = false;


            }

        }
        private void Charge(object sender, RoutedEventArgs e)
        {
            try
            {
                log.DroneCharge(drn.Id);
                drn = log.GetDrone(drn.Id);
               this.DataContext = drn;
                ParcelOpsRFS(null);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error");
            }
            finally
            {
                myPopup1.IsOpen = false;


            }

        }
        private void Realse(object sender, RoutedEventArgs e)
        {
            try
            {
                log.DroneReleaseCharge(drn.Id,  Double.Parse(TBREALSE.Text));
                drn = log.GetDrone(drn.Id);
                ParcelOpsRFS(null);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error");
            }
            finally
            {
                myPopup4.IsOpen = false;


            }

        }


        #endregion
        #region Simulator 
        bool SimulatorStat()
        { return (bool)Simulator.IsChecked;  }
        private void Simulator_Checked(object sender, RoutedEventArgs e)
        {
            

        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                log.StartSimulator(drn.Id, ()=> { Reset(); }, SimulatorStat);
            });
          
        }
        #endregion


    }





}