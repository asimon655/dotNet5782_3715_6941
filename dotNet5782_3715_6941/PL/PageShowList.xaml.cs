using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for PageShowList.xaml
    /// </summary>
    public partial class PageShowList : Page
    {
  
            public IBL.Ibl a { set; get; }
            public PageShowList (IBL.Ibl x)
            {
                a = x;
                InitializeComponent();
                ListOf.ItemsSource = a.DronesPrint();
                StatusSelector.Text = "ALL";
                StatusSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.DroneStatuses));

            }

            private void Button_Click(object sender, RoutedEventArgs e)
            {

                ListOf.ItemsSource = a.DronesPrint();

            }

            private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                

            }

            private void ListOf_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {

            }

            private void Button_Click_1(object sender, RoutedEventArgs e)
            {
               
            }

        }
    }

