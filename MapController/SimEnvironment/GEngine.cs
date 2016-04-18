using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using LightControl;
using Triangulering;

namespace SimEnvironment
{
    class GEngine
    {
        #region Constructors
        public GEngine(Form form, Bitmap map)
        {
            window = form;
            Map = map;
        }
        #endregion

        #region Fields and Properties
        public const int SimulationHeigt = 640;
        public const int SimulationWidht = 640;
        public const int TileSize = 32;

        public GraphicsDraw graphicsDraw;
        public Fps FpsCounter;
        //InfoDrawing info;

        public Bitmap Map;
        public Form window;

        #endregion

        #region Methods
        public void init()
        {
            graphicsDraw = new GraphicsDraw(window, Map);
            FpsCounter = new Fps();
        }

        //Load the Map from a picture
        public void LoadLevel(List<LightingUnit> LightUnitCoordinates, Circle Router1, Circle Router2)
        {
            graphicsDraw.InitBitMaps();
            graphicsDraw.LoadMapIntoBitMap(Router1, Router2);
            graphicsDraw.LoadLampsIntoBitMap(LightUnitCoordinates);

        }
        public void Drawing(Coords EmployerPosition, List<LightingUnit> ActivatedLightingUnitsOnUser, Circle router1, Circle router2)
        {
            graphicsDraw.LoadLightIntoBitMap(ActivatedLightingUnitsOnUser);
            graphicsDraw.Draw(FpsCounter.fps, EmployerPosition, router1, router2);
            FpsCounter.FPS();
        }
        #endregion
    }
}
