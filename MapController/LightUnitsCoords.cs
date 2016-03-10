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


        public LightUnitsCoords(int height, int width, int pixelDensity) 
        {
            Height = height;
            Width = width;
            PixelDensity = pixelDensity;
        }
        int tal = 0;
        public void GetLightUnitCoords(ref List<LightingUnit> lightUnitCoords)
        {
            for (int y = 50; y < Height-250; y++)
            {
                for (int x = 50; x < Width-250; x++)
                {
                    if (CheckCoords(x,y))
                    {
                        tal++;
                        if (tal > 25)
                            break;
                        else
                            lightUnitCoords.Add(new LightingUnit(x, y));

                    }
                }
            }
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
