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

        LightControl.Fps FpsCounter;
        Bitmap Map;
        Form window;


        //Starting position


        public GEngine(Form form, Bitmap map)
        {
            window = form;
            Map = map;
        }
        public void init()
        {
            
            grapihicsDraw = new GraphicsDraw(window, Map);
            
            FpsCounter = new LightControl.Fps();
        }
        //Load the Map from a picture
        public void LoadLevel(List<LightingUnit> LightingUnits)
        {
            //Fejlen er her et sted
            grapihicsDraw.Begin();
            grapihicsDraw.DrawMap();
            grapihicsDraw.DrawLamps(LightingUnits);
            Console.WriteLine("her int");
        }
        public void Drawing(Point EmployerPosition, List<LightingUnit> LightingUnits)
        {
            grapihicsDraw.Position();
                grapihicsDraw.DrawLight(LightingUnits);
                grapihicsDraw.Draw(FpsCounter.fps, EmployerPosition);
                FpsCounter.FPS();

        }
        
        //Move the player
       
    }
}
