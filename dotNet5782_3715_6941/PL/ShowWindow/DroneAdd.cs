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

        public Window2(BlApi.Ibl x, Page pg)
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
            Array StionsIds = (from stat in x.GetStaions() select stat.Id).ToArray();
            ComboBox input1 = creteComboBox(StionsIds);
            Grid input1GridC = CreateGridColumn(3, new int[3] { 1, 8, 1 });
            Grid input1GridR = CreateGridRow(3, new int[3] { 1, 1, 1 });
            Viewbox text2 = creteLabel("Wight:  ");
            Grid.SetColumn(text1, 0);
            Grid.SetRow(input1GridC, 1);
            Grid.SetColumn(input1GridR, 1);
            Grid.SetColumn(input1, 1);

            Grid.SetColumn(text2, 2);
            Grid rowsComb = CreateGridRow(3, new int[3] { 1, 1, 1 });
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
            colums.Children.Add(input1GridR);
            input1GridR.Children.Add(input1GridC);
            input1GridC.Children.Add(input1);
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
    }
}