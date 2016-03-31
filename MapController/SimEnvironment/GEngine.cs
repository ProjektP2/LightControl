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
        // Size of the const.
        public const int SimulationHeigt = 640;
        public const int SimulationWidht = 640;
        public const int TileSize = 32;

        GraphicsDraw graphicsDraw;
        Fps FpsCounter;
        //InfoDrawing info;

        Bitmap Map;
        Form window;

        public GEngine(Form form, Bitmap map)
        {
            window = form;
            Map = map;
        }

        internal Fps Fps
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        internal GraphicsDraw GraphicsDraw
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        public void init()
        {
            graphicsDraw = new GraphicsDraw(window, Map);
            FpsCounter = new Fps();
            //info = new InfoDrawing(window);
            //info.init();
        }
        //Load the Map from a picture
        public void LoadLevel(List<LightingUnit> LightUnitCoordinates)
        {
            graphicsDraw.InitBitMaps();
            graphicsDraw.DrawMap();
            graphicsDraw.DrawLamps(LightUnitCoordinates);
            
        }
        public void Drawing(Point EmployerPosition, List<LightingUnit> ActivatedLightingUnitsOnUser)
        {  
                graphicsDraw.DrawLight(ActivatedLightingUnitsOnUser);
                graphicsDraw.Draw(FpsCounter.fps, EmployerPosition);
                FpsCounter.FPS();
            //info.LightINFO(ActivatedLightingUnitsOnUser);
        }
    }
}
