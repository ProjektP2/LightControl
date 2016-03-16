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

        GraphicsDraw grapihicsDraw;
        Fps FpsCounter;

        Bitmap Map;
        Form window;

        public GEngine(Form form, Bitmap map)
        {
            window = form;
            Map = map;
        }
        public void init()
        {
            grapihicsDraw = new GraphicsDraw(window, Map);
            FpsCounter = new Fps();
        }
        //Load the Map from a picture
        public void LoadLevel()
        {
            grapihicsDraw.Begin();
            grapihicsDraw.DrawMap();
            grapihicsDraw.DrawLamps();
            
        }
        public void Drawing(Point EmployerPosition)
        {  
                grapihicsDraw.DrawLight();
                grapihicsDraw.Draw(FpsCounter.fps, EmployerPosition);
                grapihicsDraw.Position();
                FpsCounter.FPS();
        }
    }
}
