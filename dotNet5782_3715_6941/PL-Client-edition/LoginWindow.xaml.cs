using System;
using System.Windows;
using WPFSpark;

namespace PL_Client_edition
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>

    public partial class LoginWindow : SparkWindow
    {
        private readonly BlApi.Ibl dat;
        public LoginWindow()
        {

            InitializeComponent();
            dat = BlApi.BlFactory.GetBl();

        }
        public LoginWindow(BlApi.Ibl dat)
        {

            InitializeComponent();
            this.dat = dat;

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int clientId;

            if (int.TryParse(UserName.Text, out clientId))
            {
                try
                {
                    BO.Customer cst = dat.GetCostumer(clientId);
                    new Main(dat, cst.Id).Show();
                    Close();
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
