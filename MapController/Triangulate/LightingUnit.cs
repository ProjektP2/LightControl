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
        float stepInterval = 0.05f;
        float maxLevel = 1.0f;
        float minLevel = 0.0f;
        float LightingLevel = 0.0f;
        int[] group = new int[4];
        float[] scene = new float[16];


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


        public float goToMax()
        {
            LightingLevel = maxLevel;
            return LightingLevel;
        }

        public float goToMin()
        {
            LightingLevel = minLevel;
            return LightingLevel;
        }

        public float stepUp()
        {
            LightingLevel = LightingLevel + stepInterval;
            return LightingLevel;
        }

        public float stepDown()
        {
            LightingLevel = LightingLevel - stepInterval;
            return LightingLevel;
        }

    }
}
