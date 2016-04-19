using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightControl;
using MapController.Quadtree;

namespace TreeStructure
{
    public class Bounds : Coords
    {
        public int Width;
        public int Height;
        private Coords _position = new Coords();
        private Coords BottomRight;
        private Coords TopLeft;
        private IBoundable _boundable;

        public Bounds(Coords position, int width, int height)
        {
            _position.x = position.x;
            _position.y = position.y;
            this.x = position.x;
            this.y = position.y;
            Width = width;
            Height = height;
        }

        public void InitializeBoundable(IBoundable boundable)
        {
            _boundable = boundable;
            boundable.CalculateBoundCoords(_position, out BottomRight, out TopLeft);
        }
        public bool Contains(Bounds bound)
        {
            return (bound.x >= _position.x && bound.y >= _position.y &&
                bound.x <= BottomRight.x &&
                bound.y <= BottomRight.y);
        }

        public bool Intersects(Bounds bound)
        {
            bool boundCheck = !((bound.BottomRight.x <= _position.x) ||
                         (bound.BottomRight.y <= _position.y) ||
                         (bound.TopLeft.x >= _position.x + Width) ||
                         (bound.TopLeft.y >= _position.y + Height));
            if (!boundCheck)
            {
                bool debug1 = (bound.BottomRight.x >= _position.x + Width);
                bool debug2 = (bound.BottomRight.y >= _position.y + Height);
                bool debug3 = (bound.TopLeft.x <= _position.x);
                bool debug4 = (bound.TopLeft.y <= _position.y);
                boundCheck = !(debug1 || debug2 || debug3 || debug4);
            }
            return boundCheck;
            /*
            if (bound._boundable?.GetType() == typeof (VectorSearchQuery))
            {
                bool debug1 = !(bound.BottomRight.x <= _position.x);
                bool debug2 = !(bound.BottomRight.y <= _position.y);
                bool debug3 = !(bound.TopLeft.x >= _position.x + Width);
                bool debug4 = !(bound.TopLeft.y >= _position.y + Height);
                bool debug1 = (bound.BottomRight.x >= _position.x + Width);
                bool debug2 = (bound.BottomRight.y >= _position.y + Height);
                bool debug3 = (bound.TopLeft.x <= _position.x);
                bool debug4 = (bound.TopLeft.y <= _position.y);
                return !(debug1 || debug2 || debug3 || debug4);
                //return true;
            }
            else
            {
                return !((bound.BottomRight.x <= _position.x) ||
                         (bound.BottomRight.y <= _position.y) ||
                         (bound.TopLeft.x >= _position.x + Width) ||
                         (bound.TopLeft.y >= _position.y + Height));
            } */
        }
    }
}
