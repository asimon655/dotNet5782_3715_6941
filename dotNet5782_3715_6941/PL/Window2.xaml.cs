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


        public Window2(BO.Drone drn)
        {
            bool ClientsExsist = !(drn.ParcelTransfer is null); 
            InitializeComponent();

            Model.Children.Add(creteLabel(drn.Model));
            Id.Children.Add(creteLabel(drn.Id.ToString()));
            ///////////////second row /////////////////
            
            Grid columnsMain = CreateGridColumn(5, new int[5] { 1, 2, 2, 2, 2 });


            //battery animation : 
            Grid battStatRows = CreateGridRow(3 , new int[3]{ 1,3,1}); 
            Grid battStatCols = CreateGridColumn(3, new int[3] { 1, 3 ,1  }); 
            Grid battStatShow = new Grid();
            Border batteryBorder = new Border() { BorderBrush = System.Windows.Media.Brushes.Black, BorderThickness =   new Thickness(5)};
            RowDefinition Empty = new RowDefinition();
            Empty.Height = new GridLength(100-drn.BatteryStat, GridUnitType.Star);
            battStatShow.RowDefinitions.Add(Empty);
            RowDefinition Full = new RowDefinition();
            Full.Height = new GridLength(drn.BatteryStat, GridUnitType.Star);
            battStatShow.RowDefinitions.Add(Full);
            Grid FullGrid = new Grid() { Background = System.Windows.Media.Brushes.LawnGreen };
            Grid EmptyGridRows = CreateGridRow(2, new int [2]{ 10,1});
            Grid EmptyGridCols = CreateGridColumn(3, new int[3] { 1, 1, 1 });
            Grid EmptyGrid = new Grid { Background = System.Windows.Media.Brushes.Black };
            //postioning 
            Grid.SetColumn(EmptyGrid, 1);
            Grid.SetRow(EmptyGridCols, 1);
            Grid.SetRow(EmptyGridRows, 0); 
            Grid.SetRow(FullGrid, 1);
            Grid.SetColumn(batteryBorder, 1);
            Grid.SetRow(battStatCols, 1);
            Grid.SetColumn(battStatRows, 0); 
            // adding by order (sorted) 
            columnsMain.Children.Add(battStatRows);
            battStatRows.Children.Add(battStatCols);
            /// 1 branch 
            battStatRows.Children.Add(EmptyGridRows);
            EmptyGridRows.Children.Add(EmptyGridCols);
            EmptyGridCols.Children.Add(EmptyGrid);
            /// 2branch 
            battStatCols.Children.Add(batteryBorder);
            batteryBorder.Child = battStatShow; 
            battStatShow.Children.Add(FullGrid);
            FullGrid.Children.Add(creteLabel(Math.Round(drn.BatteryStat, 0).ToString()+'%'));




            //enums : 

            Grid enumsRows = CreateGridRow(3, new int[3] { 1, 1, 1 });
            Grid WeightGrid = CreateGridColumn(2, new int[2] { 1, 1 });
             Grid PriortyGrid = CreateGridColumn(2 , new int [2] {1,1} );
            Grid StatusGrid = CreateGridColumn(2, new int[2] { 1, 1 });
            Viewbox WeightConst = creteLabel("Weight: ");
            Viewbox WeightLbl = creteLabel(drn.Weight.ToString());
            Viewbox PriortyConst = creteLabel("Priorty: ");
            Viewbox PriortyLbl= creteLabel((ClientsExsist ? drn.ParcelTransfer.Priorety.ToString() : "None"));
            Viewbox StatusConst = creteLabel("Status: ");
            Viewbox StatusLbl = creteLabel(drn.DroneStat.ToString());
            Grid.SetColumn(enumsRows, 1);
            Grid.SetRow(WeightGrid, 0);
            Grid.SetRow(PriortyGrid, 2);
            Grid.SetRow(StatusGrid, 1);
            Grid.SetColumn(WeightConst, 0);
            Grid.SetColumn(WeightLbl, 1);
            Grid.SetColumn(PriortyConst, 0);
            Grid.SetColumn(PriortyLbl, 1);
            Grid.SetColumn(StatusConst, 0);
            Grid.SetColumn(StatusLbl, 1);
            columnsMain.Children.Add(enumsRows);
            enumsRows.Children.Add(WeightGrid);
            enumsRows.Children.Add(PriortyGrid);
            enumsRows.Children.Add(StatusGrid);
            WeightGrid.Children.Add(WeightLbl);
            WeightGrid.Children.Add(WeightConst);
            StatusGrid.Children.Add(StatusConst);
            StatusGrid.Children.Add(StatusLbl);
            PriortyGrid.Children.Add(PriortyConst);
            PriortyGrid.Children.Add(PriortyLbl);
            // location
            Grid Location = CreateGridColumn(2,new int[2]{ 1,1});
            Grid LocationXY = CreateGridRow(4, new int[4] {2,3,3,2});
            Viewbox LocationConst = creteLabel("Location: ");
            Viewbox LocationX = creteLabel(Math.Round(drn.Current.Lattitude,3).ToString());
            Viewbox LocationY = creteLabel(Math.Round(drn.Current.Longitude, 3).ToString());


            Grid.SetColumn(Location, 2); 
            Grid.SetColumn(LocationXY, 1);
            Grid.SetColumn(LocationConst, 0);
            Grid.SetRow(LocationX, 1);
            Grid.SetRow(LocationY, 2);
            columnsMain.Children.Add(Location);
            Location.Children.Add(LocationConst);
            Location.Children.Add(LocationXY);
            LocationXY.Children.Add(LocationY);
            LocationXY.Children.Add(LocationX);




            //dist 
            Grid Dist = CreateGridColumn(2, new int[2] { 1, 1 });
            Viewbox DistConst = creteLabel("Distance: ");
            Viewbox DistLbl = creteLabel(ClientsExsist ?  Math.Round(drn.ParcelTransfer.Distance, 3).ToString() : "None");
           


            Grid.SetColumn(Dist, 3);
            Grid.SetColumn(DistLbl, 1);
            Grid.SetColumn(DistConst, 0);
            columnsMain.Children.Add(Dist);
            Dist.Children.Add(DistConst);
            Dist.Children.Add(DistLbl);


            //binded parcels 
            Grid Bind = CreateGridColumn(2, new int[2] { 1, 1 });
            Viewbox BindConst = creteLabel("Parcels's Id : ");
            Viewbox BindLbl = creteLabel(ClientsExsist ?drn.ParcelTransfer.Id.ToString() : "None");



            Grid.SetColumn(Bind, 4);
            Grid.SetColumn(BindLbl, 1);
            Grid.SetColumn(BindConst, 0);
            columnsMain.Children.Add(Bind);
            Bind.Children.Add(BindConst);
           Bind.Children.Add(BindLbl);
            

            Specs.Children.Add(columnsMain);
            //////////////////////////third row ///////////////////////////////////////////////////////////////
            Grid Costumers = CreateGridColumn(2, new int[2] { 1, 1 });
            //// costumer -1 
            Grid SenderCostumer1 = CreateGridColumn(4, new int[4] { 1, 1, 1, 1 });
            Grid SenderIdGrid = CreateGridRow(2, new int[2] { 1, 1 });
            Grid SenderNameGrid = CreateGridRow(2, new int[2] { 1, 1 });
            Grid SenderLocationGrid = CreateGridRow(2, new int[2] { 1, 1 });
            Grid SenderLocationRows = CreateGridRow(2, new int[2] { 1, 1 });
            Grid SenderPhotoGrid = CreateGridRow(2, new int[2] { 4, 1 });
            Viewbox SenderIdConst = creteLabel("ID: ");
            Viewbox SenderNameConst = creteLabel("Name: ");
            Viewbox SenderLocationConst = creteLabel("Location: ");
            Viewbox SenderPhotoConst = creteLabel("Sender");

            Viewbox SenderLocationX;
            Viewbox SenderLocationY;
            Viewbox SenderNameLbl ;
            Viewbox SenderIdLbl ;
            System.Windows.Controls.Image SenderPhoto;
            if (ClientsExsist) {
                 SenderLocationX = creteLabel(Math.Round(drn.ParcelTransfer.Pickup.Lattitude,3).ToString());
                 SenderLocationY = creteLabel(Math.Round(drn.ParcelTransfer.Pickup.Longitude,3).ToString());
                 SenderNameLbl = creteLabel(drn.ParcelTransfer.Sender.name);
                SenderIdLbl = creteLabel(drn.ParcelTransfer.Sender.id.ToString());
                if (!File.Exists(TMP + @"image" + drn.ParcelTransfer.Sender.id.ToString() + ".png"))
                    SaveImage("https://thispersondoesnotexist.com/image", TMP + @"image" + drn.ParcelTransfer.Sender.id.ToString() + ".png", ImageFormat.Png);
                SenderPhoto = new System.Windows.Controls.Image();
                SenderPhoto.Source = new BitmapImage(new Uri(TMP + @"image" + drn.ParcelTransfer.Sender.id.ToString() + ".png"));

            } 
            else {

                SenderLocationX = creteLabel("None");
                 SenderLocationY = creteLabel("None");
                 SenderNameLbl = creteLabel("None");
                 SenderIdLbl = creteLabel("None");
                SenderPhoto = new System.Windows.Controls.Image();
                SenderPhoto.Source = new BitmapImage(new Uri(@"https://media.istockphoto.com/vectors/male-user-icon-vector-id517998264?k=20&m=517998264&s=612x612&w=0&h=pdEwtkJlZsIoYBVeO2Bo4jJN6lxOuifgjaH8uMIaHTU="));

            } 


         
            Grid.SetColumn(SenderCostumer1, 0);
            Grid.SetColumn(SenderPhotoGrid, 0); 
            Grid.SetColumn(SenderIdGrid, 1);
            Grid.SetColumn(SenderNameGrid, 2);
            Grid.SetColumn(SenderLocationGrid, 3);
            Grid.SetRow(SenderIdConst, 0);
            Grid.SetRow(SenderNameConst, 0);
            Grid.SetRow(SenderLocationConst, 0);
            Grid.SetRow(SenderPhotoConst, 1);
            Grid.SetRow(SenderPhoto, 0);
            Grid.SetRow(SenderIdLbl, 1);
            Grid.SetRow(SenderNameLbl, 1);
            Grid.SetRow(SenderLocationRows ,1);
            Grid.SetRow(SenderLocationX, 0);
            Grid.SetRow(SenderLocationY, 1);


            Costumers.Children.Add(SenderCostumer1);
            SenderCostumer1.Children.Add(SenderPhotoGrid);
            SenderCostumer1.Children.Add(SenderIdGrid);
            SenderCostumer1.Children.Add(SenderNameGrid);
            SenderCostumer1.Children.Add(SenderLocationGrid);
            SenderPhotoGrid.Children.Add(SenderPhotoConst);
            SenderPhotoGrid.Children.Add(SenderPhoto);
            SenderIdGrid.Children.Add(SenderIdConst);
            SenderIdGrid.Children.Add(SenderIdLbl);
            SenderNameGrid.Children.Add(SenderNameConst);
            SenderNameGrid.Children.Add(SenderNameLbl);
            SenderLocationGrid.Children.Add(SenderLocationConst);
            SenderLocationGrid.Children.Add(SenderLocationRows);
            SenderLocationRows.Children.Add(SenderLocationX);
            SenderLocationRows.Children.Add(SenderLocationY);




            /////costuner -2 
            Grid TargetCostumer1 = CreateGridColumn(4, new int[4] { 1, 1, 1, 1 });
            Grid TargetIdGrid = CreateGridRow(2, new int[2] { 1, 1 });
            Grid TargetNameGrid = CreateGridRow(2, new int[2] { 1, 1 });
            Grid TargetLocationGrid = CreateGridRow(2, new int[2] { 1, 1 });
            Grid TargetLocationRows = CreateGridRow(2, new int[2] { 1, 1 });
            Grid TargetPhotoGrid = CreateGridRow(2, new int[2] { 4, 1 });
            Viewbox TargetIdConst = creteLabel("ID: ");
            Viewbox TargetNameConst = creteLabel("Name: ");
            Viewbox TargetLocationConst = creteLabel("Location: ");
            Viewbox TargetPhotoConst = creteLabel("Target");

            Viewbox TargetLocationX;
            Viewbox TargetLocationY;
            Viewbox TargetNameLbl;
            Viewbox TargetIdLbl;
            System.Windows.Controls.Image TargetPhoto;
            if (ClientsExsist)
            {
                TargetLocationX = creteLabel(Math.Round(drn.ParcelTransfer.Pickup.Lattitude, 3).ToString());
                TargetLocationY = creteLabel(Math.Round(drn.ParcelTransfer.Pickup.Longitude, 3).ToString());
                TargetNameLbl = creteLabel(drn.ParcelTransfer.Target.name);
                TargetIdLbl = creteLabel(drn.ParcelTransfer.Target.id.ToString());
                if (!File.Exists(TMP + @"image" + drn.ParcelTransfer.Target.id.ToString() + ".png"))
                    SaveImage("https://thispersondoesnotexist.com/image", TMP + @"image" + drn.ParcelTransfer.Target.id.ToString() + ".png", ImageFormat.Png, TMP + @"image" + drn.ParcelTransfer.Sender.id.ToString() + ".png");
                TargetPhoto = new System.Windows.Controls.Image();
                TargetPhoto.Source = new BitmapImage(new Uri(TMP + @"image" + drn.ParcelTransfer.Target.id.ToString() + ".png"));

            }
            else
            {

                TargetLocationX = creteLabel("None");
                TargetLocationY = creteLabel("None");
                TargetNameLbl = creteLabel("None");
                TargetIdLbl = creteLabel("None");
                TargetPhoto = new System.Windows.Controls.Image();
                TargetPhoto.Source = new BitmapImage(new Uri(@"https://media.istockphoto.com/vectors/male-user-icon-vector-id517998264?k=20&m=517998264&s=612x612&w=0&h=pdEwtkJlZsIoYBVeO2Bo4jJN6lxOuifgjaH8uMIaHTU="));

            }



            Grid.SetColumn(TargetCostumer1, 1);
            Grid.SetColumn(TargetPhotoGrid, 0);
            Grid.SetColumn(TargetIdGrid, 1);
            Grid.SetColumn(TargetNameGrid, 2);
            Grid.SetColumn(TargetLocationGrid, 3);
            Grid.SetRow(TargetIdConst, 0);
            Grid.SetRow(TargetNameConst, 0);
            Grid.SetRow(TargetLocationConst, 0);
            Grid.SetRow(TargetPhotoConst, 1);
            Grid.SetRow(TargetPhoto, 0);
            Grid.SetRow(TargetIdLbl, 1);
            Grid.SetRow(TargetNameLbl, 1);
            Grid.SetRow(TargetLocationRows, 1);
            Grid.SetRow(TargetLocationX, 0);
            Grid.SetRow(TargetLocationY, 1);


            Costumers.Children.Add(TargetCostumer1);
            TargetCostumer1.Children.Add(TargetPhotoGrid);
            TargetCostumer1.Children.Add(TargetIdGrid);
            TargetCostumer1.Children.Add(TargetNameGrid);
            TargetCostumer1.Children.Add(TargetLocationGrid);
            TargetPhotoGrid.Children.Add(TargetPhotoConst);
            TargetPhotoGrid.Children.Add(TargetPhoto);
            TargetIdGrid.Children.Add(TargetIdConst);
            TargetIdGrid.Children.Add(TargetIdLbl);
            TargetNameGrid.Children.Add(TargetNameConst);
            TargetNameGrid.Children.Add(TargetNameLbl);
            TargetLocationGrid.Children.Add(TargetLocationConst);
            TargetLocationGrid.Children.Add(TargetLocationRows);
            TargetLocationRows.Children.Add(TargetLocationX);
            TargetLocationRows.Children.Add(TargetLocationY);




            //adds 
            ClintsORGif.Children.Add(Costumers);
           
            

            if (!File.Exists(TMP + @"image" + drn.Model.Replace(" ", "_") + ".png"))
                SaveFirstImage(drn.Model);
            Photo0.Source = new BitmapImage(new Uri(TMP + @"image" + drn.Model.Replace(" ", "_") + ".png"));
            Function.Click += new RoutedEventHandler(Button_Click_Show);

        }


        public Window2(BlApi.Ibl x ,Page pg )
        {
           
            pageof = pg; 

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
            Viewbox text2 = creteLabel("Wight:  ");
            Grid.SetColumn(text1, 0);
            Grid.SetColumn(input1, 1);
            Grid.SetColumn(text2, 2);
            Grid rowsComb = CreateGridRow( 3, new int [3] {1,1,1} );
            ComboBox combo1 = creteComboBox(Enum.GetValues(typeof(BO.WeightCategories)));
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
                BO.Drone drony = new BO.Drone();

                drony.Model = ((pacads[0] as Viewbox).Child as TextBox).Text;
                drony.Id = Convert.ToInt32((((pacads[1] as Viewbox).Child as TextBox).Text));
                drony.Weight = (BO.WeightCategories)(pacads[2] as ComboBox).SelectedItem;


                log.AddDrone(drony, Convert.ToInt32((((pacads[3] as Viewbox).Child as TextBox).Text)));
                (pageof as MainWindow).ListOf.ItemsSource = log.DronesPrintFiltered((pageof as MainWindow).Stat, (pageof as MainWindow).Weight);
                try
                {
                    pageof.NavigationService.Refresh();
                }
                catch { }
             
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
