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

        private Viewbox createLabel(string text)
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

        private ComboBox creteComboBox(Array enumy)
        {

            ComboBox comboBox1 = new ComboBox();
            comboBox1.ItemsSource = enumy;

            return comboBox1;


        }

        private Grid CreateGridRow(int? row, int[] arr)
        {
            if (arr.Length != row && !(row is null))
                throw new Exception("THE ARRAY DOESNT FIT ");
            if ((row is null) && arr.Length != 1)
                throw new Exception("THE ARRAY DOESNT FIT ");
            Grid gridy = new Grid();
            for (int i = 0; i < row; i++)
            {
                RowDefinition tmp = new RowDefinition();
                tmp.Height = new GridLength(arr[i], GridUnitType.Star);
                gridy.RowDefinitions.Add(tmp);
            }
            return gridy;



        }

        private Grid CreateGridColumn(int? column, int[] arr)
        {
            if (arr.Length != column && !(column is null))
                throw new Exception("THE ARRAY DOESNT FIT ");
            if ((column is null) && arr.Length != 1)
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


    }
}