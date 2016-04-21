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
using MapController.SimEnvironment;
using LightControl;

namespace SimEnvironment
{
    public partial class Form1 : Form
    {
        public static int width = Screen.PrimaryScreen.WorkingArea.Width;
        public static int height = Screen.PrimaryScreen.WorkingArea.Height;

        
        public Form1()
        {
            InitializeComponent();
        }
        Loop loop;
        Initialize init;
        private void Form1_Load(object sender, EventArgs e)
        {
            //Starts when the Form i Loaded
            this.Show();
            this.Focus();
            this.Width = width;
            this.Height = height;
            init = new Initialize(this);
            loop = new Loop(this, init);
            AllocConsole();
            init.Start();
            loop.calculationLoop();
        }

        //Console Window to Debug 
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        static extern bool AllocConsole();
                
        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Stop Niko!");
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

        }

        internal Initialize Initialize
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }
    } 
}
