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

namespace PL_Client_edition
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>

    public partial class LoginWindow : SparkWindow
    {
        BlApi.Ibl dat;
        public LoginWindow()
        {

            InitializeComponent();
            this.dat = BlApi.BlFactory.GetBl();

        }
        public LoginWindow(BlApi.Ibl dat)
        {

            InitializeComponent();
            this.dat =dat;

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int clientId;

            if (Int32.TryParse(UserName.Text, out clientId))
            {
                try
                {
                    BO.Customer cst = dat.GetCostumer(clientId);
                    new Main(dat,cst.Id).Show();
                    this.Close();
                }
                catch (Exception err)
                {

                    MessageBox.Show(err.Message, "Error");
                }
                
            
            } 
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {

            new AddClient(dat).Show();
           
        }
    }
}
