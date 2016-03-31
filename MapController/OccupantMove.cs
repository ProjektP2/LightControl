using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using SimEnvironment;

namespace Triangulering
{
    class OccupantMove
    {
        private Collision collision;
        private Bitmap Map;

        private int PlayerSpeed = 3;
        private bool right, left, up, down;

        public OccupantMove(Bitmap map)
        {
            Map = map;
            collision = new Collision(Map);
        }
        public void NoPress(KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.D:
                case Keys.Right:
                    right = false;
                    break;
                case Keys.A:
                case Keys.Left:
                    left = false;
                    break;
                case Keys.W:
                case Keys.Up:
                    up = false;
                    break;
                case Keys.S:
                case Keys.Down:
                    down = false;
                    break;
            }
        }
        public void Press(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D:
                case Keys.Right:
                    right = true;
                    break;
                case Keys.A:
                case Keys.Left:
                    left = true;
                    break;
                case Keys.W:
                case Keys.Up:
                    up = true;
                    break;
                case Keys.S:
                case Keys.Down:
                    down = true;
                    break;
            }
        }
        public Point PlayerMove(Point EmployerPosition)
        {
            //Checks out if the direction is blocked 
            if (up)
            {
                if (collision.CheckCollison(EmployerPosition.X, EmployerPosition.Y - PlayerSpeed, -6, 0, 6, 0))
                   EmployerPosition.Y -= PlayerSpeed;
            }
            if (down)
            {
                if (collision.CheckCollison(EmployerPosition.X, EmployerPosition.Y + PlayerSpeed, -6, 2, 6, 2))
                    EmployerPosition.Y += PlayerSpeed;
            }
            if (left)
            {
                if (collision.CheckCollison(EmployerPosition.X - PlayerSpeed, EmployerPosition.Y, -6, 0, -6, 2))
                    EmployerPosition.X -= PlayerSpeed;
            }
            if (right)
            {
                if (collision.CheckCollison(EmployerPosition.X + PlayerSpeed, EmployerPosition.Y, 6, 0, 6, 2))
                    EmployerPosition.X += PlayerSpeed;
            }
               
            return EmployerPosition;
        }
    }
}
