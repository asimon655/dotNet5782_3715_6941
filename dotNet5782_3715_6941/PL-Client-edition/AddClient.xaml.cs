using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL_Client_edition
{
    /// <summary>
    /// Interaction logic for AddClient.xaml
    /// </summary>
    public partial class AddClient : Window
    {
        #region Fields 
        BlApi.Ibl dat;
        IEnumerable<string> Answers;
        static internal string TMP = System.IO.Path.GetTempPath();
        string file = "";
        #endregion
        internal void MetaDataCstReset(System.Windows.Controls.Image Photo2, int SenderId)
        {

            if (!File.Exists(PhotoAsync.makePath(SenderId)))
                PhotoAsync.SaveImageAsync(PhotoAsync.FaceAIURL, PhotoAsync.makePath(SenderId), PhotoAsync.fileEndEnum).ContinueWith(x => {
                    if (x.Result)

                        Dispatcher.Invoke(() =>
                        {
                            Photo2.Source = new BitmapImage(new Uri(PhotoAsync.makePath(SenderId)));
                        });
                });
            else
                Photo2.Source = new BitmapImage(new Uri(PhotoAsync.makePath(SenderId)));


        }





        public AddClient(BlApi.Ibl dat)
        {
            InitializeComponent();

            this.dat = dat;

            dat.GetCapchaQuestion().ContinueWith(x => {

                Dispatcher.Invoke(() =>
                {
                    myPopup.DataContext = x.Result.First();
                    CaptchCheck.IsEnabled = true;

                });
                Answers = x.Result.Skip(1);


            }
            );


            Add.Visibility = Visibility.Visible;

            RtbInputFile.Drop += RtbInputFile_Drop;
            RtbInputFile.PreviewDragOver += RtbInputFile_PreviewDragOver;

        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BO.Customer add = new BO.Customer()
                {
                    Name = NameTB.Text,
                    Id = Int32.Parse(IdTB.Text),
                    Phone_Num = PhoneTB.Text,
                    Loct = new BO.Location(Double.Parse(LongTB.Text), Double.Parse(LatTB.Text))
                };

                if (file.Equals(""))
                {
                    MetaDataCstReset(new Image(), Int32.Parse(IdTB.Text));
                }
                else
                {
                    if (!File.Exists(TMP + @"image" + Int32.Parse(IdTB.Text) + ".png"))
                        File.Copy(file, PhotoAsync.makePath(Int32.Parse(IdTB.Text)));
                }
                dat.AddCustomer(add);
                this.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error");
            }

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

                if (ans.Equals(res))
                {
                    myPopup.IsOpen = false;
                    ButtonAdd.IsEnabled = true;
                    Vi.Visibility = Visibility.Visible;
                    sprocketControl1.Visibility = Visibility.Hidden;
                }

            }
        }

        private void CaptchaChecker_Checked(object sender, RoutedEventArgs e)
        {

        }
        private void RtbInputFile_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                var file = files[0];

                WaterMark.Visibility = Visibility.Hidden;
                UserPhoto.Visibility = Visibility.Visible;
                try
                {
                    UserPhoto.Source = new BitmapImage(new Uri(file));
                    this.file = file;
                }
                catch
                {
                    MessageBox.Show("Enrer a photo and not a doucument ", "Error");
                }


            }
        }


        private void RtbInputFile_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }



    }
}
