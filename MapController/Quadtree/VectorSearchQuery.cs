using System;
using LightControl;
using System.Collections.Generic;
using Triangulering;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace TreeStructure
{
    public class VectorSearchQuery : Query
    {
        private int _width;
        private int _height;
        private Occupant _occupant;
        IMovementVectorProvider _lightsToActivate;
        /*public override Bounds Bound
        {
            get { return _vectorBound; }
            set { _vectorBound = value; }
        }
        private Bounds _vectorBound;*/

        private Rectangle _vectorBound;
        public override Rectangle Bound
        {
            get { return _vectorBound; }
            set { _vectorBound = value; }
        }

        public VectorSearchQuery(Rectangle mapBound, QuadTree tree, Occupant occupant,
                                 IMovementVectorProvider lightsToactivate)
        {
            MapBound = mapBound;
            Tree = tree;
            _occupant = occupant;
            _width = 30;
            _height = 60;
            _lightsToActivate = lightsToactivate;
        }
        public override Rectangle GetBound(Coords entityPosition, int width, int height)
        {
            //Bounds Bound = new Bounds(entityPosition, width, height);
            Coords MovementVector = _lightsToActivate.GetMovementVector(_occupant);
            Coords baseMoveVector = GetBaseMoveVector(MovementVector);
            int TopLeftX, TopLeftY;
            int boundHeight, boundWidth;

            if (baseMoveVector.x >= 0 && baseMoveVector.y >= 0)
            {
                TopLeftX = (int)(entityPosition.x - _width * baseMoveVector.y);
                TopLeftY = (int)(entityPosition.y - _width * baseMoveVector.x);
                boundHeight = (int)(_height * MovementVector.y + _width * baseMoveVector.x * 2);
                boundWidth = (int)(_height * MovementVector.x + _width * baseMoveVector.y * 2);
            }
            else if (baseMoveVector.x <= 0 && baseMoveVector.y <= 0)
            {
                TopLeftX = (int)(entityPosition.x + _height * MovementVector.x + _width * baseMoveVector.y);
                TopLeftY = (int)(entityPosition.y + _height * MovementVector.y + _width * baseMoveVector.x);
                boundHeight = (int)(-1 * (_height * MovementVector.y + _width * baseMoveVector.x * 2));
                boundWidth = (int)(-1 * (_height * MovementVector.x + _width * baseMoveVector.y * 2));
            }
            else
            {
                if (baseMoveVector.x > 0 && baseMoveVector.y > 0)
                {
                    TopLeftX = (int)(entityPosition.x - _width * baseMoveVector.y);
                    TopLeftY = (int)(entityPosition.y - _width * baseMoveVector.x);
                    boundHeight = (int)(_height * MovementVector.x);
                    boundWidth = (int)(_height * MovementVector.x);
                }
                else if (baseMoveVector.x < 0 && baseMoveVector.y < 0)
                {
                    TopLeftX = (int)(entityPosition.x + _height * MovementVector.x);
                    TopLeftY = (int)(entityPosition.y + _height * MovementVector.x);
                    boundHeight = (int)(-1 * (_height * MovementVector.x));
                    boundWidth = (int)(-1 * (_height * MovementVector.x));
                }
                else if (baseMoveVector.x > 0 && baseMoveVector.y < 0)
                {
                    TopLeftX = (int)(entityPosition.x);
                    TopLeftY = (int)(entityPosition.y + _height * MovementVector.y);
                    boundHeight = (int)(_height * MovementVector.x);
                    boundWidth = (int)(_height * MovementVector.x);
                }
                else
                {
                    TopLeftX = (int)(entityPosition.x + _height * MovementVector.x);
                    TopLeftY = (int)(entityPosition.y);
                    boundHeight = (int)(_height * MovementVector.y);
                    boundWidth = (int)(_height * MovementVector.y);
                }
            }
            
            Point TopLeft = new Point(TopLeftX, TopLeftY);
            Size boundSize = new Size(boundWidth, boundHeight);
            Rectangle Bound = new Rectangle(TopLeft, boundSize);
            return Bound;
        }

        public override void SearchTree(Coords entityPosition, ref List<LightingUnit> list)
        {
            _vectorBound = GetBound(entityPosition, _width, _height);
            //_vectorBound.InitializeBoundable(this);
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
        public override void CalculateBoundCoords(Coords Position, out Coords TopLeft, out Coords BottomRight)
        {
            Coords MovementVector = _lightsToActivate.GetMovementVector(_occupant);
            Coords baseMoveVector = GetBaseMoveVector(MovementVector);

            TopLeft = GetTopBoundCoord(baseMoveVector, Position);
            BottomRight = GetBottomBoundCoord(MovementVector, baseMoveVector, Position);

            GetBottomBoundCoord(MovementVector, baseMoveVector, Position);
            GetTopBoundCoord(baseMoveVector, Position);
        }
        public Coords GetBottomBoundCoord(Coords movementVector, Coords baseMovementVector, Coords Position)
        {
            Coords MovementVector = movementVector;
            Coords BaseMovementVector = baseMovementVector;
            Coords BottomRight;
            double BottomRightX, BottomRightY;
            double width, height;

            
            
            width = _width * BaseMovementVector.x + _width * BaseMovementVector.y;
            height = _height * MovementVector.x + _height * MovementVector.y;

            if (height != 0)
            {
                BottomRightX = Position.x  + height + width;
                BottomRightY = Position.y + height + width;
            }
            else
            {
                height = _height * MovementVector.x - _height * MovementVector.y;
                width = _width * BaseMovementVector.x - _width * BaseMovementVector.y;
                BottomRightX = Position.x + height + width;
                BottomRightY = Position.y + height + width;
            }
            
            return BottomRight = new Coords(BottomRightX, BottomRightY); 
        }
        public Coords GetTopBoundCoord(Coords baseMovevector, Coords Position)
        {
            Coords BaseMovementVector = baseMovevector;
            Coords TopLeft;
            double TopLeftX, TopLeftY;

            
            
            TopLeftX = Position.x - _width * BaseMovementVector.y;
            TopLeftY = Position.y - _width * BaseMovementVector.x;
            
            return TopLeft = new Coords(TopLeftX, TopLeftY);
        }
    }
}
