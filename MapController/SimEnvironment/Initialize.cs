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
        public Circle Router1 = new Circle(0,100);
        public Circle Router2 = new Circle(0,300);

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

        public Initialize(Form form)
        {
            Window = form;
            Window.KeyPreview = true;
            nyList = new List<LightingUnit>();
            _bound = new Bounds(0, 0, GEngine.SimulationWidht, GEngine.SimulationHeigt);
            tree = new QuadTree(_bound);
            
        }
        public void Position()
        {
            //NewOccupant.UpdatePositions(point.X, point.Y);
            _occupant.Update();
            Triangulate.TriangulatePositionOfSignalSource(_occupant, Router1, Router2);

            Query query = new RadiusSearchQuery(100, _bound, tree);
            StartTreeSearch startSearch = new StartTreeSearch();
            List<LightingUnit> newlist = new List<LightingUnit>();

            nyList = startSearch.SearchQuery(new Coords(_occupant.Position1.x, _occupant.Position1.y), query);

            DetermineLightsToActivate.FindUnitsToActivate(LightUnitCoordinates, _occupant);

            Controller.IncrementLights(ref LightUnitCoordinates);
            Info.WattUsageInfo(Controller.Wattusage());
            
            Info.SignelInfo(Router1.Radius, Router2.Radius);
            Info.BrugerWiFi(_occupant.WiFiPosition2);
            Info.Brugerpos(_occupant.Position2);
        }
        private void CreateLightUnit()
        {
            LightUnitsCoords lol2 = new LightUnitsCoords(GEngine.SimulationHeigt, GEngine.SimulationWidht, 30); // 
            LightUnitCoordinates = new List<LightingUnit>();
            lol2.GetLightUnitCoords(ref LightUnitCoordinates);


        }
        public void Start()
        {
            _occupant = new Occupant(Map, Window, 'W', 'S', 'A', 'D');
            gEngine = new GEngine(Window, Map);
            loop = new Loop(Window);
            CreateLightUnit();
            Controller = new DALIController(LightUnitCoordinates);
            Controller.InitGroups();
            ControlPanel = new ControlPanel(Window, Controller, LightUnitCoordinates);

            Info = new InfoDrawing(Window);
            Info.initWattInfo();
            Info.initSignalInfo();
            Info.InitBrugerPosWiFi();
            Info.InitBrugerPos();

            tree.CreateQuadTree(LightUnitCoordinates);
            gEngine.init();
            gEngine.LoadLevel(LightUnitCoordinates, Router1, Router2);
        }
    } 
}
