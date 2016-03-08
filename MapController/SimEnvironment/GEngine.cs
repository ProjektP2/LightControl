using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace SimEnvironment
{
    class GEngine
    {
        // Size of the const.
        public const int FormHeigt = 640;
        public const int FormWidht = 750;
        public const int TileSize = 32;

        GraphicsDraw grapihicsDraw;
        Collision collision;
        Bitmap Map;
        Form window;

        public bool Running = true;
        //Starting position
        private int playerX = 4*32;
        private int playerY = 4*32;
        // The Speed the player walks with
        private int PlayerSpeed = 3;
        bool right, left, up, down;
        int frameRendered = 0;
        int fps = 0;
        long startTime = Environment.TickCount;

        public GEngine(Form form)
        {
            window = form;
        }
        public void Init()
        {
            grapihicsDraw = new GraphicsDraw(window, Map);
            collision = new Collision(Map);
            grapihicsDraw.Begin();
            grapihicsDraw.DrawMap();
            grapihicsDraw.DrawLamps();
            StartGameLoop();
        }
        //Load the Map from a picture
        public void LoadLevel()
        {
            Map = new Bitmap("Map3.png");
        }
        private void StartGameLoop()
        {

            do
            {
                //makes the computer not to fuck up!
                Application.DoEvents();
                //
                PlayerMove();
                grapihicsDraw.Position();
                grapihicsDraw.Draw(playerX, playerY, fps);
                FPS();

            } while (Running);
            Application.Exit();
        }
        private void FPS()
        {
            frameRendered++;
            if (Environment.TickCount >= startTime + 1000)
            {
                fps = frameRendered;
                frameRendered = 0;
                startTime = Environment.TickCount;
            }
        }


        //Move the player
        public void NoPress(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.D)
                right = false;
            if (e.KeyCode == Keys.A)
                left = false;
            if (e.KeyCode == Keys.W)
                up = false;
            if (e.KeyCode == Keys.S)
                down = false;
        }
        public void Press(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
                up = true;
            if (e.KeyCode == Keys.S)
                down = true;
            if (e.KeyCode == Keys.A)
                left = true;
            if (e.KeyCode == Keys.D)
                right = true;
        }
        public void PlayerMove()
        {
            if (up == true)
            {
                //Checks out if the direction is blocked 
                if (collision.CheckCollison(playerX, playerY - PlayerSpeed,2,2,30,2))
                    playerY -= PlayerSpeed;
            }
            if (down == true)
            {
                //Checks out if the direction is blocked
                if (collision.CheckCollison(playerX, playerY+ PlayerSpeed,2,30,30,30))
                    playerY += PlayerSpeed;
            }
            if (left == true)
            {
                //Checks out if the direction is blocked
                if (collision.CheckCollison(playerX - PlayerSpeed, playerY, 2,2,2,30))
                    playerX -= PlayerSpeed;
            }
            if (right == true)
            {
                //Checks out if the direction is blocked
                if (collision.CheckCollison(playerX + PlayerSpeed, playerY, 30, 2, 30, 30))
                    playerX += PlayerSpeed;
            }
        }
    }
}
