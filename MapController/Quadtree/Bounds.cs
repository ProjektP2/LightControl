using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightControl;

namespace MapController.QuadTree
{
    class Bounds : Coords
    {
        public int Width;
        public int Height;
        private int CenterX, CenterY;
        private int TopRightX, TopRightY;

        public Bounds(double x, double y, int width, int height)
        {
            this.x = x;
            this.y = y;
            Width = width;
            Height = height;
            TopRightX = (int)x + width;
            TopRightY = (int)y;
        }

        public bool Contains(Bounds bound)
        {
            return bound.x >= x && bound.y >= y && 
                bound.TopRightX <= TopRightX && 
                bound.TopRightY <= TopRightY;
        }

        public bool Intersects(Bounds bound)
        {
            return (bound.x >= TopRightX || bound.TopRightX <= x ||
                bound.y >= TopRightY || bound.TopRightY <= y); 
        }
    }
}
