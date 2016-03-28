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

        Point SimulationPosition = new Point((Form1.width/2)-(GEngine.SimulationWidht/2), (Form1.height/2)-(GEngine.SimulationWidht/2));

        public GraphicsDraw(Form form, Bitmap map)
        {
            Map = map;
            window = form;

            pb.Width = GEngine.SimulationWidht;
            pb.Height = GEngine.SimulationWidht;
            pb.Location = SimulationPosition;
            pb.Visible = true;
            pb.Show();
            window.Controls.Add(pb);
            
        }
        public void InitBitMaps()
        {
            BB = new Bitmap(GEngine.SimulationWidht, GEngine.SimulationHeigt);
            MAPMAP = new Bitmap(GEngine.SimulationWidht, GEngine.SimulationHeigt);
            Lamps = new Bitmap(GEngine.SimulationWidht, GEngine.SimulationHeigt);
            Light = new Bitmap(GEngine.SimulationWidht, GEngine.SimulationHeigt);
            player = new Bitmap("Player3.png");
            teils = new Bitmap("Teils.png");
            lamp = new Bitmap("Lamp.png");
        }
        public void DrawMap()
        {
            
            G = Graphics.FromImage(MAPMAP);
            for (int x = 0; x < Map.Width; x++)
            {
                for (int y = 0; y < Map.Height; y++)
                {
                    //Determine the color code of the pixel on the map
                    // And draw to the window
                    Color PixelCode = Map.GetPixel(x, y);
                    string pixelColorStringValue =
                        PixelCode.R.ToString("D3") +
                        PixelCode.G.ToString("D3") +
                        PixelCode.B.ToString("D3");
                    GetSurce(pixelColorStringValue);
                    dRect = new Rectangle((x * GEngine.TileSize), (y * GEngine.TileSize), GEngine.TileSize, GEngine.TileSize);
                    G.DrawImage(teils, dRect, sRect, GraphicsUnit.Pixel);
                }
            }
            G.Dispose();
            teils.Dispose();
        }
        public void DrawLamps(List<LightingUnit> LightUnitCoordinates)
        {
            G = Graphics.FromImage(Lamps);
            foreach (var item in LightUnitCoordinates)         
            {
                int xx = Convert.ToInt32(item.x);
                int yy = Convert.ToInt32(item.y);
                sRect = new Rectangle(0, 0, 5, 5);
                lamp.MakeTransparent(Color.CadetBlue);
                G.DrawImage(lamp, xx-2, yy-2, sRect, GraphicsUnit.Pixel);
            }
            G.Dispose();
            lamp.Dispose();
        }

        public void DrawLight(List<LightingUnit> ActivatedLightingUnitsOnUser)
        {
            double R = _radius * _radius;
            //Lock Bitmap to get BitmapData
            int Width = Light.Width;
             PixelFormat pxf = PixelFormat.Format32bppArgb;
             Rectangle reccct = new Rectangle(0, 0, Light.Width, Light.Height);
             BitmapData bmpData = Light.LockBits(reccct, ImageLockMode.WriteOnly, pxf);
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
                         double Cirklensligning = ((x - item.x) * (x - item.x)) + ((y - item.y) * (y - item.y));
                         if (Cirklensligning <= R)
                         {
                                PlaceInArray = (int)(((y * Width * 4) + x * 4) + 3);
                                double Alpha = volume + (Math.Sqrt(Cirklensligning) * 2); // 3 eller 4
                                if (Alpha > 200)
                                {
                                    Alpha = 200;
                                }
                                else
                                {
                                    if (rgbValues[PlaceInArray] > (Byte)(Alpha))
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
            G = Graphics.FromImage(BB);            
            //Map
            G.DrawImage(MAPMAP, 0, 0);
            // Occupant
            player.MakeTransparent(Color.CadetBlue);
            G.DrawImage(player, point.X-8, point.Y-8);
            //Lamps Drawing
            G.DrawImage(Lamps, 0, 0);
            //Light Drawing
            G.DrawImage(Light, 0, 0);
            //Info Drawing
            G.DrawString("FPS:" + fps, window.Font, Brushes.Red, 590, 0);
            //Draw it to the window
            pb.Image = BB;
        }
    }   
}
