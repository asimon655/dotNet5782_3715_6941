using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace PL
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Page
    {
        public IBL.Ibl dat { set; get; }
        public Stat status; 
        public Window1(Stat gets , IBL.Ibl dat)
        {
            this.dat = dat;
            status = gets; 
            InitializeComponent();
           
            //Btn1.Content ="Idrones - SA ";  
            SolidColorBrush brush = new SolidColorBrush(Colors.Red);
            Btn1.Background = brush;
            ColorAnimation anima = new ColorAnimation(Colors.GreenYellow, Colors.OrangeRed,
                new Duration(TimeSpan.FromSeconds(5)));
            anima.AutoReverse = true;
            anima.RepeatBehavior = RepeatBehavior.Forever;
            brush.BeginAnimation(SolidColorBrush.ColorProperty, anima);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            status.a = false;
            this.NavigationService.Navigate(new MainWindow(dat,status));
}
        //new MainWindow(dat).ShowDialog();
    

        private void GifLoader_Unloaded(object sender, RoutedEventArgs e)
        {


            MediaElement media = (MediaElement)sender;
            media.LoadedBehavior = MediaState.Manual;
            media.Position = TimeSpan.FromMilliseconds(1);
            media.Play();



        }
        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            status.a = false;
            this.NavigationService.Navigate(new Page1(dat) );
        }
    }
}
