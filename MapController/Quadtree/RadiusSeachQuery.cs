using System;
using LightControl;
using System.Collections.Generic;
using Triangulering;
using System.Diagnostics;
using System.Drawing;

namespace TreeStructure
{
    public class RadiusSearchQuery : Query
    {
        int _radius;
        int _width, _height;
        private Rectangle _mapBound;
        private QuadTree _tree;

        public override Rectangle Bound
        {
            get { return _radiusBound; }
            set { _radiusBound = value; }
        }
        private Rectangle _radiusBound;

        public RadiusSearchQuery(int Radius, Rectangle Mapbound, QuadTree tree)
        {
            _radius = Radius;
            _width = _height = _radius;
            _mapBound = Mapbound;
            _tree = tree;
        }
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
            _tree.GetLightUnitInBound(ref list, _radiusBound);
        }
    }
}
