﻿using MaterialDesignThemes.Wpf;
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
using WPFSpark;
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
        string file=""; 
        #endregion
        public CostumerShow(BlApi.Ibl dat, BO.Customer cst)
        {
            this.dat = dat;
            InitializeComponent();


            Show.Visibility = Visibility.Visible;
            Add.Visibility = Visibility.Hidden;
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



        public CostumerShow(BlApi.Ibl dat)
        {
            this.dat = dat;
            InitializeComponent();
            List<string> resCaptcha = dat.GetCapchaQuestion();
            myPopup.DataContext = resCaptcha.First();
            Answers = resCaptcha.Skip(1);
            Add.Visibility = Visibility.Visible;
            Show.Visibility = Visibility.Hidden;
            RtbInputFile.Drop += RtbInputFile_Drop;
            RtbInputFile.PreviewDragOver += RtbInputFile_PreviewDragOver;

        }



        private void ListOfPackges_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new ParcelShow(dat, dat.GetParcel(((sender as ListView).SelectedItem as BO.ParcelInCustomer).Id)).Show();
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
                    bool valid = true ; 
                    if (!File.Exists(TMP + @"image" + Int32.Parse(IdTB.Text) + ".png"))
                        valid = Window2.SaveImage("https://thispersondoesnotexist.com/image", TMP + @"image" + Int32.Parse(IdTB.Text) + ".png", ImageFormat.Png);
                    if (valid)
                        CostumerPhoto.Source = new BitmapImage(new Uri(TMP + @"image" + Int32.Parse(IdTB.Text) + ".png"));
                }
                else
                {
                    if (!File.Exists(TMP + @"image" + Int32.Parse(IdTB.Text) + ".png"))
                        File.Copy(file, TMP + @"image" + Int32.Parse(IdTB.Text) + ".png");
                }
                dat.AddCustomer(add);
                this.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error"); 
            }

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
