using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightControl;
using Triangulering;

namespace LightControl
{
    class LightUnitsCoords
    {
        // 32px = 1meter
        public int Height { get; private set; }
        public int Width { get; private set; }
        public double PixelDensity { get; set; }

        List<LightingUnit> lightUnitCoords;

        public LightUnitsCoords(int height, int width, int pixelDensity,
            List<LightingUnit> coordList) 
        {
            Height = height;
            Width = width;
            PixelDensity = pixelDensity;
            lightUnitCoords = coordList;
        }
        
        public List<LightingUnit> GetLightUnitCoords()
        {
            for (int x = 0; x < Height; x++)
            {
                for (int y = 0; y < Width; y++)
                {
                    if (CheckCoords(x,y))
                    {
                        lightUnitCoords.Add(new LightingUnit(x,y));
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
    }
}
