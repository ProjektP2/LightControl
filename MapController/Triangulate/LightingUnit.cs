using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Triangulering
{
    class LightingUnit : Coords
    {
        public double LightingLevel;

        public void SetLightLevel(double InputLightingLevel)
        {
            LightingLevel = InputLightingLevel;
        }
    }
}
