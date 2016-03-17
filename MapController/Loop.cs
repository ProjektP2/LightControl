using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using SimEnvironment;
using Triangulering;

namespace LightControl
{
    class Loop
    {
        Form Window;
        Bitmap Map = new Bitmap("Map3.png");
        GEngine gEngine;
        OccupantMove occupantMove;

        Point EmployerPosition;


        public bool Running = true;


        public Loop(Form form)
        {
            Window = form;
        }

        internal OccupantMove OccupantMove
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        internal GEngine GEngine
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        internal Fps Fps
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        public void Start()
        {

            EmployerPosition = new Point(4 * 32, 4 * 32);
            gEngine = new GEngine(Window, Map);
            occupantMove = new OccupantMove(Map);

            gEngine.init();
            gEngine.LoadLevel();
            calculationLoop();
        }

        private void calculationLoop()
        {
            do
            {
                Application.DoEvents();
                EmployerPosition = occupantMove.PlayerMove(EmployerPosition);
                gEngine.Drawing(EmployerPosition);
            } while (Running);
            Application.Exit();
        }


        public void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            occupantMove.Press(e);
        }
        public void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            occupantMove.NoPress(e);
        }
        public void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Running = false;
            Application.Exit();
        }

    }
}
