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
        private Bounds _bound;

        public DALIController Controller = new DALIController();
        InfoDrawing Info;

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

        public Loop loop;
      
        public bool Running = true;

        public List<LightingUnit> LightUnitCoordinates = new List<LightingUnit>();
        List<LightingUnit> nyList;
        QuadTree tree;
        Occupant NewOccupant = new Occupant();

        public Initialize(Form form)
        {
            Window = form;
            nyList = new List<LightingUnit>();
            _bound = new Bounds(0, 0, GEngine.SimulationWidht, GEngine.SimulationHeigt);
            tree = new QuadTree(_bound);
        }
        public void Position(Point point)
        {
            NewOccupant.UpdatePositions(point.X, point.Y);

            Query query = new RadiusSearchQuery(100, _bound, tree);
            StartTreeSearch startSearch = new StartTreeSearch();
            List<LightingUnit> newlist = new List<LightingUnit>();

            nyList = startSearch.SearchQuery(new Coords(NewOccupant.Position1.x, NewOccupant.Position1.y), query);

            DetermineLightsToActivate.FindUnitsToActivate(ref LightUnitCoordinates, NewOccupant);

            Controller.IncrementLights(ref LightUnitCoordinates);
            Info.WattUsageInfo(Controller.Wattusage());

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

            Controller.InitGroups();
            Info = new InfoDrawing(Window);
            Info.initWattInfo();

            tree.CreateQuadTree(LightUnitCoordinates);
            gEngine.init();
            gEngine.LoadLevel(LightUnitCoordinates);
        }    
    } 
}
