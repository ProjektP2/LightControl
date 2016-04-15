using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using SimEnvironment;
using Triangulering;
using TreeStructure;
using MapController.SimEnvironment;

namespace LightControl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using System.Drawing;
    using SimEnvironment;
    using Triangulering;
    using TreeStructure;

        class Loop
    {
        public Form Window;
        private Initialize _initialization;
        public bool Running = true;
        
        public Loop(Form form)
        {
            Window = form;
            Window.FormClosing += new FormClosingEventHandler(this.Form1_FormClosing);
        }

        public void calculationLoop()
        {
            _initialization = new Initialize(Window);
            _initialization.Start();
            while (Running)
            {
                Application.DoEvents();
                _initialization.Position();
                _initialization.gEngine.Drawing(_initialization.occupant.Position2, _initialization.LightUnitCoordinates,
                    _initialization.Router1, _initialization.Router2);
            }
        }
        
        public void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Running = false;
            Application.Exit();
        }
    } 
}
