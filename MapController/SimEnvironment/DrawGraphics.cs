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

        Graphics BBG;
        Rectangle sRect;
        Rectangle dRect;
        Graphics G;
        Graphics GMap;
        Graphics Glamps;
        Graphics GCircle;
        Graphics GBlack;

        Bitmap Map;
        Bitmap player;
        Bitmap teils;
        Bitmap lamp;

        Bitmap BB;
        Bitmap MAPMAP;
        Bitmap Lamps;
        Bitmap Light;
        Bitmap Black;

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
            LightingUnitCoordinates.Add(new Coords(30, 50));
            LightingUnitCoordinates.Add(new Coords(60, 50));
            LightingUnitCoordinates.Add(new Coords(90, 50));
            LightingUnitCoordinates.Add(new Coords(200, 50));
            LightingUnitCoordinates.Add(new Coords(250, 50));
            LightUnitsCoords lol2 = new LightUnitsCoords(GEngine.FormHeigt, GEngine.FormWidht, GEngine.TileSize);
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
            GBlack = window.CreateGraphics();
            BB = new Bitmap(GEngine.FormWidht, GEngine.FormHeigt);
            MAPMAP = new Bitmap(GEngine.FormWidht, GEngine.FormHeigt);
            Lamps = new Bitmap(GEngine.FormWidht, GEngine.FormHeigt);
            Light = new Bitmap(GEngine.FormWidht-200, GEngine.FormHeigt);
            Black = new Bitmap(GEngine.FormWidht, GEngine.FormHeigt);
            player = new Bitmap("Player.png");
            teils = new Bitmap("Teils.png");
            lamp = new Bitmap("Lamp.png");

        }
        public void Position()
        {
            double xx = PositionCoords.x / GEngine.TileSize;
            double YY = PositionCoords.y / GEngine.TileSize;
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
            GMap.Dispose();
            teils.Dispose();
        }
        public void DrawLamps()
        {
            Glamps = Graphics.FromImage(Lamps);
            foreach (var item in LightingUnitCoordinates)
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

        public void BlackMap()
        {
            /*GBlack = Graphics.FromImage(Black);
            sRect = new Rectangle(0, 0, GEngine.FormWidht - 100, GEngine.FormHeigt);
            SolidBrush myBrush = new SolidBrush(Color.FromArgb(255, 000, 000, 000));
            GBlack.FillRectangle(myBrush, sRect);*/
        }

        public void DrawLight()
        {
            
            GCircle = Graphics.FromImage(Light);
            /*sRect = new Rectangle(0, 0, GEngine.FormWidht - 100, GEngine.FormHeigt);
            SolidBrush myBrush = new SolidBrush(Color.FromArgb(255, 000, 000, 000));
            GCircle.FillRectangle(myBrush, sRect);*/

            /*GCircle = Graphics.FromImage(Light);
            sRect = new Rectangle(0, 0, Black.Width, Black.Height);
            GCircle.DrawImage(Black, 0, 0, sRect, GraphicsUnit.Pixel);
            */
            int radius = 30;
            double procent = 0.9;

            PixelFormat pxf = PixelFormat.Format32bppArgb;
            Rectangle reccct = new Rectangle(0, 0, Light.Width, Light.Height);
            BitmapData bmpData = Light.LockBits(reccct, ImageLockMode.ReadWrite, pxf);
            IntPtr ptr = bmpData.Scan0;

            int numBytes = bmpData.Stride * Light.Height;
            byte[] rgbValues = new byte[numBytes];

            Marshal.Copy(ptr,rgbValues, 0, numBytes);
            int tal = 0;
            for (int i = 0; i < rgbValues.Length; i+=4)
            {
                rgbValues[i] = 0;
                rgbValues[i+1] = 0;
                rgbValues[i + 2] = 0;
                rgbValues[i + 3] = 255;
                tal++;
            }
            foreach (var item in LightingUnitCoordinates)
            {
                double volume = 255 - (255 * (procent));

                int X0 = Convert.ToInt32(item.x);
                int Y0 = Convert.ToInt32(item.y);
                int ee = 0;
                for (int y = Y0 - radius; y < Y0 + radius; y++)
                {
                    for (int x = X0 - radius; x < X0 + radius; x++)
                    {
                        int r = radius * radius;
                        int Cirklensligning = ((x - X0) * (x - X0)) + ((y - Y0) * (y - Y0));
                        if (Cirklensligning <= r)
                        {
                            int jaa = 0 + (Convert.ToInt32(volume + (Math.Sqrt(Cirklensligning))));
                                ee = (y * Light.Width*4) + x*4;
                            //rgbValues[ee] = 0;
                            //rgbValues[ee + 1] = 0;
                            //rgbValues[ee+2] = 0;
                            rgbValues[ee+3] = 155;
                        }
                    }
                }
            }
            Marshal.Copy(rgbValues, 0, ptr, numBytes);

            Light.UnlockBits(bmpData);

            // double ggg = (tal / Light.Width);
            //int y = Convert.ToInt32(Math.Floor(ggg));
            //int x = tal % Light.Width;
            //int r = radius * radius;
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
            
            sRect = new Rectangle(0, 0, GEngine.FormWidht, GEngine.FormHeigt);
            G.DrawImage(MAPMAP, 0, 0, sRect, GraphicsUnit.Pixel);

            //Player Drawing
            sRect = new Rectangle(0, 0, GEngine.TileSize, GEngine.TileSize);
            player.MakeTransparent(Color.CadetBlue);
            G.DrawImage(player, xpos, ypos, sRect, GraphicsUnit.Pixel);

            //Lamps Drawing
            sRect = new Rectangle(0, 0, GEngine.FormWidht, GEngine.FormHeigt);
            G.DrawImage(Lamps, 0, 0, sRect, GraphicsUnit.Pixel);

            //Light Drawing
            sRect = new Rectangle(0, 0, GEngine.FormWidht, GEngine.FormHeigt);
            G.DrawImage(Light, 0, 0, sRect, GraphicsUnit.Pixel);

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

            }
            catch (Exception)
            {    
            }
        }

    }   
}
