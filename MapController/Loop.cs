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

        List<LightingUnit> nyList;
        QuadTree tree;
        List<LightingUnit> LightUnitCoordinates;
        Occupant NewOccupant = new Occupant();

        public Loop(Form form)
        {
            Window = form;
            Window.KeyDown += new KeyEventHandler(this.Form1_KeyDown);
            Window.KeyUp += new KeyEventHandler(this.Form1_KeyUp);
            Window.KeyPreview = true;
            nyList = new List<LightingUnit>();
            Bounds bound = new Bounds(0, 0, GEngine.SimulationWidht, GEngine.SimulationHeigt);
            tree = new QuadTree(bound, false, null);
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

        internal Occupant Occupant
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        internal QuadTree QuadTree
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
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

        internal DetermineLightsToActivate DetermineLightsToActivate
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        internal LightUnitsCoords LightUnitsCoords
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        private void CreateLightUnit()
        {
            LightUnitsCoords lol2 = new LightUnitsCoords(GEngine.SimulationHeigt, GEngine.SimulationWidht, 30); // 
            LightUnitCoordinates = new List<LightingUnit>();
            lol2.GetLightUnitCoords(ref LightUnitCoordinates);
        }

        public void Position(Point point)
        {
            NewOccupant.UpdatePositions(point.X, point.Y);
            nyList = tree.RadiusSearchQuery(NewOccupant.Position1, 100);
            ActivatedLightingUnitsOnUser = DetermineLightsToActivate.LightsToActivateOnUser(NewOccupant, nyList);
            //ActivatedLightingUnitsOnUser = DetermineLightsToActivate.LightsToActivateOnUser(NewOccupant, LightUnitCoordinates);
            ActivatedLightingUnitsInPath = DetermineLightsToActivate.LightsToActivateInPath(NewOccupant, LightUnitCoordinates);

            NewOccupant.Position1.x = NewOccupant.Position2.x;
            NewOccupant.Position1.y = NewOccupant.Position2.y;
        }

        public void Start()
        {
            occupantPosition = new Point(4 * 32, 4 * 32);
            gEngine = new GEngine(Window, Map);
            occupantMove = new OccupantMove(Map);

            CreateLightUnit();
            // Skal flyttes
            foreach (var item in LightUnitCoordinates)
            {
                tree.InsertNode(new QuadTreeNode(item));
            }
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
