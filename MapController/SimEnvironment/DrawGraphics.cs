﻿using System;
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
            LightUnitsCoords lol2 = new LightUnitsCoords(GEngine.FormHeigt, GEngine.FormWidht, 22);
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
            
            int radius = 30; // helst ikke her!!!
            double procent = 1.0; // her må der ændres [0 til 1]

            //Lock Bitmap to get BitmapData

             PixelFormat pxf = PixelFormat.Format32bppArgb;
             Rectangle reccct = new Rectangle(0, 0, Light.Width, Light.Height);
             BitmapData bmpData = Light.LockBits(reccct, ImageLockMode.ReadWrite, pxf);
             IntPtr ptr = bmpData.Scan0;

             int numBytes = bmpData.Stride * Light.Height;
             byte[] rgbValues = new byte[numBytes];

             Marshal.Copy(ptr,rgbValues, 0, numBytes);
             for (int i = 3; i < rgbValues.Length; i+=4)
             {
                 rgbValues[i] = 255;
             }

             #region noget
             foreach (var item in LightUnitCoordinates)
             {
                 double volume = 255 - (255 * (procent));
                 int PlaceInArray;
                 for (double y = item.y - radius; y < item.y + radius; y++)
                 {
                     for (double x = item.x - radius; x < item.x + radius; x++)
                     {
                         double R = radius * radius;
                         double Cirklensligning = ((x - item.x) * (x - item.x)) + ((y - item.y) * (y - item.y));
                         if (Cirklensligning <= R)
                         {
                             PlaceInArray = Convert.ToInt32(((y * Light.Width * 4) + x * 4) + 3);
                            double tal = volume + (Math.Sqrt(Cirklensligning)*4); // 3 eller 4
                            if (tal > 255)
                            {
                                tal = 255;
                            }
                            if (rgbValues[PlaceInArray] > (Byte)(tal))
                            {
                                rgbValues[PlaceInArray] = (Byte)(tal);
                            }
                        }
                     }
                 }
             }
             #endregion
             Marshal.Copy(rgbValues, 0, ptr, numBytes);
             Light.UnlockBits(bmpData);
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
            sRect = new Rectangle(0, 0, GEngine.TileSize/2, GEngine.TileSize/2);
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


                BBG = window.CreateGraphics();
                BBG.DrawImage(BB, 0, 0, GEngine.FormWidht, GEngine.FormHeigt);
                G.Clear(Color.Green);
                BBG.Dispose();
        }

    }   
}
