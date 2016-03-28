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

namespace SimEnvironment
{
    public partial class Form1:Form
    {
        public static int width = Screen.PrimaryScreen.WorkingArea.Width;
        public static int height = Screen.PrimaryScreen.WorkingArea.Height;

        LightControl.Loop loop;
        public Form1()
        {
            InitializeComponent();
        }

        internal LightControl.Loop Loop
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Starts when the Form i Loaded
            this.Show();
            this.Focus();
            this.Width = width;
            this.Height = height;
            loop = new LightControl.Loop(this);

            AllocConsole();
            loop.Start();
        }

        //Console Window to Debug 
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            loop.Form1_FormClosing(sender, e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("hej");
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

        }
    }
}
