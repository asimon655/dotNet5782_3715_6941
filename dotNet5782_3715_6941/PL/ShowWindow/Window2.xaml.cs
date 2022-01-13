using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

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
        private void Button_Click_Add(object sender, RoutedEventArgs e)
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
        private void Button_Click_Show(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!(drn.ParcelTransfer is null))
            {
                new ParcelShow(log, log.GetParcel(drn.ParcelTransfer.Id)).Show();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!(drn.ParcelTransfer is null))
            {
                new CostumerShow(log, log.GetCostumer(drn.ParcelTransfer.Target.id)).Show();
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (!(drn.ParcelTransfer is null))
            {
                new CostumerShow(log, log.GetCostumer(drn.ParcelTransfer.Sender.id)).Show();
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            myPopup2.IsOpen = false;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            myPopup3.IsOpen = false;
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            myPopup1.IsOpen = false;
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            myPopup4.IsOpen = false;
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            myPopup0.IsOpen = false;
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            try {
                log.UpdateDrone(drn.Id, WCEB.Text);
            
            } catch(Exception err) {
                MessageBox.Show(err.Message, "Error");
            
            }
        }
    }
}
