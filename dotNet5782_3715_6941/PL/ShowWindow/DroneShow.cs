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

        #region DroneShow
        BO.Drone drn;
        public Window2( BlApi.Ibl log, BO.Drone drn)
        {
            this.log = log; 
            this.drn = drn; 
            bool ClientsExsist = !(drn.ParcelTransfer is null);
            InitializeComponent();

            Model.Children.Add(createLabel(drn.Model));
            Id.Children.Add(createLabel(drn.Id.ToString()));
            ///////////////second row /////////////////

            Grid columnsMain = CreateGridColumn(6, new int[6] { 1, 2, 2, 2, 2,1 });
            Button opbtn = new Button();
            opbtn.Content = createLabel("Operations");
            opbtn.Click += PopupShiwiw;
            Grid.SetColumn(opbtn, 5);
            columnsMain.Children.Add(opbtn);

            //battery animation : 
            Grid battStatRows = CreateGridRow(3, new int[3] { 1, 3, 1 });
            Grid battStatCols = CreateGridColumn(3, new int[3] { 1, 3, 1 });
            Grid battStatShow = new Grid();
            Border batteryBorder = new Border() { BorderBrush = System.Windows.Media.Brushes.Black, BorderThickness = new Thickness(5) };
            RowDefinition Empty = new RowDefinition();
            Empty.Height = new GridLength(100 - drn.BatteryStat, GridUnitType.Star);
            battStatShow.RowDefinitions.Add(Empty);
            RowDefinition Full = new RowDefinition();
            Full.Height = new GridLength(drn.BatteryStat, GridUnitType.Star);
            battStatShow.RowDefinitions.Add(Full);
            Grid FullGrid = new Grid() { Background = System.Windows.Media.Brushes.LawnGreen };
            Grid EmptyGridRows = CreateGridRow(2, new int[2] { 10, 1 });
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
            FullGrid.Children.Add(createLabel(Math.Round(drn.BatteryStat, 0).ToString() + '%'));




            //enums : 

            Grid enumsRows = CreateGridRow(3, new int[3] { 1, 1, 1 });
            Grid WeightGrid = CreateGridColumn(2, new int[2] { 1, 1 });
            Grid PriortyGrid = CreateGridColumn(2, new int[2] { 1, 1 });
            Grid StatusGrid = CreateGridColumn(2, new int[2] { 1, 1 });
            Viewbox WeightConst = createLabel("Weight: ");
            Viewbox WeightLbl = createLabel(drn.Weight.ToString());
            Viewbox PriortyConst = createLabel("Priorty: ");
            Viewbox PriortyLbl = createLabel((ClientsExsist ? drn.ParcelTransfer.Priorety.ToString() : "None"));
            Viewbox StatusConst = createLabel("Status: ");
            Viewbox StatusLbl = createLabel(drn.DroneStat.ToString());
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
            Grid Location = CreateGridColumn(2, new int[2] { 1, 1 });
            Grid LocationXY = CreateGridRow(4, new int[4] { 2, 3, 3, 2 });
            Viewbox LocationConst = createLabel("Location: ");
            Viewbox LocationX = createLabel(Math.Round(drn.Current.Lattitude, 3).ToString());
            Viewbox LocationY = createLabel(Math.Round(drn.Current.Longitude, 3).ToString());


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
            Viewbox DistConst = createLabel("Distance: ");
            Viewbox DistLbl = createLabel(ClientsExsist ? Math.Round(drn.ParcelTransfer.Distance, 3).ToString() : "None");



            Grid.SetColumn(Dist, 3);
            Grid.SetColumn(DistLbl, 1);
            Grid.SetColumn(DistConst, 0);
            columnsMain.Children.Add(Dist);
            Dist.Children.Add(DistConst);
            Dist.Children.Add(DistLbl);


            //binded parcels 
            Grid Bind = CreateGridColumn(2, new int[2] { 1, 1 });
            Viewbox BindConst = createLabel("Parcels's Id : ");
            Viewbox BindLbl = createLabel(ClientsExsist ? drn.ParcelTransfer.Id.ToString() : "None");



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
            Viewbox SenderIdConst = createLabel("ID: ");
            Viewbox SenderNameConst = createLabel("Name: ");
            Viewbox SenderLocationConst = createLabel("Location: ");
            Viewbox SenderPhotoConst = createLabel("Sender");

            Viewbox SenderLocationX;
            Viewbox SenderLocationY;
            Viewbox SenderNameLbl;
            Viewbox SenderIdLbl;
            System.Windows.Controls.Image SenderPhoto;
            if (ClientsExsist)
            {
                bool valid=true; 
                SenderLocationX = createLabel(Math.Round(drn.ParcelTransfer.Pickup.Lattitude, 3).ToString());
                SenderLocationY = createLabel(Math.Round(drn.ParcelTransfer.Pickup.Longitude, 3).ToString());
                SenderNameLbl = createLabel(drn.ParcelTransfer.Sender.name);
                SenderIdLbl = createLabel(drn.ParcelTransfer.Sender.id.ToString());
                if (!File.Exists(TMP + @"image" + drn.ParcelTransfer.Sender.id.ToString() + ".png"))
                   valid =   SaveImage("https://thispersondoesnotexist.com/image", TMP + @"image" + drn.ParcelTransfer.Sender.id.ToString() + ".png", ImageFormat.Png);
                if (valid)
                {
                    SenderPhoto = new System.Windows.Controls.Image();
                    SenderPhoto.Source = new BitmapImage(new Uri(TMP + @"image" + drn.ParcelTransfer.Sender.id.ToString() + ".png"));
                    Grid.SetRow(SenderPhoto, 0);
                    SenderPhotoGrid.Children.Add(SenderPhoto);
                }
            
                }
            else
            {

                SenderLocationX = createLabel("None");
                SenderLocationY = createLabel("None");
                SenderNameLbl = createLabel("None");
                SenderIdLbl = createLabel("None");
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
            Grid.SetRow(SenderIdLbl, 1);
            Grid.SetRow(SenderNameLbl, 1);
            Grid.SetRow(SenderLocationRows, 1);
            Grid.SetRow(SenderLocationX, 0);
            Grid.SetRow(SenderLocationY, 1);


            Costumers.Children.Add(SenderCostumer1);
            SenderCostumer1.Children.Add(SenderPhotoGrid);
            SenderCostumer1.Children.Add(SenderIdGrid);
            SenderCostumer1.Children.Add(SenderNameGrid);
            SenderCostumer1.Children.Add(SenderLocationGrid);
            SenderPhotoGrid.Children.Add(SenderPhotoConst);

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
            Viewbox TargetIdConst = createLabel("ID: ");
            Viewbox TargetNameConst = createLabel("Name: ");
            Viewbox TargetLocationConst = createLabel("Location: ");
            Viewbox TargetPhotoConst = createLabel("Target");

            Viewbox TargetLocationX;
            Viewbox TargetLocationY;
            Viewbox TargetNameLbl;
            Viewbox TargetIdLbl;
            System.Windows.Controls.Image TargetPhoto;
            if (ClientsExsist)
            {
                bool valid = true; 
                TargetLocationX = createLabel(Math.Round(drn.ParcelTransfer.Pickup.Lattitude, 3).ToString());
                TargetLocationY = createLabel(Math.Round(drn.ParcelTransfer.Pickup.Longitude, 3).ToString());
                TargetNameLbl = createLabel(drn.ParcelTransfer.Target.name);
                TargetIdLbl = createLabel(drn.ParcelTransfer.Target.id.ToString());
                if (!File.Exists(TMP + @"image" + drn.ParcelTransfer.Target.id.ToString() + ".png"))
                    valid = SaveImage("https://thispersondoesnotexist.com/image", TMP + @"image" + drn.ParcelTransfer.Target.id.ToString() + ".png", ImageFormat.Png, TMP + @"image" + drn.ParcelTransfer.Sender.id.ToString() + ".png");
                if (valid)
                {
                    TargetPhoto = new System.Windows.Controls.Image();
                    TargetPhoto.Source = new BitmapImage(new Uri(TMP + @"image" + drn.ParcelTransfer.Target.id.ToString() + ".png"));
                    Grid.SetRow(TargetPhoto, 0);
                    TargetPhotoGrid.Children.Add(TargetPhoto);
                }
           
                }
            else
            {

                TargetLocationX = createLabel("None");
                TargetLocationY = createLabel("None");
                TargetNameLbl = createLabel("None");
                TargetIdLbl = createLabel("None");
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

            {
                bool valid = true;
                if (!File.Exists(TMP + @"image" + drn.Model.Replace(" ", "_") + ".png"))
                    valid = SaveFirstImage(drn.Model);
                if (valid)
                {
                    Photo0.Source = new BitmapImage(new Uri(TMP + @"image" + drn.Model.Replace(" ", "_") + ".png"));
                }
                Function.Click += new RoutedEventHandler(Button_Click_Show);
            }
        }



       private  void PopupShiwiw(object sender, RoutedEventArgs e)
        {
            myPopup.Visibility = Visibility.Visible;
            def.Visibility = Visibility.Hidden;
            if (!(drn.ParcelTransfer is null))
                ParcelOpsRFS(log.GetParcel(drn.ParcelTransfer.Id));
            else
                ParcelOpsRFS(null);

        }
        private ParcelO ParcelC(BO.Parcel parcel)
        {

            int caseNum = -1;
            if (!(parcel.ParcelCreation is null))
            {
                caseNum++;
                if (!(parcel.ParcelBinded is null))
                {
                    caseNum++;
                    if (!(parcel.ParcelPickedUp is null))
                    {
                        caseNum++;
                        if (!(parcel.ParcelDelivered is null))
                        {
                            caseNum++;
                        }
                    }
                }
            }
            if (caseNum < 1)
                throw new Exception("the parcel is not even decleared ");
            return (ParcelO)caseNum;

        }


        private void ParcelOpsRFS(BO.Parcel ? pcl)
        {

            /// operation0 - Bind 
            /// operation 3 deliver
            /// operation2 pickup
            /// operation 1 charge 
            /// operation4 realse 
            /// 

            Opeation0.IsEnabled = false;
            Opeation1.IsEnabled = false;
            Opeation2.IsEnabled = false;
            Opeation3.IsEnabled = false;
            Opeation4.IsEnabled = false;
            if (pcl is null) /// parcel only created and never binded 
            {
                Opeation0.Background = System.Windows.Media.Brushes.Green;
                Opeation0.IsEnabled = true;

            }
            else
            {
                ParcelO Stat = ParcelC(pcl);

        
                if (Stat == ParcelO.Deliver)
                {
                    //No further more operations
                }
                if (Stat == ParcelO.Bind)
                {
                    Opeation2.Background = System.Windows.Media.Brushes.LightBlue;
                    Opeation2.IsEnabled = true;
               
                }
                if (Stat == ParcelO.PickUp)
                {
                    Opeation3.Background = System.Windows.Media.Brushes.MediumVioletRed ;
                    Opeation3.IsEnabled = true;
                }
            }
            if (drn.DroneStat == BO.DroneStatuses.Free)
            {
                Opeation1.IsEnabled = true;
                Opeation1.Background = System.Windows.Media.Brushes.LightPink;
            }
            if (drn.DroneStat == BO.DroneStatuses.Matance)
            {
                Opeation4.IsEnabled = true;
                Opeation4.Background = System.Windows.Media.Brushes.Red;
            }
        }
        private void Bind(object sender, RoutedEventArgs e)
        {
            try
            {
                log.BindParcelToDrone(drn.Id);
                drn = log.GetDrone(drn.Id);
                ParcelOpsRFS(log.GetParcel(drn.ParcelTransfer.Id));
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error");
            }
            finally {
                myPopup0.IsOpen = false;


            }

        }
        private void PickUp(object sender, RoutedEventArgs e)
        {
            try
            {
                log.DronePickUp(drn.Id);
                drn = log.GetDrone(drn.Id);
                ParcelOpsRFS(log.GetParcel(drn.ParcelTransfer.Id));
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error");
            }
            finally
            {
                myPopup2.IsOpen = false;


            }
        }
        private void Deliver(object sender, RoutedEventArgs e)
        {
            try
            {
                log.DroneDelivere(drn.Id);
                drn = log.GetDrone(drn.Id);
                ParcelOpsRFS(log.GetParcel(drn.ParcelTransfer.Id));
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error");
            }
            finally
            {
                myPopup3.IsOpen = false;


            }

        }
        private void Charge(object sender, RoutedEventArgs e)
        {
            try
            {
                log.DroneCharge(drn.Id);
                drn = log.GetDrone(drn.Id);
                ParcelOpsRFS(log.GetParcel(drn.ParcelTransfer.Id));
            }
            catch (Exception err )
            {
                MessageBox.Show(err.Message, "Error");
            }
            finally
            {
                myPopup1.IsOpen = false;


            }

        }
        private void Realse(object sender, RoutedEventArgs e)
        {
            try
            {
                log.DroneReleaseCharge(drn.Id, 10);
                drn = log.GetDrone(drn.Id);
                ParcelOpsRFS(log.GetParcel(drn.ParcelTransfer.Id));
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error");
            }
            finally
            {
                myPopup4.IsOpen = false;


            }

        }

    }









        #endregion

    }