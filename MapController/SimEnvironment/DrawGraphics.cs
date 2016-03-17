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

        PictureBox pb = new PictureBox();
        Collision collision;

        Point start = new Point((Form1.width/2)-(GEngine.SimulationWidht/2), (Form1.height/2)-(GEngine.SimulationWidht/2));
        // DET HER SKAL FLYTTES TIL LOOP
        List<LightingUnit> ActivatedLightingUnitsOnUser = new List<LightingUnit>();
        List<LightingUnit> ActivatedLightingUnitsInPath = new List<LightingUnit>();
        
        List<LightingUnit> LightUnitCoordinates;
        Coords PreviousPosition = new Coords(99999,99999);
        
        Coords PositionCoords = new Coords();
        
        Coords MouseCoords = new Coords();
        // HER TIL
        public GraphicsDraw(Form form, Bitmap map)
        {
            //DET HER SKAL FLYTTES TIL LOOP
            LightUnitsCoords lol2 = new LightUnitsCoords(GEngine.SimulationHeigt, GEngine.SimulationWidht, 30); // 
            LightUnitCoordinates = new List<LightingUnit>();
            lol2.GetLightUnitCoords(ref LightUnitCoordinates);
            //HER TIL
            Map = map;
            window = form;

            pb.Width = GEngine.SimulationWidht;
            pb.Height = GEngine.SimulationWidht;
            pb.Location = start;
            pb.Visible = true;
            pb.Show();
            window.Controls.Add(pb);
        }

        public void Begin()
        {
            collision = new Collision(Map);
            BB = new Bitmap(GEngine.SimulationWidht, GEngine.SimulationHeigt);
            MAPMAP = new Bitmap(GEngine.SimulationWidht, GEngine.SimulationHeigt);
            Lamps = new Bitmap(GEngine.SimulationWidht, GEngine.SimulationHeigt);
            Light = new Bitmap(GEngine.SimulationWidht, GEngine.SimulationHeigt);
            player = new Bitmap("Player3.png");
            teils = new Bitmap("Teils.png");
            lamp = new Bitmap("Lamp.png");
        }
        public void Position()
        {//HELE DENNE METODE SKAL FLYTTES VÆK
            double xx = PositionCoords.x / GEngine.TileSize;
            double YY = PositionCoords.y / GEngine.TileSize;
            MouseCoords.x = Convert.ToInt32((Math.Floor(xx)));
            MouseCoords.y = Convert.ToInt32((Math.Floor(YY)));

            
            ActivatedLightingUnitsOnUser = DetermineLightsToActivate.LightsToActivateOnUser(PositionCoords, LightUnitCoordinates);
            ActivatedLightingUnitsInPath = DetermineLightsToActivate.LightsToActivateInPath(PreviousPosition, PositionCoords, LightUnitCoordinates);

            PreviousPosition.x = PositionCoords.x;
            PreviousPosition.y = PositionCoords.y;
            //HER TIL
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

                                PlaceInArray = Convert.ToInt32(((y * Width * 4) + x * 4) + 3);
                                double Alpha = volume + (Math.Sqrt(Cirklensligning) * 2); // 3 eller 4
                                if (Alpha > 200)
                                {
                                    Alpha = 200;
                                }
                            if (rgbValues[PlaceInArray] > (Byte)(Alpha))
                            {
                                //if (collision.CheckLightCollision(Convert.ToInt32(x), Convert.ToInt32(y)))
                                {
                                    rgbValues[PlaceInArray] = (Byte)(Alpha);
                                }
                            }
                        }
                     }
                 }
             }
             Marshal.Copy(rgbValues, 0, ptr, numBytes);
             Light.UnlockBits(bmpData);
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

        public void Draw(int fps, Point point)
        {
            //Player possion
            PositionCoords.x = point.X;
            PositionCoords.y = point.Y;

            G = Graphics.FromImage(BB);            
            //Map
            G.DrawImage(MAPMAP, 0, 0);
            // Employer
            player.MakeTransparent(Color.CadetBlue);
            G.DrawImage(player, point.X-8, point.Y-8);
            //Lamps Drawing
            G.DrawImage(Lamps, 0, 0);
            //Light Drawing
            G.DrawImage(Light, 0, 0);
            //Info Drawing
            G.DrawString("FPS:" + fps + "\r\n" + "Map X:" + MouseCoords.x + "\r\n" +
               "Map Y" + MouseCoords.y, window.Font, Brushes.Red, 590, 0);
            //Draw it to the window
            pb.Image = BB;
        }
    }   
}
