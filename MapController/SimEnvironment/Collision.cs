using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace SimEnvironment
{
    class Collision
    {
        Bitmap Map;
        double XX;
        double YY;
        int XPosX;
        int YPosY;
        public Collision(Bitmap map)
        {
            Map = map;
        }
        private bool GetSurce(string pixelColorStringValue)
        {
            bool move;
            switch (pixelColorStringValue)
            {
                //Determine which tiles is that can be walked on and not walked on
                case "255020147": move = true; break;
                case "255255000": move = false; break;
                case "075000130": move = true; break;
                case "240128128": move = false; break;
                default: move = false; break;
                    //Test
            }
            return move;
        }
        //Read color code from the Map
        private bool ReadFromMap(int x, int y)
        {
            Color PixelCode = Map.GetPixel(x, y);
            string pixelColorStringValue =
            PixelCode.R.ToString("D3") + "" +
            PixelCode.G.ToString("D3") + "" +
            PixelCode.B.ToString("D3") + "";
            return GetSurce(pixelColorStringValue);
        }

        // Determine the position of the player on the map int teils.
        private void CollisonPosition(int posX, int posY)
        {
            XX = posX / 32;
            YY = posY / 32;
            XPosX = Convert.ToInt32((Math.Ceiling(XX)));
            YPosY = Convert.ToInt32((Math.Ceiling(YY)));
        }
        //
        private bool noget(int posx, int posy, int x, int y)
        {

            CollisonPosition(posx + x, posy + y);
            return ReadFromMap(XPosX, YPosY);
        }


        public bool CheckCollisonLeft(int posx, int posy, int x1, int y1, int x2, int y2)
        {
            //First position

            if (noget(posx, posy, x1, y1) == true)
                return noget(posx, posy, x2, y2);
            else
                return false;
            /*
            if (noget(posx, posy, 2, 2) == true)
                return noget(posx, posy, 2, 30);
            else
                return false*/
        }
        public bool CheckCollisonRight(int posx, int posy)
        {
            bool CanMove = true;
            //First position
            CollisonPosition(posx+30, posy+2);
            CanMove = ReadFromMap(XPosX, YPosY);
            //Second position
            CollisonPosition(posx+30, posy + 30);
            if (CanMove == true)
                CanMove = ReadFromMap(XPosX, YPosY);
            return CanMove;
        }
        public bool CheckCollisonDown(int posx, int posy)
        {
            bool CanMove = true;
            //First position
            CollisonPosition(posx+2, posy + 30);
            CanMove = ReadFromMap(XPosX, YPosY);
            //Second position
            CollisonPosition(posx + 30, posy + 30);
            if (CanMove == true)
                CanMove = ReadFromMap(XPosX, YPosY);
            return CanMove;
        }
        public bool CheckCollisonUp(int posx, int posy)
        {
            bool CanMove = true;
            //First position
            CollisonPosition(posx+2, posy+2);
            CanMove = ReadFromMap(XPosX, YPosY);
            //Second position
            CollisonPosition(posx + 30, posy+2);
            if (CanMove == true)
                CanMove = ReadFromMap(XPosX, YPosY);
            return CanMove;
        }
    }
}
