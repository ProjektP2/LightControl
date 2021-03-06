﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
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
    public class GraphicsDraw
    {
        int _radius = 88;

        private struct DrawLightData
        {
            public int numBytes;
            public PixelFormat pixelFormat;
            public Rectangle rectangle;
            public BitmapData bmpData;
            public IntPtr ptr;
        }
        private DrawLightData _drawLightData;

        private struct RectCorners
        {
            public double TopLeftX, TopLeftY,
                BottomRightX, BottomRightY;
        }
        private RectCorners _rectCorners;

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
        Stopwatch sw = new Stopwatch();

        private PictureBox SimulationRoom;

        public GraphicsDraw(Form form, Bitmap map, PictureBox simulationRoom)
        {
            Map = map;
            window = form;
            SimulationRoom = simulationRoom;
        }
        public void InitBitMaps()
        {
            LoadMapUsingPath loadMap = new LoadMapUsingPath();
            BB = new Bitmap(GEngine.SimulationWidht, GEngine.SimulationHeigt);
            MAPMAP = new Bitmap(GEngine.SimulationWidht, GEngine.SimulationHeigt);
            Lamps = new Bitmap(GEngine.SimulationWidht, GEngine.SimulationHeigt);
            Light = new Bitmap(GEngine.SimulationWidht, GEngine.SimulationHeigt);
            try
            {
                player = loadMap.LoadFileIntoBitMap("Player3.png");
                teils = loadMap.LoadFileIntoBitMap("Teils.png");
                lamp = loadMap.LoadFileIntoBitMap("Lamp.png");
            }
            catch (ArgumentException exception)
            {
                Application.Exit();
            }
            catch (Exception exception)
            {
                Application.Exit();
            }
        }


        public void LoadMapIntoBitMap(Circle Router1, Circle Router2)
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
            LoadRouters(Router1);
            LoadRouters(Router2);
            G.Dispose();
            teils.Dispose();
        }
        private void LoadRouters(Circle Router)
        {
            sRect = new Rectangle(0, 64, GEngine.TileSize, GEngine.TileSize);
            dRect = new Rectangle((int)Math.Floor(Router.x), ((int)Math.Floor(Router.y)), GEngine.TileSize, GEngine.TileSize);
            G.DrawImage(teils, dRect, sRect, GraphicsUnit.Pixel);
        }

        public void LoadLampsIntoBitMap(List<LightingUnit> LightUnitCoordinates)
        {
            G = Graphics.FromImage(Lamps);
            foreach (var item in LightUnitCoordinates)
            {
                int xx = Convert.ToInt32(item.x);
                int yy = Convert.ToInt32(item.y);
                sRect = new Rectangle(0, 0, 5, 5);
                lamp.MakeTransparent(Color.CadetBlue);
                G.DrawImage(lamp, xx - 2, yy - 2, sRect, GraphicsUnit.Pixel);
            }
            G.Dispose();
            lamp.Dispose();
        }
       
        public void LoadLightIntoBitMap(List<LightingUnit> ActivatedLightingUnitsOnUser)
        {
            byte minTrasnparency = 200;
            _drawLightData = InitDrawLightData();
            byte[] rgbValues = new byte[_drawLightData.numBytes];
            
            Marshal.Copy(_drawLightData.ptr, rgbValues, 0, _drawLightData.numBytes);
            InitRGBValues(_drawLightData.numBytes, minTrasnparency, ref rgbValues);

            SetTransparency(ActivatedLightingUnitsOnUser, rgbValues, minTrasnparency);
            
            Marshal.Copy(rgbValues, 0, _drawLightData.ptr, _drawLightData.numBytes);
            Light.UnlockBits(_drawLightData.bmpData);
        }

        private void SetTransparency(List<LightingUnit> ActivatedLightingUnitsOnUser, 
            byte[] rgbValues, byte minTrasnparency)
        {
            foreach (var item in ActivatedLightingUnitsOnUser)
            {
                if (item.LightingLevel > 0)
                {
                    double volume = minTrasnparency - (minTrasnparency * (item.LightingLevel));
                    SetAlphaPixel(item, rgbValues, volume, minTrasnparency);
                }
            }
        }
        
        private void SetAlphaPixel(LightingUnit item, byte[] rgbValues, double volume, byte minTrasnparency)
        {

            try
            {
                int PlaceInArray;
                int Width = Light.Width;
                double R = _radius * _radius;
                double Cirklensligning, Alpha;
                InitRectCorners(item);
                double leftY = _rectCorners.TopLeftY;
                double rightY = _rectCorners.BottomRightY;
                double leftX = _rectCorners.TopLeftX;
                double rightX = _rectCorners.BottomRightX;
                for (double y = leftY; y < rightY; y++)
                {
                    double lasts = (y - item.y) * (y - item.y);
                    for (double x = leftX; x < rightX; x++)
                    {
                        Cirklensligning = ((x - item.x) * (x - item.x)) + (lasts);
                        if (Cirklensligning <= R && y < GEngine.SimulationHeigt && y > 0 && x < GEngine.SimulationWidht && x > 0)
                        {
                            PlaceInArray = (int)(((y * Width * 4) + x * 4) + 3);
                            Alpha = volume;
                            if (Alpha > minTrasnparency)
                            {
                                Alpha = minTrasnparency;
                            }
                            else if(Alpha < 0)
                            {
                                Alpha = 0;
                            }
                            if (rgbValues[PlaceInArray] > (byte) (Alpha))
                            {
                                rgbValues[PlaceInArray] = (byte)(Alpha);
                            }
                        }
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
            }
        }

        private void InitRGBValues(int size, byte value, ref byte[] rgbValues)
        {
            int i;
            int iterations = size;
            int iterationsfirst = iterations - 16;
            for (i = 3; i < iterationsfirst; i += 16)
            {
                rgbValues[i] = value;
                rgbValues[i + 4] = value;
                rgbValues[i + 8] = value;
                rgbValues[i + 12] = value;
            }
            for (; i < iterations; i+= 4)
            {
                rgbValues[i] = value;
            }
        }

        private DrawLightData InitDrawLightData()
        {
            DrawLightData drawLightData = new DrawLightData();
            drawLightData.pixelFormat = PixelFormat.Format32bppArgb;
            drawLightData.rectangle = new Rectangle(0, 0, Light.Width, Light.Height);
            drawLightData.bmpData = Light.LockBits(drawLightData.rectangle,
                ImageLockMode.WriteOnly, drawLightData.pixelFormat);
            drawLightData.ptr = drawLightData.bmpData.Scan0;
            drawLightData.numBytes = drawLightData.bmpData.Stride * Light.Height;
            return drawLightData;
        }

        private void InitRectCorners(LightingUnit item)
        {
            _rectCorners.TopLeftX = item.x - _radius;
            _rectCorners.TopLeftY = item.y - _radius;
            _rectCorners.BottomRightX = item.x + _radius;
            _rectCorners.BottomRightY = item.y + _radius;
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

        public void Draw(int fps, Coords point, Circle router1, Circle router2)
        {
            G = Graphics.FromImage(BB);
            //Map
            G.DrawImage(MAPMAP, 0, 0);
            // Occupant
            player.MakeTransparent(Color.CadetBlue);
            G.DrawImage(player, (int)point.x - 8, (int)point.y - 8);
            //Lamps Drawing
            G.DrawImage(Lamps, 0, 0);
            //Light Drawing
            G.DrawImage(Light, 0, 0);
            //Info Drawing
            G.DrawString("FPS:" + fps, window.Font, Brushes.Red, 590, 0);
            //Draw it to the window
            SimulationRoom.Image = BB;
            DrawSignalRadius(G, router1, router2, point);
        }

        public void DrawSignalRadius(Graphics g, Circle router1, Circle router2, Coords point)
        {
            int x1 = (int)router1.x - (int)router1.Radius;
            int y1 = (int)router1.y - (int)router1.Radius;
            int width1 = 2 * (int)router1.Radius;
            int height1 = 2 * (int)router1.Radius;

            int x2 = (int)router2.x - (int)router2.Radius;
            int y2 = (int)router2.y - (int)router2.Radius;
            int width2 = 2 * (int)router2.Radius;
            int height2 = 2 * (int)router2.Radius;

            Pen green = new Pen(Color.Green);
            g.DrawEllipse(green, x1, y1, width1, height1);
            g.DrawLine(green, (int)router1.x, (int)router1.y, (int)point.x, (int)point.y);
            g.DrawEllipse(green, x2, y2, width2, height2);
            g.DrawLine(green, (int)router2.x, (int)router2.y, (int)point.x, (int)point.y);

        }
    }
}
