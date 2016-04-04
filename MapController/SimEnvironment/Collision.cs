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
        private bool Check(int posx, int posy, int x, int y)
        {

            CollisonPosition(posx + x, posy + y);
            return ReadFromMap(XPosX, YPosY);
        }
        public bool CheckLightCollision(int posx, int posy)
        {
            if (Check(posx, posy, 0, 0) == true)
                return Check(posx, posy, 0, 0);
            else
                return false;
        }
        
        public bool CheckCollison(double posx, double posy, int x1, int y1, int x2, int y2)
        {
            if (Check((int)posx, (int)posy, x1, y1) == true)
                return Check((int)posx, (int)posy, x2, y2);
            else
                return false;
        }



    }
}
