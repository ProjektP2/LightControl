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
            boundable.CalculateBoundCoords(_position, out TopLeft, out BottomRight);
        }
        public bool Contains(Bounds bound)
        {
            return (bound.x >= _position.x && bound.y >= _position.y &&
                bound.x <= BottomRight.x &&
                bound.y <= BottomRight.y);
        }

        public bool Intersects(Bounds bound)
        {
            BottomRight = new Coords(_position.x + Width, _position.y + Height);
            TopLeft = new Coords(_position.x, _position.y);

            bool boundCheck = !((bound.BottomRight.y < TopLeft.y) || (bound.TopLeft.y > BottomRight.y) ||
                               (bound.BottomRight.x < TopLeft.x) || (bound.TopLeft.x > BottomRight.x));

            bool boundCheck2 = !((bound.BottomRight.x > BottomRight.x) || (bound.BottomRight.y > BottomRight.y) ||
                                (bound.TopLeft.x < TopLeft.x) || (bound.TopLeft.y < TopLeft.y));

            return boundCheck || boundCheck2;
        }
    }
}
