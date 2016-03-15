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
        Bitmap  Map = new Bitmap("Map3.png");
        GEngine gEngine;
        OccupantMove occupantMove;

        Point EmployerPosition;


        List<LightingUnit> ActivatedLightingUnitsOnUser = new List<LightingUnit>();
        List<LightingUnit> ActivatedLightingUnitsInPath = new List<LightingUnit>();

        List<LightingUnit> LightUnitCoordinates;
        Coords PreviousPosition = new Coords(99999, 99999);

        Coords PositionCoords = new Coords();

        private void CreateLamps()
        {
            LightUnitsCoords lol2 = new LightUnitsCoords(GEngine.SimulationHeigt, GEngine.SimulationWidht, 30); 
            LightUnitCoordinates = new List<LightingUnit>();
            lol2.GetLightUnitCoords(ref LightUnitCoordinates);
        }


        public bool Running = true;


        public Loop(Form form)
        {
            Window = form;
        }


        public void Position()
        {
            double xx = PositionCoords.x / GEngine.TileSize;
            double YY = PositionCoords.y / GEngine.TileSize;

            ActivatedLightingUnitsOnUser = DetermineLightsToActivate.LightsToActivateOnUser(PositionCoords, LightUnitCoordinates);
            ActivatedLightingUnitsInPath = DetermineLightsToActivate.LightsToActivateInPath(PreviousPosition, PositionCoords, LightUnitCoordinates);

            PreviousPosition.x = PositionCoords.x;
            PreviousPosition.y = PositionCoords.y;

        }



        public void Start()
        {
           
            EmployerPosition = new Point(4 * 32, 4 * 32);
            gEngine = new GEngine(Window, Map);
            occupantMove = new OccupantMove(Map);
            
            gEngine.init();
            gEngine.LoadLevel(LightUnitCoordinates);
            calculationLoop();
        }

        private void calculationLoop()
        {
            
            do
            {
                Application.DoEvents();
                EmployerPosition = occupantMove.PlayerMove(EmployerPosition);
                Position();
                gEngine.Drawing(EmployerPosition, ActivatedLightingUnitsOnUser);


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
