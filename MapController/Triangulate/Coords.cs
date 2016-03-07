using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Triangulering
{
    //Simple class containing two doubles. The class is used to represent a set of coordinates.
    //Default coordinates is set to (0,0)

    class Coords
    {
        public double x, y;

        public Coords(double p1, double p2)
        {
            x = p1;
            y = p2;
        }

        public Coords()
        {
            x = 0;
            y = 0;
        }
    }
}
