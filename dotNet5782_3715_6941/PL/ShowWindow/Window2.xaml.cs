using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace PL
{




    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        private readonly List<object> pacads = new List<object>();
        private readonly BlApi.Ibl log;
        private readonly Page pageof;
        private void PopUpShow_Add(object sender, RoutedEventArgs e)
        {
            try
            {
                BO.Drone drony = new BO.Drone
                {
                    Model = NameTB.Text,
                    Id = Convert.ToInt32(IdTB.Text),
                    Weight = (BO.WeightCategories)(WeightCB).SelectedItem
                };


                log.AddDrone(drony, (int)(StatCB).SelectedItem);



            }
            catch (Exception err)
            {

                MessageBox.Show(err.ToString(), "Exception", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            finally
            {
                Close();
            }

        }
        private void PopUpShow_Show(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private void PopUpShow(object sender, RoutedEventArgs e)
        {
            if (!(drn.ParcelTransfer is null))
            {
                new ParcelShow(log, log.GetParcel(drn.ParcelTransfer.Id)).Show();
            }
        }

        private void PopUpShow_1(object sender, RoutedEventArgs e)
        {
            if (!(drn.ParcelTransfer is null))
            {
                new CostumerShow(log, log.GetCostumer(drn.ParcelTransfer.Target.id)).Show();
            }
        }

        private void PopUpShow_2(object sender, RoutedEventArgs e)
        {
            if (!(drn.ParcelTransfer is null))
            {
                new CostumerShow(log, log.GetCostumer(drn.ParcelTransfer.Sender.id)).Show();
            }
        }

        private void PopUpShow_3(object sender, RoutedEventArgs e)
        {
            myPopup2.IsOpen = false;
        }

        private void PopUpShow_4(object sender, RoutedEventArgs e)
        {
            myPopup3.IsOpen = false;
        }

        private void PopUpShow_5(object sender, RoutedEventArgs e)
        {
            myPopup1.IsOpen = false;
        }

        private void PopUpShow_6(object sender, RoutedEventArgs e)
        {
            myPopup4.IsOpen = false;
        }

        private void PopUpShow_7(object sender, RoutedEventArgs e)
        {
            myPopup0.IsOpen = false;
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            try {
                log.UpdateDrone(drn.Id, WCEB.Text);

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
                        else
                            Dispatcher.Invoke(() => Photo0.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\PL\Images\NoImage.jpg")));
                    });
                }
                else
                {
                    try
                    {
                        Photo0.Source = new BitmapImage(new Uri(PhotoAsync.makePath(drn.Model)));
                    }
                    catch { }
                }

            } catch(Exception err) {
                MessageBox.Show(err.Message, "Error");
            
            }
        }

        private void WCEB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (WCEB.Text == drn.Model.ToString())
            {
                Update.IsEnabled = false;
                UpdateCol.Width = new GridLength(0, GridUnitType.Star);
            }
            else
            {
                Update.IsEnabled = true;
                UpdateCol.Width = new GridLength(1, GridUnitType.Star);
            }
        }
    }
}
