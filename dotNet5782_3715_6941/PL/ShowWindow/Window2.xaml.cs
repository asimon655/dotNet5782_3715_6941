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

                drony.Model = ((pacads[0] as Viewbox).Child as TextBox).Text;
                drony.Id = Convert.ToInt32((((pacads[1] as Viewbox).Child as TextBox).Text));
                drony.Weight = (BO.WeightCategories)(pacads[2] as ComboBox).SelectedItem;


                log.AddDrone(drony, (int)(pacads[3] as ComboBox).SelectedItem);
                (pageof as MainWindow).ListOf.ItemsSource = log.GetDronesFiltered((pageof as MainWindow).Stat, (pageof as MainWindow).Weight);
                try
                {
                    pageof.NavigationService.Refresh();
                 
                }
                catch { }
                IEnumerable<BO.DroneList> Dronelst = log.GetDrones();
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
        private void GifLoader_Unloaded(object sender, RoutedEventArgs e)
        {


            MediaElement media = (MediaElement)sender;
            media.LoadedBehavior = MediaState.Manual;
            media.Position = TimeSpan.FromMilliseconds(1);
            media.Play();



        }

    }
}
