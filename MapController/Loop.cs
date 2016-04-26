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

        public class Loop
    {
        public Form Window;
        private Initialize _init;
        public bool Running = true;

        public Loop(Form form, Initialize Initialized)
        {
            Window = form;
            _init = Initialized;
            Window.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
        }

        public void calculationLoop()
        {
            while (Running)
            {
                Application.DoEvents();
                Position();
                UpdateLights();
                DisplayInfo();
                DrawEverything();
            }
        }

        public void Position()
        {
            //Moves the player
            _init.occupant.Update();

            //Updates the occupants WiFi position
            _init.Triangulate.TriangulatePositionOfSignalSource(_init.occupant, _init.Router1, _init.Router2);

        }

        public void UpdateLights()
        {
            Query radiusQuery = new RadiusSearchQuery(80, _init.Bound, _init.Tree);
            Query vectorQuery = new VectorSearchQuery(_init.Bound, _init.Tree, _init.occupant, _init.ActivateLights);
            StartTreeSearch startSearch = new StartTreeSearch(_init.Tree);
            _init.NyList = startSearch.SearchQuery(new Coords(_init.occupant.WiFiPosition1.x, _init.occupant.WiFiPosition1.y), radiusQuery, vectorQuery);

            _init.ActivateLights.FindUnitsToActivate(_init.NyList, _init.occupant);

            _init.Controller.IncrementLights(_init.LightUnitCoordinates);
        }

        public void DisplayInfo()
        {
            _init.Info.WattUsageInfo(_init.Controller);

            _init.Info.SignalInfo(_init.Router1.Radius, _init.Router2.Radius);
            _init.Info.BrugerWiFi(_init.occupant.WiFiPosition2);
            _init.Info.Brugerpos(_init.occupant.Position2);
        }

        public void DrawEverything()
        {
            _init.gEngine.Drawing(_init.occupant.Position2, _init.LightUnitCoordinates,
                                                _init.Router1, _init.Router2);
        }

        public void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Running = false;
            Application.Exit();
        }
    } 
}
