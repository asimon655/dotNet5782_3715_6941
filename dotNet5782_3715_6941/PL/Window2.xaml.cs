using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
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
        public List<string> ImageList;
        private void SaveFirstImage(string Model)
        {
            // Declaring 'x' as a new WebClient() method
            WebClient x = new WebClient();

            // Setting the URL, then downloading the data from the URL.
            string source = x.DownloadString(@"https://www.google.com/search?q=" +Model.Replace(" ","+")+  @"+drone&tbm=isch&ved=2ahUKEwj2__aHx8P0AhVNwoUKHWOxAyMQ2-cCegQIABAA&oq=mavic+3+cine&gs_lcp=CgNpbWcQAzIHCCMQ7wMQJzIECAAQGDIECAAQGDIECAAQGDIECAAQGDIECAAQGDIECAAQGFCPDliPDmDuD2gAcAB4AIABhQGIAfoBkgEDMC4ymAEAoAEBqgELZ3dzLXdpei1pbWfAAQE&sclient=img&ei=cOqnYfaHCc2ElwTj4o6YAg&bih=596&biw=1229");

            // Declaring 'document' as new HtmlAgilityPack() method
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();

            // Loading document's source via HtmlAgilityPack
            document.LoadHtml(source);
            var nodes = document.DocumentNode.SelectNodes("//img[@src]");
            string link = nodes == null ? "None" : nodes.ToList().ConvertAll(
                    r => r.Attributes.ToList().ConvertAll(
                    i => i.Value)).SelectMany(j => j).Skip(2).First( x => x.Contains("http") );
            SaveImage(link, @"c:\temp\image" + Model.Replace(" ","_") + ".png", ImageFormat.Png); ;


        }
        private bool AreEqule(string filepath1, string filepath2)
        {
            FileInfo first = new FileInfo(filepath1);
            FileInfo second = new FileInfo(filepath2); 
            if (first.Length != second.Length)
                return false;
            using (FileStream fs1 = first.OpenRead())
            using (FileStream fs2 = second.OpenRead())
                for (int i = 0; i < first.Length; i++)
                    if (fs1.ReadByte() != fs2.ReadByte())
                        return false;
            return true;

        }

        public void SaveImage(string imageUrl, string filename, ImageFormat format , String ? FileOther = null )
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

        public Window2(IBL.BO.Drone drn)
        {
            
            InitializeComponent();
            Model.Content = drn.Model;
            ID.Content = drn.Id;
            LocationX.Content = drn.Current.Lattitude;
            LocationY.Content = drn.Current.Longitude;

            //Battery.Content =Math.Round( drn.BatteryStat,2) ;
           
            EMPTY.Height =new GridLength(100-drn.BatteryStat ,GridUnitType.Star ) ;
            FULL.Height = new GridLength(drn.BatteryStat,GridUnitType.Star);

            Status.Content = drn.DroneStat;
            MaxWeight.Content = drn.Weight;

            if (!File.Exists(@"c:\temp\image" + drn.Model.Replace(" ", "_") + ".png"))
                SaveFirstImage(drn.Model);
            Photo0.Source = new BitmapImage(new Uri(@"c:\temp\image" + drn.Model.Replace(" ","_") + ".png"));
            if (!(drn.ParcelTransfer is null))
            {
                
                if(!File.Exists(@"c:\temp\image" + drn.ParcelTransfer.Sender.id.ToString() + ".png"))
                    SaveImage("https://thispersondoesnotexist.com/image", @"c:\temp\image" + drn.ParcelTransfer.Sender.id.ToString() + ".png", ImageFormat.Png);
                if (!File.Exists(@"c:\temp\image" + drn.ParcelTransfer.Target.id.ToString() + ".png"))
                    SaveImage("https://thispersondoesnotexist.com/image", @"c:\temp\image" + drn.ParcelTransfer.Target.id.ToString() + ".png", ImageFormat.Png, @"c:\temp\image" + drn.ParcelTransfer.Sender.id.ToString() + ".png");
      
                Photo1.Source = new BitmapImage(new Uri(@"c:\temp\image"+drn.ParcelTransfer.Target.id.ToString() + ".png"));
                Photo2.Source = new BitmapImage(new Uri(@"c:\temp\image" + drn.ParcelTransfer.Sender.id.ToString() + ".png"));
                     SenderId.Content = drn.ParcelTransfer.Sender.id;
                SenderName.Content = drn.ParcelTransfer.Sender.name;
                TargetId.Content = drn.ParcelTransfer.Target.id;
                TargetName.Content = drn.ParcelTransfer.Target.name;
                NoParcel.Content = drn.ParcelTransfer.Id;
                SenderLocationX.Content = Math.Round(drn.ParcelTransfer.Pickup.Lattitude,2);
                SenderLocationY.Content = Math.Round(drn.ParcelTransfer.Pickup.Longitude,2);
                TargetLocationX.Content = Math.Round(drn.ParcelTransfer.Dst.Lattitude,2);
                TargetLocationY.Content = Math.Round(drn.ParcelTransfer.Dst.Longitude,2);
                Priorety.Content = drn.ParcelTransfer.Priorety;
            }
            else
            {
                SenderName.Content = "None";
                TargetId.Content = "None";
                TargetName.Content = "None";
                NoParcel.Content = "None";
                SenderLocationX.Content = "None";
                SenderLocationY.Content = "None";
                TargetLocationX.Content = "None";
                TargetLocationY.Content = "None";
                Priorety.Content = "None";

            } 

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
        }
        
    }
}
