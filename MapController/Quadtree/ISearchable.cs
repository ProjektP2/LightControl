using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Triangulering;

namespace Quadtree
{
    public interface ISearchable
    {
        void GetLightUnitInBound(ref List<LightingUnit> list, Rectangle circleBound);
    }
}
