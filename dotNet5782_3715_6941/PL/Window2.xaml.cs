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
        List<Object> pacads = new List<object>(); 
        IBL.Ibl log;
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
            textBox1.Text = ">";
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
            SaveImage(link, @"c:\temp\image" + Model.Replace(" ", "_") + ".png", ImageFormat.Png); ;


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


        public Window2(IBL.BO.Drone drn)
        {

            InitializeComponent();

            Model.Children.Add(creteLabel(drn.Model));
            Id.Children.Add(creteLabel(drn.Id.ToString()));
            ///////////////second row /////////////////
            
            Grid columnsMain = CreateGridColumn(5, new int[5] { 1, 2, 2, 2, 2 });
            //battery animation : 
            Grid battStatCols = CreateGridColumn(3, new int[3] { 1, 3 ,1  }); 
            Grid battStatShow = new Grid();
     
            RowDefinition Empty = new RowDefinition();
            Empty.Height = new GridLength(100-drn.BatteryStat, GridUnitType.Star);
            battStatShow.RowDefinitions.Add(Empty);
            RowDefinition Full = new RowDefinition();
            Full.Height = new GridLength(drn.BatteryStat, GridUnitType.Star);
            battStatShow.RowDefinitions.Add(Full);

            Grid FullGrid = new Grid() { Background = System.Windows.Media.Brushes.LawnGreen };
            Grid.SetRow(FullGrid, 1);
            Grid.SetColumn(battStatShow, 1);
            Grid.SetColumn(battStatCols, 0);
            columnsMain.Children.Add(battStatCols);
            battStatCols.Children.Add(battStatShow);
            battStatShow.Children.Add(FullGrid); 



            //


            Specs.Children.Add(columnsMain); 








            if (!File.Exists(@"c:\temp\image" + drn.Model.Replace(" ", "_") + ".png"))
                SaveFirstImage(drn.Model);
            Photo0.Source = new BitmapImage(new Uri(@"c:\temp\image" + drn.Model.Replace(" ", "_") + ".png"));
            if (!(drn.ParcelTransfer is null))
            {

                if (!File.Exists(@"c:\temp\image" + drn.ParcelTransfer.Sender.id.ToString() + ".png"))
                    SaveImage("https://thispersondoesnotexist.com/image", @"c:\temp\image" + drn.ParcelTransfer.Sender.id.ToString() + ".png", ImageFormat.Png);
                if (!File.Exists(@"c:\temp\image" + drn.ParcelTransfer.Target.id.ToString() + ".png"))
                    SaveImage("https://thispersondoesnotexist.com/image", @"c:\temp\image" + drn.ParcelTransfer.Target.id.ToString() + ".png", ImageFormat.Png, @"c:\temp\image" + drn.ParcelTransfer.Sender.id.ToString() + ".png");

               
             
            }
            else
            {
                
         

            }

        }


        public Window2(IBL.Ibl x)
        {
           

            InitializeComponent();
            ///////////////// first  row /////////////////////////
            Viewbox inputModel = creteTextBox();
            Model.Children.Add(inputModel);
            pacads.Add(inputModel);
            Viewbox inputId = creteTextBox();
            Id.Children.Add(inputId);
            pacads.Add(inputId);
            ///////////////// second row /////////////////////////
            Grid colums = CreateGridColumn(4, new int[4] { 1, 1, 1, 1 });
            Viewbox text1 = creteLabel(" charging staion's ID :  ");
            Viewbox input1 = creteTextBox();
            Viewbox text2 = creteLabel(" charging staion's ID :  ");
            Grid.SetColumn(text1, 0);
            Grid.SetColumn(input1, 1);
            Grid.SetColumn(text2, 2);
            Grid rowsComb = CreateGridRow( 3, new int [3] {1,1,1} );
            ComboBox combo1 = creteComboBox(Enum.GetValues(typeof(IBL.BO.WeightCategories)));
            pacads.Add(combo1);
            combo1.VerticalContentAlignment = VerticalAlignment.Stretch;
            combo1.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            combo1.FontStretch = FontStretches.UltraExpanded;
            combo1.FontSize = 20;
            Grid.SetRow(combo1, 1);
            rowsComb.Children.Add(combo1);
            Grid.SetColumn(rowsComb, 3);
            colums.Children.Add(text1);
            colums.Children.Add(text2);
            colums.Children.Add(input1);
            colums.Children.Add(rowsComb);
            Specs.Children.Add(colums);
            pacads.Add(input1); 
               

            // add 2 columns to Main Grid - that we working on 

            ///////////////// third  row /////////////////////////
            MediaElement gif = new MediaElement();
            gif.Source = new Uri(@"https://i.pinimg.com/originals/e0/7c/81/e07c810beb24489a0d99d04a37cf7a3f.gif");
            gif.MediaEnded += new RoutedEventHandler(GifLoader_Unloaded);
            gif.Stretch = Stretch.Fill;
            ClintsORGif.Children.Add(gif);


            log = x;
            Function.Click += new RoutedEventHandler(Button_Click_Add);
            






        }
        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            try
            {
                IBL.BO.Drone drony = new IBL.BO.Drone();

                drony.Model = ((pacads[0] as Viewbox).Child as TextBox).Text;
                drony.Id = Convert.ToInt32((((pacads[1] as Viewbox).Child as TextBox).Text));
                drony.Weight = (IBL.BO.WeightCategories)(pacads[2] as ComboBox).SelectedItem;


                log.AddDrone(drony, Convert.ToInt32((((pacads[3] as Viewbox).Child as TextBox).Text)));
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
