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
                    if (!File.Exists(TMP + @"image" + drn.ParcelTransfer.Target.id + ".png"))
                        SaveImageAsync("https://thispersondoesnotexist.com/image", TMP + @"image" + drn.ParcelTransfer.Target.id + ".png", ImageFormat.Png).ContinueWith( x => { if (x.Result) {
                                Dispatcher.Invoke(() =>
                                {
                                    Photo1.Source = new BitmapImage(new Uri(TMP + @"image" + drn.ParcelTransfer.Target.id + ".png"));
                                });  } } );
                    else 
                        Photo1.Source = new BitmapImage(new Uri(TMP + @"image" + drn.ParcelTransfer.Target.id + ".png"));
                    if (!File.Exists(TMP + @"image" + drn.ParcelTransfer.Sender.id + ".png"))
                        SaveImageAsync("https://thispersondoesnotexist.com/image", TMP + @"image" + drn.ParcelTransfer.Sender.id + ".png", ImageFormat.Png, TMP + @"image" + drn.ParcelTransfer.Target.id + ".png").ContinueWith(x => { if (x.Result) {
                                Dispatcher.Invoke(() =>
                                {
                                    Photo2.Source = new BitmapImage(new Uri(TMP + @"image" + drn.ParcelTransfer.Sender.id + ".png"));
                                }); } });
                    else 
                        Photo2.Source = new BitmapImage(new Uri(TMP + @"image" + drn.ParcelTransfer.Sender.id + ".png"));


                }
            }
   


        }

        #endregion
        #region Populate




         async Task<bool> SaveImageAsync(string imageUrl, string filename, ImageFormat format, String? FileOther = null)
        {
          
            Thread.Sleep(10);

            WebClient client = new WebClient();

            client = new WebClient();
            try
            {
                Stream stream = await client.OpenReadTaskAsync(imageUrl);
                Bitmap bitmap; bitmap = new Bitmap(stream);

                if (bitmap != null)
                {
                    bitmap.Save(filename, format);
                }

                stream.Flush();
                stream.Close();
                client.Dispose();
                if (!(FileOther is null))
                {
                    if (AreEqule(filename, FileOther))
                    {
                        File.Delete(filename);
                         await SaveImageAsync(imageUrl, filename, format, FileOther);
                    }

                }
            }
            catch
            {

             
                return false;
            }
            return true;
        }


        #endregion

        #region DroneShow
        BO.Drone drn;
        Action reset;
        BackgroundWorker backgroundWorker1;
        bool stop = false;
        public Window2(BlApi.Ibl log, BO.Drone drn , Action  ? action = null)
        {
            InitializeComponent();
            backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.ProgressChanged += (s, e) => { Reset(); reset(); };
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
            MetaDataCstReset();

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
        private void Simulator_Checked(object sender, RoutedEventArgs e)
        {
            stop = true;
            if (!backgroundWorker1.IsBusy)
                backgroundWorker1.RunWorkerAsync();
        }

        private void Simulator_Unchecked(object sender, RoutedEventArgs e)
        {
            stop = false;
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Action refresh = () => backgroundWorker1.ReportProgress(1);
            log.StartSimulator(drn.Id, refresh, () => stop);
        }
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            object result = e.Result;
        }


        #endregion


    }





}