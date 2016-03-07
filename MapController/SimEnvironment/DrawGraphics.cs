using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace SimEnvironment
{
    class GraphicsDraw
    {
        Graphics BBG;
        Rectangle sRect;
        Rectangle dRect;
        Graphics G;
        Bitmap BB;
        Bitmap Map;
        Bitmap player;
        Bitmap teils;
        Form window;
        int posX, posY;
        public int mouseX, mouseY;

        public GraphicsDraw(Form form, Bitmap map)
        {
            Map = map;
            window = form;
        }
        public void Begin()
        {
            G = window.CreateGraphics();
            BB = new Bitmap(GEngine.FormWidht, GEngine.FormHeigt);
            player = new Bitmap("Player.png");
            teils = new Bitmap("Teils.png");
        }
        public void Position()
        {
            double xx = posX / 32;
            double YY = posY / 32;
            mouseX = Convert.ToInt32((Math.Floor(xx)));
            mouseY = Convert.ToInt32((Math.Floor(YY)));
        }

        private void GetSurce(string pixelColorStringValue)
        {
            switch (pixelColorStringValue)
             {
                //Diffrent Color codes, reads diffrent locations on the tile bitmap
                 case "255020147": sRect = new Rectangle(0, 0, GEngine.TileSize, GEngine.TileSize); break;
                 case "255255000": sRect = new Rectangle(32, 0, GEngine.TileSize, GEngine.TileSize); break;
                 case "075000130": sRect = new Rectangle(0, 0, GEngine.TileSize, GEngine.TileSize); break;
                 case "240128128": sRect = new Rectangle(32, 0, GEngine.TileSize, GEngine.TileSize); break;
                 default: new Rectangle(0, 0, GEngine.TileSize, GEngine.TileSize); break;
             }
        }

        public void Draw(int xpos, int ypos)
        {
            //Player possion
            posX = xpos;
            posY = ypos;
            // Draw the teils to the window
            for (int x = 0; x < Map.Width; x ++)
            {
                for (int y = 0; y < Map.Height; y ++)
                {
                    //Determine the color code of the pixel on the map
                    // And draw to the window
                    Color PixelCode = Map.GetPixel(x,y);
                    string pixelColorStringValue =
                        PixelCode.R.ToString("D3") + "" +
                        PixelCode.G.ToString("D3") + "" +
                        PixelCode.B.ToString("D3") + "";
                    GetSurce(pixelColorStringValue);
                    dRect = new Rectangle((x * GEngine.TileSize), (y * GEngine.TileSize), GEngine.TileSize, GEngine.TileSize);
                    G.DrawImage(teils, dRect, sRect, GraphicsUnit.Pixel);                
                }
   
            }
            //Player Drawing
            sRect = new Rectangle(0, 0, GEngine.TileSize, GEngine.TileSize);
            player.MakeTransparent(Color.CadetBlue);
            G.DrawImage(player, posX, posY, sRect, GraphicsUnit.Pixel);

            //Info Drawing
            G.DrawString("Map X:" + mouseX + "\r\n" +
                "Map Y" + mouseY, window.Font, Brushes.Black, 650, 0);
            //Draw it to the window
            G = Graphics.FromImage(BB);
            try
            {
                BBG = window.CreateGraphics();
                BBG.DrawImage(BB, 0, 0, GEngine.FormWidht, GEngine.FormHeigt);
                G.Clear(Color.Green);
            }
            catch (Exception)
            {    
            }

        }

    }
    
}
