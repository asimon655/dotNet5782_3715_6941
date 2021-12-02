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
using EO.WebBrowser;


namespace PL
{
    public class CommandClose : ICommand
    {
        public Stat ?  status;
        public Window win; 
        public CommandClose(Stat gets ,Window x )
        {
            MessageBox.Show("hello2");
            status = gets;
            win = x; 
        
        }
        public event EventHandler CanExecuteChanged;
        //
        // Summary:
        //     Occurs when changes occur that affect whether or not the command should execute.


        //
        // Summary:
        //     Defines the method that determines whether the command can execute in its current
        //     state.
        //
        // Parameters:
        //   parameter:
        //     Data used by the command. If the command does not require data to be passed,
        //     this object can be set to null.
        //
        // Returns:
        //     true if this command can be executed; otherwise, false.
        public bool CanExecute(object? parameter)
        {
            MessageBox.Show("hello3");
            return (status is null ? false : status.a);
        }
        //
        // Summary:
        //     Defines the method to be called when the command is invoked.
        //
        // Parameters:
        //   parameter:
        //     Data used by the command. If the command does not require data to be passed,
        //     this object can be set to null.
        public void Execute(object? parameter )
        {
            MessageBox.Show("hello");
           win.Close(); 
        
        
        }
    }
}