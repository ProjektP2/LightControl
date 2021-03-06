﻿using System;
using LightControl;
using System.Collections.Generic;
using Triangulering;
using System.Diagnostics;
using System.Drawing;
using Quadtree;

namespace TreeStructure
{
    public class VectorSearchQuery : Query
    {
        private int _width;
        private int _height;
        private Occupant _occupant;
        IMovementVectorProvider _lightsToActivate;
        
        private Rectangle _vectorBound;
        public override Rectangle Bound
        {
            get { return _vectorBound; }
            set { _vectorBound = value; }
        }

        public VectorSearchQuery(IBoundable mapBound, ISearchable tree, Occupant occupant,
                                 IMovementVectorProvider lightsToactivate)
        {
            MapBound = mapBound;
            Tree = tree;
            _occupant = occupant;
            _width = 30;
            _height = 60;
            _lightsToActivate = lightsToactivate;
        }
        /* Makes the vector bound for each direction 
         * This methods have a high cyclomatic complexity and should be structred differently */
        public override Rectangle GetBound(Coords entityPosition, int width, int height)
        {
            Coords MovementVector = _lightsToActivate.GetMovementVector(_occupant);
            Coords baseMoveVector = GetBaseMoveVector(MovementVector);
            int TopLeftX, TopLeftY;
            int boundHeight, boundWidth;
            // Rightward OR downward direction vector bound
            if (baseMoveVector.x >= 0 && baseMoveVector.y >= 0)
            {
                TopLeftX = (int)(entityPosition.x - _width * baseMoveVector.y);
                TopLeftY = (int)(entityPosition.y - _width * baseMoveVector.x);
                boundHeight = (int)(_height * MovementVector.y + _width * baseMoveVector.x * 2);
                boundWidth = (int)(_height * MovementVector.x + _width * baseMoveVector.y * 2);
            }
            // Leftward OR upward direction vector bound
            else if (baseMoveVector.x <= 0 && baseMoveVector.y <= 0)
            {
                TopLeftX = (int)(entityPosition.x + _height * MovementVector.x + _width * baseMoveVector.y);
                TopLeftY = (int)(entityPosition.y + _height * MovementVector.y + _width * baseMoveVector.x);
                boundHeight = (int)(-1 * (_height * MovementVector.y + _width * baseMoveVector.x * 2));
                boundWidth = (int)(-1 * (_height * MovementVector.x + _width * baseMoveVector.y * 2));
            }
            else
            {
                // Rightward AND Downward direction vector bound
                if (baseMoveVector.x > 0 && baseMoveVector.y > 0)
                {
                    TopLeftX = (int)(entityPosition.x - _width * baseMoveVector.y);
                    TopLeftY = (int)(entityPosition.y - _width * baseMoveVector.x);
                    boundHeight = (int)(_height * MovementVector.x);
                    boundWidth = (int)(_height * MovementVector.x);
                }
                // Leftward AND Upward direction vector bound
                else if (baseMoveVector.x < 0 && baseMoveVector.y < 0)
                {
                    TopLeftX = (int)(entityPosition.x + _height * MovementVector.x);
                    TopLeftY = (int)(entityPosition.y + _height * MovementVector.x);
                    boundHeight = (int)(-1 * (_height * MovementVector.x));
                    boundWidth = (int)(-1 * (_height * MovementVector.x));
                }
                // Rightward AND Upward direction vector bound
                else if (baseMoveVector.x > 0 && baseMoveVector.y < 0)
                {
                    TopLeftX = (int)(entityPosition.x);
                    TopLeftY = (int)(entityPosition.y + _height * MovementVector.y);
                    boundHeight = (int)(_height * MovementVector.x);
                    boundWidth = (int)(_height * MovementVector.x);
                }
                // Leftward AND Downward direction vector bound
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

        // Normalizes the movement vector length
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
    }
}
