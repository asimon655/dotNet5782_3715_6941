using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Threading;
using WPFSpark;

namespace PL
{
    /// <summary>
    /// Interaction logic for LogIn.xaml
    /// </summary>
    public partial class LogIn : SparkWindow
    {
        
        public LogIn()
        {
            InitializeComponent();
        }
        BlApi.Ibl dat; 
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            dat = BlApi.BlFactory.GetBl();

            //if (Password.Password == "Golo1256" && UserName.Text == "idangolo123@gmail.com")
            if (UserName.Text == "admin")
            {
                new ManngerWin(dat).Show();
                this.Close();
            }
            //else if (Password.Password == "WeakPassword" && UserName.Text == "Asimon@gmail.com")
            else if (UserName.Text != "admin")
            {
                new Window3(dat).Show();
                this.Close();
            }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            new CostumerShow(dat).Show();
        }
    }
}
