using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeStructure;
using LightControl;
using Triangulering;

namespace TreeStructure
{
    /*public abstract class Creator
    {
        public abstract Query Create();
    }

    public class QuadTreeCreator
    {
        public Coords StartPosition { get { return _startPosition; } set { _startPosition = value; } }
        public int Width { get { return _width; } set { _width = value; } }
        public int Height { get { return _height; } set { _height = value; } }
        private Coords _startPosition = new Coords(0, 0);
        private int _width = 0;
        private int _height = 0;

        public QuadTreeCreator(int width, int height)
        {
            _width = width;
            _height = height;
        }
        public QuadTree Create()
        {
            Bounds MapBound = new Bounds(StartPosition, _width, _height);

            return new QuadTree(MapBound);
        }
    }

    public class RadiusQueryCreator : Creator
    {
        private int _radius;

        public int Radius
        {
            get { return _radius; }
            set { _radius = value; }
        }
        public QuadTree Tree { get; set; }
        
        public RadiusQueryCreator(int radius, QuadTree tree)
        {
            _radius = radius;
            Tree = tree;
        }
        public override Query Create()
        {
            return new RadiusSearchQuery(_radius, Tree.bound, Tree);
        }
    }

    public class VectorQueryCreator : Creator
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public IMovementVectorProvider MovementVector { get; set; }
        public QuadTree Tree { get; set; }
        public Occupant Occupant { get; set; }
        public VectorQueryCreator(IMovementVectorProvider movementVector, QuadTree tree, Occupant occupant)
        {
            MovementVector = movementVector;
            Tree = tree;
            Occupant = occupant;
        }
        public override Query Create()
        {
            return new VectorSearchQuery(Tree.bound, Tree, Occupant, MovementVector);
        }
    }*/
}
