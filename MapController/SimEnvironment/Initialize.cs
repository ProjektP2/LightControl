using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightControl;
using Triangulering;
using System.Windows.Forms;
using SimEnvironment;
using TreeStructure;
using System.Drawing;

namespace MapController.SimEnvironment
{
    //This class is used to organize the initialization of all objects required for simulating light control. 
    class Initialize
    {
        private Form Window;
        public Bitmap Map = new Bitmap("Map3.png");
        public Point occupantPosition;

        public OccupantMove OccupantMove
        {
            get { return _occupantMove; }
            set { _occupantMove = value; }
        }
        private OccupantMove _occupantMove;
        public GEngine gEngine
        {
            get { return _gEngine; }
            private set { _gEngine = value; }
        }
        private GEngine _gEngine;

        public List<LightingUnit> ActivatedLightingUnitsOnUser
        {
            get { return _activatedLightingUnitsOnUser; }
            set { _activatedLightingUnitsOnUser = value; }
        }
        private List<LightingUnit> _activatedLightingUnitsOnUser;

        public Loop loop;
      
        public bool Running = true;

        List<LightingUnit> LightUnitCoordinates;
        List<LightingUnit> nyList;
        QuadTree tree;
        Occupant NewOccupant = new Occupant();
        List<LightingUnit> AllLightingUnits = new List<LightingUnit>();
        List<LightingUnit> ActivatedLightingUnitsInPath = new List<LightingUnit>();

        public Initialize(Form form)
        {
            Window = form;
            ActivatedLightingUnitsOnUser = new List<LightingUnit>();
            nyList = new List<LightingUnit>();
            Bounds bound = new Bounds(0, 0, GEngine.SimulationWidht, GEngine.SimulationHeigt);
            tree = new QuadTree(bound, false, null);
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
        private void CreateLightUnit()
        {
            LightUnitsCoords lol2 = new LightUnitsCoords(GEngine.SimulationHeigt, GEngine.SimulationWidht, 30); // 
            LightUnitCoordinates = new List<LightingUnit>();
            lol2.GetLightUnitCoords(ref LightUnitCoordinates);
        }
        public void Start()
        {
            occupantPosition = new Point(4 * 32, 4 * 32);
            _occupantMove = new OccupantMove(Map, Window);
            gEngine = new GEngine(Window, Map);
            loop = new Loop(Window);
            CreateLightUnit();
            
            foreach (var item in LightUnitCoordinates)
            {
                tree.InsertNode(new QuadTreeNode(item));
            }
            gEngine.init();
            gEngine.LoadLevel(LightUnitCoordinates);
        }    
    } 
}
