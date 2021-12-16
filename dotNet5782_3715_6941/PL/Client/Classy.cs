
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Shell;
using System.Runtime.InteropServices;
using System.Windows.Interop;
namespace PL
{

        public class  Classy
        {

        public CommandClose blabla { private  set; get;  }
        public Classy(Stat gets ,Window x , System.Windows.Controls.Frame y )
        {
            blabla = new CommandClose(gets,x,y); 
        
        }
        public void  dostuff() {
            MessageBox.Show("i am a pro"); 
        
        }


        }

}