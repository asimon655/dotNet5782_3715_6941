using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for CostumerShow.xaml
    /// </summary>
    public partial class CostumerShow : Window
    {
        #region Fields 
        BlApi.Ibl dat;
        static internal string TMP = System.IO.Path.GetTempPath();
        #endregion
        public CostumerShow(BlApi.Ibl dat , BO.Customer cst)
        {
            this.dat = dat;
            InitializeComponent();
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
    }
}
