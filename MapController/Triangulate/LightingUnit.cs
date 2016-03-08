using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightControl;

namespace Triangulering
{
    class LightingUnit : Coords
    {
        double stepInterval = 0.05;
        double maxLevel = 1.0;
        double minLevel = 0.0;
        public double LightingLevel = 0.0;
        int[] group = new int[4];
        double[] scene = new double[16];


        public LightingUnit() :this(0, 0, null, null)
        {
        }

        public LightingUnit(double X, double Y) : this(X, Y, null, null)
        {
            x = X;
            y = Y;
        }

        public LightingUnit(double X, double Y, int[] group, float[] scene)
        {
            x = X;
            y = Y;
        }


        public double goToMax()
        {
            LightingLevel = maxLevel;
            return LightingLevel;
        }

        public double goToMin()
        {
            LightingLevel = minLevel;
            return LightingLevel;
        }

        public double stepUp()
        {
            LightingLevel = LightingLevel + stepInterval;
            return LightingLevel;
        }

        public double stepDown()
        {
            LightingLevel = LightingLevel - stepInterval;
            return LightingLevel;
        }

    }
}
