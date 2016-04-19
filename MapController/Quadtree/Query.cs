using System;
using LightControl;
using System.Collections.Generic;
using MapController.Quadtree;
using Triangulering;

namespace TreeStructure
{
    abstract class Query : IBoundable
    {
        public Bounds MapBound { get; set; }
        public QuadTree Tree { get; set; }

        /*public Query(int width, int height, Bounds map, QuadTree tree)
        {
            MapBound = map;
            Tree = tree;
        }*/
        public abstract Bounds GetBound(Coords entityPosition, int witdh, int height);
        public abstract void SearchTree(Coords entityPosition, ref List<LightingUnit> list);
        public abstract void CalculateBoundCoords(Coords Position, out Coords BottomRight, out Coords Topleft);
    }
    
    class StartTreeSearch
    {
        private List<LightingUnit> _unitList;
        public List<LightingUnit> SearchQuery(Coords position, params Query[] searches)
        {
            _unitList = new List<LightingUnit>();
            foreach (var item in searches)
            {
                item.SearchTree(position, ref _unitList);
            }
            return _unitList;
        }
    }

    class RadiusSearchQuery : Query
    {
        int _radius;
        int _width, _height;
        private Bounds _circleBound;
        private List<LightingUnit> _lightUnitList;
        private Bounds _mapBound;
        private QuadTree _tree;

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
            _circleBound = GetBound(entityPosition, _width, _height);
            _circleBound.InitializeBoundable(this);
            _tree.GetLightUnitInBound(ref list, _circleBound);
        }

        public override void CalculateBoundCoords(Coords Position, out Coords BottomRight, out Coords Topleft)
        {
            double BottomRightX, BottomRightY;
            double TopLeftX, TopLeftY;
            BottomRightX = Position.x + _width;
            BottomRightY = Position.y + _height;
            TopLeftX = Position.x - _width;
            TopLeftY = Position.y - _height;
            BottomRight = new Coords(BottomRightX, BottomRightY);
            Topleft = new Coords(TopLeftX, TopLeftY);
        }
    }

    class VectorSearchQuery : Query
    {
        private int _width;
        private int _height;
        private Bounds _vectorBound;
        private Occupant _occupant;

        public VectorSearchQuery(Bounds mapBound, QuadTree tree, Occupant occupant)
        {
            MapBound = mapBound;
            Tree = tree;
            _occupant = occupant;
            _width = 50;
            _height = 250;
        }
        public override Bounds GetBound(Coords entityPosition, int width, int height)
        {
            Bounds Bound = new Bounds(entityPosition, width, height);
            return Bound;
        }

        public override void SearchTree(Coords entityPosition, ref List<LightingUnit> list)
        {
            _vectorBound = GetBound(entityPosition, _width, _height);
            _vectorBound.InitializeBoundable(this);
            Tree.GetLightUnitInBound(ref list, _vectorBound);
        }

        private Coords GetBaseMoveVector(Coords movementVector)
        {
            Coords baseMovementVector;
            double baseMovementVectorX = movementVector.x;
            double baseMovementVectorY = movementVector.y;
            if (baseMovementVectorX > 0)
                baseMovementVectorX = 1;
            else if (baseMovementVectorX < 0)
                baseMovementVectorX = -1;
            else
                baseMovementVectorX = 0;
            if (baseMovementVectorY > 0)
                baseMovementVectorY = 1;
            else if (baseMovementVectorY < 0)
                baseMovementVectorY = -1;
            else
                baseMovementVectorY = 0;
            baseMovementVector = new Coords(baseMovementVectorX, baseMovementVectorY);
            return baseMovementVector;
        }
        public override void CalculateBoundCoords(Coords Position, out Coords BottomRight, out Coords Topleft)
        {
            //Coords MovementVector = DetermineLightsToActivate.GetMMovementVector(_occupant);
            Coords MovementVector = null;
            double BottomRightX, BottomRightY;
            double TopLeftX, TopLeftY;
            Coords baseMoveVector = GetBaseMoveVector(MovementVector);
            if (MovementVector.x == 0 && MovementVector.y > 0)
            {
                TopLeftX = Position.x - _width;
                TopLeftY = Position.y;
                BottomRightX = Position.x + _width;
                BottomRightY = Position.y + _height;
            }
            else if (MovementVector.x < 0 && MovementVector.y == 0)
            {
                TopLeftX = Position.x;
                TopLeftY = Position.y + _width;
                BottomRightX = Position.x - _height;
                BottomRightY = Position.y - _width;
            }
            else if (MovementVector.x == 0 && MovementVector.y < 0)
            {
                TopLeftX = Position.x + _width;
                TopLeftY = Position.y;
                BottomRightX = Position.x - _width;
                BottomRightY = Position.y - _height;
            }
            else
            {
                TopLeftX = Position.x;
                TopLeftY = Position.y - _width;
                BottomRightX = Position.x + _height;
                BottomRightY = Position.y + _width;
            }
            
            BottomRight = new Coords(BottomRightX, BottomRightY);
            Topleft = new Coords(TopLeftX, TopLeftY);
        }
    }
}
