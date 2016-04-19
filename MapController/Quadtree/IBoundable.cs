﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightControl;

namespace MapController.Quadtree
{
    public interface IBoundable
    {
        void CalculateBoundCoords(Coords Position, out Coords BottomRight, out Coords Topleft);
    }
}