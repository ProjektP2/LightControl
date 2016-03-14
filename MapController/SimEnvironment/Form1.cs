using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Organising;

namespace SimEnvironment
{
    public partial class Window : Form
    {
        public Window()
        {
            InitializeComponent();
        }

        private void Window_Load(object sender, EventArgs e )
        {
            //Starts when the Form i Loaded
            
            AllocConsole();
            this.Show();
            this.Focus();
            

        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            //Detect KeyDown
            //gEngine.Press(e);
        }
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            //Detect KeyUp
            //gEngine.NoPress(e);
        }

        //Console Window to Debug 
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        private void Window_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.ProgramRunning = false;
        }

    }
}
