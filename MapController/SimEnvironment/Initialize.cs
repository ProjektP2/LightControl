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
    public class Initialize
    {

        #region Fields and Properties
        private Form Window;
        PictureBox SimulationRoom = new PictureBox();
        public Bitmap Map = new Bitmap("Map3.png");
        public Circle Router1 = new Circle(0, 100);
        public Circle Router2 = new Circle(0, 300);

        public DetermineLightsToActivate ActivateLights;

        private Triangulation _triangulate;
        public Triangulation Triangulate { 
            get { return _triangulate; }
            set { _triangulate = value; }
        }

        private Bounds _bound;
        public Bounds Bound {
            get { return _bound; }
            set { _bound = value; }
        }

        private LightUnitsCoords unitList;
        private DALIController _controller;
        public DALIController Controller {
            get { return _controller; }
            set { _controller = value; }
        }

        private InfoScreen _infoScreen;
        public InfoScreen InfoScreen
        {
            get { return _infoScreen; }
            set { _infoScreen = value; }
        }
        private ControlPanel _controlPanel;
        public ControlPanel ControlPanel {
            get { return _controlPanel; }
            set { _controlPanel = value; }
        }
        private InfoDrawing _info;
        public InfoDrawing Info {
            get { return _info; }
            set { _info = value; }
        }

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
        private List<LightingUnit> _nyList;
        public List<LightingUnit> NyList {
            get { return _nyList; }
            set { _nyList = value; }
        }
        private QuadTree _tree;
        public QuadTree Tree {
            get { return _tree; }
            set { _tree = value; }
        }
        #endregion

        #region Methods
        public Initialize(Form form)
        {
            Window = form;
            Window.KeyPreview = true;
            _nyList = new List<LightingUnit>();
            _bound = new Bounds(new Coords(0,0), GEngine.SimulationWidht, GEngine.SimulationHeigt);
            _tree = new QuadTree(_bound);
            Triangulate = new Triangulation(Router1, Router2);
            ActivateLights = new DetermineLightsToActivate(150, 80, 400, Triangulate); //

            SimulationRoom.Width = GEngine.SimulationWidht;
            SimulationRoom.Height = GEngine.SimulationWidht;
            SimulationRoom.Location = new Point((Form1.width / 2) - (GEngine.SimulationWidht / 2), (Form1.height / 2) - (GEngine.SimulationWidht / 2));
            SimulationRoom.Visible = true;
            SimulationRoom.Show();
            Window.Controls.Add(SimulationRoom);

        }
        
        private void CreateLightUnit()
        {
            unitList = new LightUnitsCoords(GEngine.SimulationHeigt, GEngine.SimulationWidht, 60); //
            LightUnitCoordinates = new List<LightingUnit>();
            unitList.GetLightUnitCoords(LightUnitCoordinates);
        }
        public void Start()
        {
            //Initializations
            _occupant = new Occupant(Map, Window, 'W', 'S', 'A', 'D');
            gEngine = new GEngine(Window, Map, SimulationRoom);
            CreateLightUnit();
            Controller = new DALIController(LightUnitCoordinates);
            Controller.InitGroups();
            InfoScreen = new InfoScreen(Window, Controller);
            ControlPanel = new ControlPanel(Window, Controller, LightUnitCoordinates, InfoScreen, SimulationRoom);


            //Draw info
            Info = new InfoDrawing(Window);
            Info.initWattInfo();
            Info.initSignalInfo();
            Info.InitBrugerPosWiFi();
            Info.InitBrugerPos();

            //Quadtree, initialize the graphics engine and load the visual level
            _tree.CreateQuadTree(LightUnitCoordinates);
            gEngine.init();
            gEngine.LoadLevel(LightUnitCoordinates, Router1, Router2);
        }
        #endregion

    } 
}
