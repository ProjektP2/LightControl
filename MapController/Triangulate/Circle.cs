using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightControl;

//The circle that is defined here has the same center coordinates as the access points' 
//coordinates in a given zone. The circles radius is given by the signal strength. 
//Alternatively, this class can be called Router. However, for the sake of mathematical understanding, 
//the name is "Circle" for now.

namespace Triangulering
{
    class Circle : Coords
    {
        public double Radius;

        public Circle(int p1, int p2)
        {
            x = p1;
            y = p2;
        }

        public Circle()
        {
            x = 0;
            y = 0;
        }

        public void SetRouterPosition(int newx, int newy)
        {
            x = newx;
            y = newy;
        }

        public void SetSignalStrength(double SignalStrength)
        {
            Radius = SignalStrength;
        }
    }
}
