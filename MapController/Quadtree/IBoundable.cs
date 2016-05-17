using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Quadtree
{
    public interface IBoundable
    {
        Rectangle Bound { get; set; }
    }
}
