using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightControl;

namespace TreeStructure
{
    class Bounds : Coords
    {
        public int Width;
        public int Height;
        public int BottomRightX, BottomRightY;
        public int TopLeftX, TopLeftY;

        public Bounds(double x, double y, int width, int height)
        {
            this.x = x;
            this.y = y;
            Width = width;
            Height = height;
            BottomRightX = (int)x + width;
            BottomRightY = (int)y + height;
            TopLeftX = (int)x - width;
            TopLeftY = (int)y - height;

        }

        public bool Contains(Bounds bound)
        {
            /*return bound.TopLeftX <= x && bound.TopLeftY <= y &&
                bound.BottomRightX >= BottomRightX && 
                bound.BottomRightY >= BottomRightY;*/
            return (bound.x >= x && bound.y >= y &&
                bound.x <= BottomRightX &&
                bound.y <= BottomRightY);
            //return true;

        }

        public bool Intersects(Bounds bound)
        {
            /*return !((bound.BottomRightX <= x) ||
                (bound.BottomRightY <= y) ||
                (bound.TopLeftX >= BottomRightX) ||
                (bound.TopLeftY >= BottomRightY)); */
            return !((bound.BottomRightX <= x) ||
                (bound.BottomRightY <= y) ||
                (bound.TopLeftX >= BottomRightX) ||
                (bound.TopLeftY >= BottomRightY));

        }
    }
}
