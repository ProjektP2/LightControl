using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using LightControl;
using Triangulering;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace SimEnvironment
{
    class GraphicsDraw
    {
        int _radius = 35;
        Graphics BBG;
        Rectangle sRect;
        Rectangle dRect;
        Graphics G;
        Graphics GMap;
        Graphics Glamps;

        Bitmap Map;
        Bitmap player;
        Bitmap teils;
        Bitmap lamp;
        
        Bitmap BB;
        Bitmap MAPMAP;
        Bitmap Lamps;
        Bitmap Light;

        Form window;
        List<LightingUnit> ActivatedLightingUnitsOnUser = new List<LightingUnit>();
        List<LightingUnit> ActivatedLightingUnitsInPath = new List<LightingUnit>();
        //List<Coords> LightingUnitCoordinates = new List<Coords>();
        
        List<LightingUnit> LightUnitCoordinates;
        Coords PreviousPosition = new Coords(99999,99999);
        
        Coords PositionCoords = new Coords();
        
        Coords MouseCoords = new Coords();

        public GraphicsDraw(Form form, Bitmap map)
        {
            //LightingUnitCoordinates.Add(new Coords(30, 50));
            LightUnitsCoords lol2 = new LightUnitsCoords(GEngine.FormHeigt, GEngine.FormWidht, 30); // 
            LightUnitCoordinates = new List<LightingUnit>();
            lol2.GetLightUnitCoords(ref LightUnitCoordinates);
            Map = map;
            window = form;
        }
        public void Begin()
        {
            G = window.CreateGraphics();
            BB = new Bitmap(GEngine.FormWidht, GEngine.FormHeigt);
            MAPMAP = new Bitmap(GEngine.FormWidht, GEngine.FormHeigt);
            Lamps = new Bitmap(GEngine.FormWidht, GEngine.FormHeigt);
            Light = new Bitmap(Map.Width*GEngine.TileSize, GEngine.FormHeigt);
            player = new Bitmap("Player3.png");
            teils = new Bitmap("Teils.png");
            lamp = new Bitmap("Lamp.png");
        }
        public void Position()
        {
            double xx = PositionCoords.x / GEngine.TileSize;
            double YY = PositionCoords.y / GEngine.TileSize;
            MouseCoords.x = Convert.ToInt32((Math.Floor(xx)));
            MouseCoords.y = Convert.ToInt32((Math.Floor(YY)));

            
            ActivatedLightingUnitsOnUser = DetermineLightsToActivate.LightsToActivateOnUser(PositionCoords, LightUnitCoordinates);
            ActivatedLightingUnitsInPath = DetermineLightsToActivate.LightsToActivateInPath(PreviousPosition, PositionCoords, LightUnitCoordinates);
            /*foreach (var item in ActivatedLightingUnitsOnUser)
            {
                Console.WriteLine(item.LightingLevel);
            }
            */
            PreviousPosition.x = PositionCoords.x;
            PreviousPosition.y = PositionCoords.y;

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
            GMap.Dispose();
            teils.Dispose();
        }
        public void DrawLamps()
        {
            Glamps = Graphics.FromImage(Lamps);
            foreach (var item in LightUnitCoordinates)
            {
                int xx = Convert.ToInt32(item.x);
                int yy = Convert.ToInt32(item.y);
                sRect = new Rectangle(0, 0, 5, 5);
                lamp.MakeTransparent(Color.CadetBlue);
                Glamps.DrawImage(lamp, xx-2, yy-2, sRect, GraphicsUnit.Pixel);
            }
            Glamps.Dispose();
            lamp.Dispose();
        }

        public void DrawLight()
        {

            //_radius = 30; // helst ikke her!!!
            //double procent = 1.0; // her må der ændres [0 til 1]

            //Lock Bitmap to get BitmapData
            int Width = Light.Width;
             PixelFormat pxf = PixelFormat.Format32bppArgb;
             Rectangle reccct = new Rectangle(0, 0, Light.Width, Light.Height);
             BitmapData bmpData = Light.LockBits(reccct, ImageLockMode.ReadWrite, pxf);
             IntPtr ptr = bmpData.Scan0;

             int numBytes = bmpData.Stride * Light.Height;
             byte[] rgbValues = new byte[numBytes];

             Marshal.Copy(ptr,rgbValues, 0, numBytes);
             for (int i = 3; i < rgbValues.Length; i+=4)
             {
                 rgbValues[i] = 200;
             }
            #region noget
            int tal = 0;
             foreach (var item in ActivatedLightingUnitsOnUser)
             {
                 double volume = 255 - (255 * (item.LightingLevel));
                 int PlaceInArray;
                 for (double y = item.y - _radius; y < item.y + _radius; y++)
                 {
                     for (double x = item.x - _radius; x < item.x + _radius; x++)
                     {
                        
                        double R = _radius * _radius;
                         double Cirklensligning = ((x - item.x) * (x - item.x)) + ((y - item.y) * (y - item.y));
                         if (Cirklensligning <= R)
                         {
                            //tal++;
                            //tal = 10*Width;
                            PlaceInArray = Convert.ToInt32(((y * Width*4) + x * 4)+3);
                            double RGB_Color = volume + (Math.Sqrt(Cirklensligning)*2); // 3 eller 4
                            if (RGB_Color > 200)
                            {
                                RGB_Color = 200;
                            }
                            if (rgbValues[PlaceInArray] > (Byte)(RGB_Color))
                            {
                                rgbValues[PlaceInArray] = (Byte)(RGB_Color);
                            }
                        }
                     }
                 }
             }
             #endregion
             Marshal.Copy(rgbValues, 0, ptr, numBytes);
             Light.UnlockBits(bmpData);
            //Console.WriteLine(Light.Width);
        }
        private void GetSurce(string pixelColorStringValue)
        {
            switch (pixelColorStringValue)
             {
                //Diffrent Color codes, reads diffrent locations on the tile bitmap
                 case "255020147": sRect = new Rectangle(0, 32, GEngine.TileSize, GEngine.TileSize); break;
                 case "255255000": sRect = new Rectangle(32, 32, GEngine.TileSize, GEngine.TileSize); break;
                 default: new Rectangle(0, 0, GEngine.TileSize, GEngine.TileSize); break;
             }
        }

        public void Draw(int xpos, int ypos, int fps)
        {
            //Player possion
            PositionCoords.x = xpos;
            PositionCoords.y = ypos;

            // Draw the Bitmaps
            //Map
            //sRect = new Rectangle(0, 0, GEngine.FormWidht, GEngine.FormHeigt);
            //G.DrawImage(MAPMAP, 0, 0, sRect, GraphicsUnit.Pixel);
            G.DrawImage(MAPMAP,0,0);

            //Player Drawing
            //sRect = new Rectangle(0, 0, GEngine.TileSize/2, GEngine.TileSize/2);
            player.MakeTransparent(Color.CadetBlue);
            //G.DrawImage(player, xpos, ypos, sRect, GraphicsUnit.Pixel);
            G.DrawImage(player, xpos, ypos);
            
            //Lamps Drawing
            //sRect = new Rectangle(0, 0, GEngine.FormWidht, GEngine.FormHeigt);
            //G.DrawImage(Lamps, 0, 0, sRect, GraphicsUnit.Pixel);
            G.DrawImage(Lamps, 0, 0);
           
            //Light Drawing
            //sRect = new Rectangle(0, 0, GEngine.FormWidht, GEngine.FormHeigt);
            //G.DrawImage(Light, 0, 0, sRect, GraphicsUnit.Pixel);
            G.DrawImage(Light, 0, 0);
            //Info Drawing
            G.DrawString("FPS:" + fps + "\r\n" + "Map X:" + MouseCoords.x + "\r\n" +
               "Map Y" + MouseCoords.y, window.Font, Brushes.Red, 590, 0);
            //Draw it to the window
            G = Graphics.FromImage(BB);

            try
            {
                BBG = window.CreateGraphics();
                BBG.DrawImage(BB, 0, 0, GEngine.FormWidht, GEngine.FormHeigt);
                G.Clear(Color.Green);
                
                BBG.Dispose();
            }
            catch (Exception)
            {
            }
        }
    }   
}
