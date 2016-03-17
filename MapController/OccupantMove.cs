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
        Collision collision;
        

        Bitmap Map;
        private int PlayerSpeed = 3;
        bool right, left, up, down;
        public OccupantMove(Bitmap map)
        {
            
            Map = map;
            collision = new Collision(Map);
        }

        internal Collision Collision
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

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
        public Point PlayerMove(Point EmployerPosition)
        {
            if (up == true)
            {
                //Checks out if the direction is blocked 
                if (collision.CheckCollison(EmployerPosition.X, EmployerPosition.Y - PlayerSpeed, 2, 2, 16, 2))
                   EmployerPosition.Y -= PlayerSpeed;
            }
            if (down == true)
            {
                if (collision.CheckCollison(EmployerPosition.X, EmployerPosition.Y + PlayerSpeed, 2, 16, 16, 16))
                    EmployerPosition.Y += PlayerSpeed;
            }
            if (left == true)
            {
                if (collision.CheckCollison(EmployerPosition.X - PlayerSpeed, EmployerPosition.Y, 2, 2, 2, 16))
                    EmployerPosition.X -= PlayerSpeed;
            }
            if (right == true)
            {
                if (collision.CheckCollison(EmployerPosition.X + PlayerSpeed, EmployerPosition.Y, 16, 2, 16, 16))
                    EmployerPosition.X += PlayerSpeed;
            }
            return EmployerPosition;
        }
    }
}
