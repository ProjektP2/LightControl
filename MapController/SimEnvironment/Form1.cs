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
            init.Start();
            loop.calculationLoop();
        }              
    } 
}
