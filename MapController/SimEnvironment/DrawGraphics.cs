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
    class GraphicsDraw
    {

        Graphics BBG;
        Rectangle sRect;
        Rectangle dRect;
        Graphics G;
        Graphics GMap;
        Graphics Glamps;
        Graphics GCircle;
        
        Bitmap Map;
        Bitmap player;
        Bitmap teils;

        Bitmap BB;
        Bitmap MAPMAP;
        Bitmap Lamps;
        Bitmap Light;

        Form window;
        List<LightingUnit> ActivatedLightingUnitsOnUser = new List<LightingUnit>();
        List<LightingUnit> ActivatedLightingUnitsInPath = new List<LightingUnit>();
        List<Coords> LightingUnitCoordinates = new List<Coords>();
        
        List<LightingUnit> LightUnitCoordinates;
        Coords PreviousPosition = new Coords(99999,99999);
        
        Coords PositionCoords = new Coords();
        
        Coords MouseCoords = new Coords();

        public GraphicsDraw(Form form, Bitmap map)
        {
            LightUnitsCoords lol2 = new LightUnitsCoords(GEngine.FormHeigt, GEngine.FormWidht, 32);
            LightUnitCoordinates = new List<LightingUnit>();
            lol2.GetLightUnitCoords(ref LightUnitCoordinates);
            Map = map;
            window = form;
        }
        public void Begin()
        {
            G = window.CreateGraphics();
            GMap = window.CreateGraphics();
            Glamps = window.CreateGraphics();
            GCircle = window.CreateGraphics();
            BB = new Bitmap(GEngine.FormWidht, GEngine.FormHeigt);
            MAPMAP = new Bitmap(GEngine.FormWidht, GEngine.FormHeigt);
            Lamps = new Bitmap(GEngine.FormWidht, GEngine.FormHeigt);
            Light = new Bitmap(GEngine.FormWidht, GEngine.FormHeigt);
            player = new Bitmap("Player.png");
            teils = new Bitmap("Teils.png");

        }
        public void Position()
        {
            double xx = PositionCoords.x / 32;
            double YY = PositionCoords.y / 32;
            MouseCoords.x = Convert.ToInt32((Math.Floor(xx)));
            MouseCoords.y = Convert.ToInt32((Math.Floor(YY)));
            
            ActivatedLightingUnitsOnUser = DetermineLightsToActivate.LightsToActivateOnUser(MouseCoords, LightUnitCoordinates);
            ActivatedLightingUnitsInPath = DetermineLightsToActivate.LightsToActivateInPath(PreviousPosition, MouseCoords, LightUnitCoordinates);
            
            PreviousPosition.x = MouseCoords.x;
            PreviousPosition.y = MouseCoords.y;

        }
        public void DrawMap()
        {
            GMap = Graphics.FromImage(MAPMAP);
            for (int x = 0; x < Map.Width; x++)
            {
                for (int y = 0; y < Map.Height; y++)
                {
                    //Determine the color code of the pixel on the map
                    // And draw to the window
                    Color PixelCode = Map.GetPixel(x, y);
                    string pixelColorStringValue =
                        PixelCode.R.ToString("D3") + "" +
                        PixelCode.G.ToString("D3") + "" +
                        PixelCode.B.ToString("D3") + "";
                    GetSurce(pixelColorStringValue);
                    dRect = new Rectangle((x * GEngine.TileSize), (y * GEngine.TileSize), GEngine.TileSize, GEngine.TileSize);
                    GMap.DrawImage(teils, dRect, sRect, GraphicsUnit.Pixel);
                }

            }
        }
        public void DrawLamps()
        {
            Glamps = Graphics.FromImage(Lamps);
            foreach (var item in LightUnitCoordinates)
            {

                int xx = Convert.ToInt32(item.x);
                int yy = Convert.ToInt32(item.y);
                sRect = new Rectangle(0, 0, GEngine.TileSize, GEngine.TileSize);
                player.MakeTransparent(Color.CadetBlue);
                Glamps.DrawImage(player, xx, yy, sRect, GraphicsUnit.Pixel);
                //Console.WriteLine(item.x);
            }

        }
        public void DrawLight()
        {
            int radius = 10;
            int tal = 20;
            double procent = 1.0;
            for (double i = procent; i >= 0; i-=0.1)
            {
                tal--;
                double volume = 255 - (255 * (procent-i));
                int vm = Convert.ToInt32((Math.Floor(volume)));
                Console.WriteLine((radius+vm));
                //Console.WriteLine(vm);

                GCircle = Graphics.FromImage(Light);
                sRect = new Rectangle((6*32)-radius, (6*32)-radius, radius+vm , radius+vm);
                SolidBrush myBrush = new SolidBrush(Color.FromArgb(100, 000, 000, 000));
                GCircle.FillEllipse(myBrush, sRect);
                //GCircle.FillRectangle(myBrush, sRect);
            }
        }
        private void GetSurce(string pixelColorStringValue)
        {
            switch (pixelColorStringValue)
             {
                //Diffrent Color codes, reads diffrent locations on the tile bitmap
                 case "255020147": sRect = new Rectangle(0, 0, GEngine.TileSize, GEngine.TileSize); break;
                 case "255255000": sRect = new Rectangle(32, 0, GEngine.TileSize, GEngine.TileSize); break;
                 case "075000130": sRect = new Rectangle(0, 0, GEngine.TileSize, GEngine.TileSize); break;
                 case "240128128": sRect = new Rectangle(32, 0, GEngine.TileSize, GEngine.TileSize); break;
                 default: new Rectangle(0, 0, GEngine.TileSize, GEngine.TileSize); break;
             }
        }

        public void Draw(int xpos, int ypos, int fps)
        {
            //Player possion
            PositionCoords.x = xpos;
            PositionCoords.y = ypos;
            // Draw the teils to the 

           /*for (int x = 0; x < Map.Width; x++)
            {
                for (int y = 0; y < Map.Height; y++)
                {
                    //Determine the color code of the pixel on the map
                    // And draw to the window
                    Color PixelCode = Map.GetPixel(x, y);
                    string pixelColorStringValue =
                        PixelCode.R.ToString("D3") + "" +
                        PixelCode.G.ToString("D3") + "" +
                        PixelCode.B.ToString("D3") + "";
                    GetSurce(pixelColorStringValue);
                    dRect = new Rectangle((x * GEngine.TileSize), (y * GEngine.TileSize), GEngine.TileSize, GEngine.TileSize);
                    G.DrawImage(teils, dRect, sRect, GraphicsUnit.Pixel);
                }

            }*/

            sRect = new Rectangle(0, 0, GEngine.FormWidht, GEngine.FormHeigt);
            G.DrawImage(MAPMAP, 0, 0, sRect, GraphicsUnit.Pixel);

            //Player Drawing
            sRect = new Rectangle(0, 0, GEngine.TileSize, GEngine.TileSize);
            player.MakeTransparent(Color.CadetBlue);
            G.DrawImage(player, xpos, ypos, sRect, GraphicsUnit.Pixel);

            //Lamps Drawing
            sRect = new Rectangle(0, 0, GEngine.FormWidht, GEngine.FormHeigt);
            G.DrawImage(Lamps, 0, 0, sRect, GraphicsUnit.Pixel);

            sRect = new Rectangle(0, 0, GEngine.FormWidht, GEngine.FormHeigt);
            G.DrawImage(Light, 0, 0, sRect, GraphicsUnit.Pixel);
            /* (var item in LightUnitCoordinates)
             { 

                 int xx = Convert.ToInt32(item.x);
                 int yy = Convert.ToInt32(item.y);
                 sRect = new Rectangle(0, 0, GEngine.TileSize, GEngine.TileSize);
                 G.DrawImage(player, xx, yy, sRect, GraphicsUnit.Pixel);
                 //Console.WriteLine(item.x);
             }*/

            //Info Drawing
            G.DrawString("FPS:" + fps + "\r\n" + "Map X:" + MouseCoords.x + "\r\n" +
                "Map Y" + MouseCoords.y, window.Font, Brushes.Black, 650, 0);
            //Draw it to the window
            G = Graphics.FromImage(BB);

            try
            {
                BBG = window.CreateGraphics();
                BBG.DrawImage(BB, 0, 0, GEngine.FormWidht, GEngine.FormHeigt);

                G.Clear(Color.Green);
                GCircle.Clear(Color.Transparent);

            }
            catch (Exception)
            {    
            }
        }

    }   
}
