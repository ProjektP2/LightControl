using System;
using LightControl;
using System.Collections.Generic;
using Triangulering;
using System.Diagnostics;

namespace TreeStructure
{
    public class RadiusSearchQuery : Query
    {
        int _radius;
        int _width, _height;
        private Bounds _circleBound;
        private List<LightingUnit> _lightUnitList;
        private Bounds _mapBound;
        private QuadTree _tree;
        public override Bounds Bound
        {
            get { return _radiusBound; }
            set { _radiusBound = value; }
        }
        private Bounds _radiusBound;

        public RadiusSearchQuery(int Radius, Bounds Mapbound, QuadTree tree)
        {
            _radius = Radius;
            _width = _height = _radius;
            _mapBound = Mapbound;
            _tree = tree;
        }
        public override Bounds GetBound(Coords entityPosition, int width, int height)
        {
            Bounds Bound = new Bounds(entityPosition, width, height);
            return Bound;
        }
        public override void SearchTree(Coords entityPosition, ref List<LightingUnit> list)
        {
            _radiusBound = GetBound(entityPosition, _width, _height);
            _radiusBound.InitializeBoundable(this);
            _tree.GetLightUnitInBound(ref list, _radiusBound);
        }

        public override void CalculateBoundCoords(Coords Position, out Coords TopLeft, out Coords BottomRight)
        {
            double BottomRightX, BottomRightY;
            double TopLeftX, TopLeftY;
            TopLeftX = Position.x - _width;
            TopLeftY = Position.y - _height;
            BottomRightX = Position.x + _width;
            BottomRightY = Position.y + _height;

            TopLeft = new Coords(TopLeftX, TopLeftY);
            BottomRight = new Coords(BottomRightX, BottomRightY);
        }
    }
}
