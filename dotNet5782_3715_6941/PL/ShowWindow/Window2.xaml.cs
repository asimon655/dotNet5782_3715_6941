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

        ManngerWin admin;
        public List<string> ImageList;
        List<Object> pacads = new List<object>(); 
        BlApi.Ibl log;

        Page pageof;

        string TMP = System.IO.Path.GetTempPath();

        private Viewbox creteLabel(string text)
        {
            Viewbox view1 = new Viewbox();
            Label label1 = new Label();
            label1.Content = text;
            view1.Child = label1;
            return view1;


        }

        private Viewbox creteTextBox()
        {
            Viewbox view1 = new Viewbox();
            TextBox textBox1 = new TextBox();
            textBox1.Text = "";
            view1.Child = textBox1;
            return view1;


        }

        private ComboBox creteComboBox(Array  enumy )
        {

            ComboBox comboBox1 = new ComboBox();
            comboBox1.ItemsSource = enumy; 
    
            return comboBox1;


        }

        private Grid CreateGridRow(int  ? row  , int [] arr  )  
        {
            if (arr.Length != row && !(row is null))
                throw new Exception("THE ARRAY DOESNT FIT ");
            if ((row is null) && arr.Length != 1)
                throw new Exception("THE ARRAY DOESNT FIT ");
            Grid gridy = new Grid() ;
            for (int i = 0; i < row; i++)
            {
                RowDefinition tmp = new RowDefinition(); 
                tmp.Height = new GridLength(arr[i], GridUnitType.Star);
                gridy.RowDefinitions.Add(tmp);
            }
            return gridy; 
            
   
        
        }

        private Grid CreateGridColumn(int? column , int[] arr)
        {
            if (arr.Length != column && !(column is null)  )
                throw new Exception("THE ARRAY DOESNT FIT ");
            if ((column is null) && arr.Length != 1 )
                throw new Exception("THE ARRAY DOESNT FIT ");
            Grid gridy = new Grid();
            for (int i = 0; i < column; i++)
            {
                ColumnDefinition tmp = new ColumnDefinition();
                tmp.Width = new GridLength(arr[i], GridUnitType.Star);
                gridy.ColumnDefinitions.Add(tmp);
            }
            return gridy;



        }

        private void SaveFirstImage(string Model)
        {
            // Declaring 'x' as a new WebClient() method
            WebClient x = new WebClient();

            // Setting the URL, then downloading the data from the URL.
            string source = x.DownloadString(@"https://www.google.com/search?q=" + Model.Replace(" ", "+") + @"+drone&tbm=isch&ved=2ahUKEwj2__aHx8P0AhVNwoUKHWOxAyMQ2-cCegQIABAA&oq=mavic+3+cine&gs_lcp=CgNpbWcQAzIHCCMQ7wMQJzIECAAQGDIECAAQGDIECAAQGDIECAAQGDIECAAQGDIECAAQGFCPDliPDmDuD2gAcAB4AIABhQGIAfoBkgEDMC4ymAEAoAEBqgELZ3dzLXdpei1pbWfAAQE&sclient=img&ei=cOqnYfaHCc2ElwTj4o6YAg&bih=596&biw=1229");

            // Declaring 'document' as new HtmlAgilityPack() method
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();

            // Loading document's source via HtmlAgilityPack
            document.LoadHtml(source);
            var nodes = document.DocumentNode.SelectNodes("//img[@src]");
            string link = nodes == null ? "None" : nodes.SelectMany(j => j.Attributes).First(x => x.Value.Contains("http")).Value;
            SaveImage(link, TMP + @"image" + Model.Replace(" ", "_") + ".png", ImageFormat.Png); ;


        }


        private bool AreEqule(string filepath1, string filepath2)
        {
            FileInfo first = new FileInfo(filepath1);
            FileInfo second = new FileInfo(filepath2);
            if (first.Length != second.Length)
                return false;
            using (FileStream fs1 = first.OpenRead())
            using (FileStream fs2 = second.OpenRead())
            {
                HMACSHA1 h1 = new HMACSHA1();
    
                byte[] hash1 = h1.ComputeHash(fs1);
                byte[] hash2 = h1.ComputeHash(fs2);
                for (int i = 0; i < hash2.Length; i++)
                    if (hash1[i] != hash2[i])
                        return false;
            }
            return true;

        }

        public void SaveImage(string imageUrl, string filename, ImageFormat format, String? FileOther = null)
        {
            Thread.Sleep(10);

            WebClient client = new WebClient();

            client = new WebClient();
            Stream stream = client.OpenRead(imageUrl);
            Bitmap bitmap; bitmap = new Bitmap(stream);

            if (bitmap != null)
            {
                bitmap.Save(filename, format);
            }

            stream.Flush();
            stream.Close();
            client.Dispose();
            if (!(FileOther is null))
            {
                if (AreEqule(filename, FileOther))
                {
                    File.Delete(filename);
                    SaveImage(imageUrl, filename, format, FileOther);
                }

            }
        }


        


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
