using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightControl;

namespace LightControl
{
    class LightUnitsCoords
    {
        // 32px = 1meter
        public int Height { get; private set; }
        public int Width { get; private set; }
        public double PixelDensity { get; set; }

        List<Coords> lightUnitCoords;

        public LightUnitsCoords(int height, int width, int pixelDensity,
            List<Coords> coordList) 
        {
            Height = height;
            Width = width;
            PixelDensity = pixelDensity;
            lightUnitCoords = coordList;
        }
        
        public List<Coords> GetLightUnitCoords()
        {
            for (int x = 0; x < Height; x++)
            {
                for (int y = 0; y < Width; y++)
                {
                    if (CheckCoords(x,y))
                    {
                        lightUnitCoords.Add(new Coords(x, y));
                    }
                }
            }
            return lightUnitCoords;
        }

        private bool CheckCoords(int x, int y)
        {

            if (x % (PixelDensity + PixelDensity / 2) == 0 && y % (PixelDensity + PixelDensity / 2) == 0 
                && x != 0 && y != 0)
            {
                return true;
            }
            else
                return false;
        }
        
        public void printCoords()
        {
            foreach (Coords coord in lightUnitCoords)
            {
                Console.WriteLine($"x: {coord.x}, y: {coord.y}");
            }
            Console.ReadKey();
        } 
    }
}
