﻿using System;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Shell;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using EO.WebBrowser;

namespace PL
{
    public class Stat
    { 
        public bool a { set; get;  }
    }
    /// <summary>
    /// Interaction logic for Window3.xaml
    /// </summary>
    public partial class Window3 : Window
    {

 
           protected override void OnClosing(CancelEventArgs e)
        {
            if (status.a)
                e.Cancel = false;
            else
            {
                MessageBox.Show("Cnat Close", "Forbidden premission", MessageBoxButton.OK, MessageBoxImage.Warning);
                e.Cancel = true;
            }
        }


        public  Stat status;
  

        public Window3()
        {

        // WindowStyle =WindowStyle.None;
        status = new Stat (){ a = true };
            InitializeComponent();
            Main.NavigationService.Navigate(new Window1(status));
            Title = (Main.NavigationService.Content is null ? "None" : Main.NavigationService.Content.GetType().Name.ToString() ) ;
           // blabla = new CommandClose(status);
            object blublo = this;
            this.DataContext = new Classy(status,this); 

        }

        

     


    }
}
