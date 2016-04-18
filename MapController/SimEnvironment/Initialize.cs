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

        #region Fields and Properties
        private Form Window;
        public Bitmap Map = new Bitmap("Map3.png");
        public Circle Router1 = new Circle(0, 100);
        public Circle Router2 = new Circle(0, 300);

        public DetermineLightsToActivate ActivateLights;

        public Triangulation Triangulation;

        private Bounds _bound;

        public DALIController Controller;
        public ControlPanel ControlPanel;
        InfoDrawing Info;

        public Occupant occupant
        {
            get { return _occupant; }
            set { _occupant = value; }
        }
        private Occupant _occupant;
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
        #endregion

        #region Methods
        public Initialize(Form form)
        {
            Window = form;
            Window.KeyPreview = true;
            nyList = new List<LightingUnit>();
            _bound = new Bounds(0, 0, GEngine.SimulationWidht, GEngine.SimulationHeigt);
            tree = new QuadTree(_bound);
            Triangulation = new Triangulation(Router1, Router2);
            ActivateLights = new DetermineLightsToActivate(200, 60, 400, Triangulation);

        }
        public void Position()
        {
            //Moves the player
            _occupant.Update();

            //Updates the occupants WiFi position
            Triangulation.TriangulatePositionOfSignalSource(_occupant, Router1, Router2);

            Query query = new RadiusSearchQuery(100, _bound, tree);
            StartTreeSearch startSearch = new StartTreeSearch();
            List<LightingUnit> newlist = new List<LightingUnit>();

            nyList = startSearch.SearchQuery(new Coords(_occupant.WiFiPosition1.x, _occupant.WiFiPosition1.y), query);

            ActivateLights.FindUnitsToActivate(LightUnitCoordinates, _occupant);

            Controller.IncrementLights(LightUnitCoordinates);
            Info.WattUsageInfo(Controller.Wattusage());

            Info.SignalInfo(Router1.Radius, Router2.Radius);
            Info.BrugerWiFi(_occupant.WiFiPosition2);
            Info.Brugerpos(_occupant.Position2);
        }
        private void CreateLightUnit()
        {
            LightUnitsCoords unitList = new LightUnitsCoords(GEngine.SimulationHeigt, GEngine.SimulationWidht, 30);
            LightUnitCoordinates = new List<LightingUnit>();
            unitList.GetLightUnitCoords(ref LightUnitCoordinates);
        }
        public void Start()
        {
            //Initializations
            _occupant = new Occupant(Map, Window, 'W', 'S', 'A', 'D');
            gEngine = new GEngine(Window, Map);
            loop = new Loop(Window);
            CreateLightUnit();
            Controller = new DALIController(LightUnitCoordinates);
            Controller.InitGroups();
            ControlPanel = new ControlPanel(Window, Controller, LightUnitCoordinates);

            //Draw info
            Info = new InfoDrawing(Window);
            Info.initWattInfo();
            Info.initSignalInfo();
            Info.InitBrugerPosWiFi();
            Info.InitBrugerPos();

            //Quadtree, initialize the graphics engine and load the visual level
            tree.CreateQuadTree(LightUnitCoordinates);
            gEngine.init();
            gEngine.LoadLevel(LightUnitCoordinates, Router1, Router2);
        }
        #endregion

    } 
}
