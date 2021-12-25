using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Security.Cryptography;
namespace PL
{
    /// <summary>
    /// Interaction logic for CostumerShow.xaml
    /// </summary>
    public partial class CostumerShow : Window
    {
        #region Fields 
        BlApi.Ibl dat;
        IEnumerable<string> Answers; 
        static internal string TMP = System.IO.Path.GetTempPath();
        #endregion
        public CostumerShow(BlApi.Ibl dat , BO.Customer cst)
        {
            this.dat = dat;
            InitializeComponent();
            List<String> resCaptcha = Window2.GetCapchaQuestion();
            myPopup.DataContext = resCaptcha.First();
            Answers = resCaptcha.Skip(1); 
            //CaptchaQuestion.Content = Window2.GetCapchaQuestion(); 
            this.DataContext = cst;
            #region ListView Grouping 

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(cst.ToClient);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Priority");
            view.GroupDescriptions.Add(groupDescription);
             view = (CollectionView)CollectionViewSource.GetDefaultView(cst.FromClient);
             groupDescription = new PropertyGroupDescription("Priority");
            view.GroupDescriptions.Add(groupDescription);
            #endregion
            bool valid = true;
            if (!File.Exists(TMP + @"image" + cst.Id + ".png"))
                valid = Window2.SaveImage("https://thispersondoesnotexist.com/image", TMP + @"image" + cst.Id + ".png", ImageFormat.Png);
            if (valid)
                CostumerPhoto.Source = new BitmapImage(new Uri(TMP + @"image" + cst.Id + ".png"));
        }
        private void ListOfPackges_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new ParcelShow(dat, dat.GetParcel(((sender as ListView).SelectedItem as BO.ParcelInCustomer).Id)).Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("HI", "HI");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            //List<String> resCaptcha = Window2.GetCapchaQuestion();
            //myPopup.DataContext = resCaptcha.First();
            //Answers = resCaptcha.Skip(1);


        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            myPopup.IsOpen = false; 
        }
        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
           
           
            string plaintextans = EnterPop.Text; 
            string res = MD5Hash(plaintextans.ToLower());
            foreach (string ans in Answers)
            {
                
                if (ans.Equals(res) )
                {
                    myPopup.IsOpen = false;
                    CaptchaChecker.IsChecked = true;
                }
            }
        }
    }
}
