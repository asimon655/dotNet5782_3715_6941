using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace PL
{

    enum WindowType
    {
        show,
        add
    };


    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        

        #region MetaDataReset
        private void Reset()
        {
            drn = log.GetDrone(drn.Id);
            DataContext = drn;
            if (!(drn.ParcelTransfer is null))
            {
                ParcelOpsRFS(log.GetParcel(drn.ParcelTransfer.Id));
            }
            else
            {
                ParcelOpsRFS(null);
            }
            if (!(drn.ParcelTransfer is null))
            {
                MetaDataCstReset(Photo1, Photo2, drn.ParcelTransfer.Sender.id, drn.ParcelTransfer.Target.id);
            }
        }
        internal void MetaDataCstReset(System.Windows.Controls.Image Photo1, System.Windows.Controls.Image Photo2, int SenderId, int TargetId)
        {
            if (!File.Exists(PhotoAsync.makePath(TargetId)))
            {
                PhotoAsync.SaveImageAsync(PhotoAsync.FaceAIURL, PhotoAsync.makePath(TargetId), PhotoAsync.fileEndEnum).ContinueWith(x =>
                {
                    if (x.Result)
                    {
                        Dispatcher.Invoke(() =>
                         {
                             Photo1.Source = new BitmapImage(new Uri(PhotoAsync.makePath(TargetId)));
                         });
                    }

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
                                        Photo2.Source = new BitmapImage(new Uri(PhotoAsync.makePath(SenderId)));
                                    }
                                    catch { }
                                });
                            }
                        });
                    }
                    else
                    {
                        try
                        {
                            Photo2.Source = new BitmapImage(new Uri(PhotoAsync.makePath(SenderId)));
                        }
                        catch { }
                    }
                });
            }
            else
            {
                try
                {
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
                                        Photo2.Source = new BitmapImage(new Uri(PhotoAsync.makePath(SenderId)));
                                    }
                                    catch { }
                                });
                            }
                        });
                    }
                    else
                    {
                        try
                        {
                            Photo2.Source = new BitmapImage(new Uri(PhotoAsync.makePath(SenderId)));
                        }
                        catch { }
                    }
                    Photo1.Source = new BitmapImage(new Uri(PhotoAsync.makePath(TargetId)));
                }
                catch { }
            }
        }

        #endregion
        #region Populate







        #endregion

        #region DroneShow
        private BO.Drone drn;
        private readonly Action reset;
        private readonly BackgroundWorker simulator;
        private bool exitPending = false;
        private WindowType windowType;

        public Window2(BlApi.Ibl log, BO.Drone drn, Action? action = null)
        {
            InitializeComponent();
            simulator = new BackgroundWorker();
            simulator.DoWork += backgroundWorker1_DoWork;
            simulator.WorkerSupportsCancellation = true;
            simulator.RunWorkerCompleted += Worker_RunWorkerCompleted;
            simulator.WorkerReportsProgress = true;
            simulator.ProgressChanged += backgroundWorker1_ProgressChanged;
            this.drn = drn;
            this.log = log;
            windowType = WindowType.show;
            Add.Visibility = Visibility.Hidden;
            Show.Visibility = Visibility.Visible;
            DataContext = drn;
            reset = action;

            if (!(drn.ParcelTransfer is null))
            {
                ParcelOpsRFS(log.GetParcel(drn.ParcelTransfer.Id));
            }
            else
            {
                ParcelOpsRFS(null);
            }

            if (!File.Exists(PhotoAsync.makePath(drn.Model)))
            {
                PhotoAsync.SaveFirstImageAsync(drn.Model).ContinueWith(x =>
                {
                    if (x.Result)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            Photo0.Source = new BitmapImage(new Uri(PhotoAsync.makePath(drn.Model)));
                        });
                    }
                });
            }
            else
            {
                Photo0.Source = new BitmapImage(new Uri(PhotoAsync.makePath(drn.Model)));
            }


            if (!(drn.ParcelTransfer is null))
            {
                MetaDataCstReset(Photo1, Photo2, drn.ParcelTransfer.Sender.id, drn.ParcelTransfer.Target.id);
            }
        }



        #endregion


        #region Operations

        private void PopupShiwiw(object sender, RoutedEventArgs e)
        {

            if (!(drn.ParcelTransfer is null))
            {
                ParcelOpsRFS(log.GetParcel(drn.ParcelTransfer.Id));
            }
            else
            {
                ParcelOpsRFS(null);
            }
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
            {
                throw new Exception("the parcel is not even decleared ");
            }

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
            if (!(reset is null))
            {
                reset();
            }
        }
        private void Bind(object sender, RoutedEventArgs e)
        {
            try
            {
                log.BindParcelToDrone(drn.Id);
                drn = log.GetDrone(drn.Id);
                DataContext = drn;
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
                DataContext = drn;
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
                DataContext = drn;

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
                DataContext = drn;
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
                log.DroneReleaseCharge(drn.Id, double.Parse(TBREALSE.Text));
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
            if (!simulator.IsBusy)
            {
                simulator.RunWorkerAsync();
            }
        }

        private void Simulator_Unchecked(object sender, RoutedEventArgs e)
        {
            simulator.CancelAsync();
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            log.StartSimulator(drn.Id,
                () => simulator.ReportProgress(1),
                () => simulator.CancellationPending);
        }
        private void backgroundWorker1_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            Reset(); //local reset
            reset(); // activate reset by the father window5
        }
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (exitPending)
            {
                Close();
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (windowType == WindowType.add)
                return;
            exitPending = true;
            if (simulator.IsBusy)
            {
                simulator.CancelAsync();
                e.Cancel = true;
                // present some loading thingy
            }
        }
        #endregion


    }





}