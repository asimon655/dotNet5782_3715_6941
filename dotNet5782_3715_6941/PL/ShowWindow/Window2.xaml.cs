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

        List<Object> pacads = new List<object>(); 
        BlApi.Ibl log;
        Page pageof;
        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            try
            {
                BO.Drone drony = new BO.Drone();

                drony.Model = NameTB .Text;
                drony.Id = Convert.ToInt32(IdTB.Text);
                drony.Weight = (BO.WeightCategories)(WeightCB).SelectedItem;


                log.AddDrone(drony, (int)(StatCB).SelectedItem);
                try
                {
                    if (pageof is MainWindow)
                        (pageof as MainWindow).ListOf.ItemsSource = log.GetDronesFiltered((pageof as MainWindow).Stat, (pageof as MainWindow).Weight);
                 
                }
                catch { }

               
            }
            catch (Exception err)
            {

                MessageBox.Show(err.ToString(), "Exception", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            finally
            {
                this.Close();
            }
            
        }
        private void Button_Click_Show(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!(drn.ParcelTransfer is null))
                new ParcelShow(log, log.GetParcel(drn.ParcelTransfer.Id)).Show(); 
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!(drn.ParcelTransfer is null))
            new CostumerShow(log, log.GetCostumer(drn.ParcelTransfer.Target.id)).Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (!(drn.ParcelTransfer is null))
                new CostumerShow(log, log.GetCostumer(drn.ParcelTransfer.Sender.id)).Show();
           
        }


    }
}
