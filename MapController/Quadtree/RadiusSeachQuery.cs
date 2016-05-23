using System;
using LightControl;
using System.Collections.Generic;
using Triangulering;
using System.Diagnostics;
using System.Drawing;
using Quadtree;

namespace TreeStructure
{
    public class RadiusSearchQuery : Query
    {
        int _radius;
        int _width, _height;

        public override Rectangle Bound
        {
            get { return _radiusBound; }
            set { _radiusBound = value; }
        }
        private Rectangle _radiusBound;

        public RadiusSearchQuery(int Radius, IBoundable Mapbound, ISearchable tree)
        {
            _radius = Radius;
            _width = _height = _radius;
            MapBound = Mapbound;
            Tree = tree;
        }
        // Makes the radius bound 
        public override Rectangle GetBound(Coords entityPosition, int width, int height)
        {
            Point TopLeft = new Point((int)entityPosition.x - width / 2, (int)entityPosition.y - height / 2);
            Size boundSize = new Size(width, height);
            Rectangle Bound = new Rectangle(TopLeft, boundSize);
            return Bound;
        }
        
        public override void SearchTree(Coords entityPosition, ref List<LightingUnit> list)
        {
            _radiusBound = GetBound(entityPosition, _width, _height);
            //_radiusBound.InitializeBoundable(this);
            Tree.GetLightUnitInBound(ref list, _radiusBound);
        }
    }
}
