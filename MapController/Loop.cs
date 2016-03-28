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

        Point occupantPosition;

        public bool Running = true;

        List<LightingUnit> ActivatedLightingUnitsOnUser = new List<LightingUnit>();
        List<LightingUnit> ActivatedLightingUnitsInPath = new List<LightingUnit>();

        List<LightingUnit> LightUnitCoordinates;
        Occupant NewOccupant = new Occupant();

        private void CreateLightUnit()
        {
            LightUnitsCoords lol2 = new LightUnitsCoords(GEngine.SimulationHeigt, GEngine.SimulationWidht, 30); // 
            LightUnitCoordinates = new List<LightingUnit>();
            lol2.GetLightUnitCoords(ref LightUnitCoordinates);
        }

        public void Position(Point point)
        {
            NewOccupant.UpdatePositions(point.X, point.Y);

            ActivatedLightingUnitsOnUser = DetermineLightsToActivate.LightsToActivateOnUser(NewOccupant, LightUnitCoordinates);
            ActivatedLightingUnitsInPath = DetermineLightsToActivate.LightsToActivateInPath(NewOccupant, LightUnitCoordinates);

            NewOccupant.Position1.x = NewOccupant.Position2.x;
            NewOccupant.Position1.y = NewOccupant.Position2.y;
        }


        public Loop(Form form)
        {
            Window = form;
            Window.KeyDown += new KeyEventHandler(this.Form1_KeyDown);
            Window.KeyUp += new KeyEventHandler(this.Form1_KeyUp);
            //Window.KeyPreview = true;
        }
        public void Start()
        {
            occupantPosition = new Point(4 * 32, 4 * 32);
            gEngine = new GEngine(Window, Map);
            occupantMove = new OccupantMove(Map);

            CreateLightUnit();

            gEngine.init();
            gEngine.LoadLevel(LightUnitCoordinates);
            calculationLoop();
        }

        private void calculationLoop()
        {
            while (Running)
            {
              Application.DoEvents();
                occupantPosition = occupantMove.PlayerMove(occupantPosition);
                Position(occupantPosition);
                gEngine.Drawing(occupantPosition, ActivatedLightingUnitsOnUser);
            }
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
