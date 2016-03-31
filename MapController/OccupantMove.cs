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
        private Form _window;
        private int PlayerSpeed = 3;
        bool right, left, up, down;
        public OccupantMove(Bitmap map, Form window)
        {
            _window = window;
            _window.KeyDown += new KeyEventHandler(this.Form1_KeyDown);
            _window.KeyUp += new KeyEventHandler(this.Form1_KeyUp);
            _window.KeyPreview = true;
            Map = map;
            collision = new Collision(Map);
        }

        public void NoPress(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right)
                right = false;
            if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left)
                left = false;
            if (e.KeyCode == Keys.W || e.KeyCode == Keys.Up)
                up = false;
            if (e.KeyCode == Keys.S || e.KeyCode == Keys.Down)
                down = false;
        }
        public void Press(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W || e.KeyCode == Keys.Up)
                up = true;
            if (e.KeyCode == Keys.S || e.KeyCode == Keys.Down)
                down = true;
            if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left)
                left = true;
            if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right)
                right = true;
        }
        public Point PlayerMove(Point EmployerPosition)
        {
            if (up == true)
            {
                //Checks out if the direction is blocked 
                if (collision.CheckCollison(EmployerPosition.X, EmployerPosition.Y - PlayerSpeed, -6, 0, 6, 0))
                   EmployerPosition.Y -= PlayerSpeed;
            }
            if (down == true)
            {
                if (collision.CheckCollison(EmployerPosition.X, EmployerPosition.Y + PlayerSpeed, -6, 2, 6, 2))
                    EmployerPosition.Y += PlayerSpeed;
            }
            if (left == true)
            {
                if (collision.CheckCollison(EmployerPosition.X - PlayerSpeed, EmployerPosition.Y, -6, 0, -6, 2))
                    EmployerPosition.X -= PlayerSpeed;
            }
            if (right == true)
            {
                if (collision.CheckCollison(EmployerPosition.X + PlayerSpeed, EmployerPosition.Y, 6, 0, 6, 2))
                    EmployerPosition.X += PlayerSpeed;
            }
            return EmployerPosition;
        }

        public void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Press(e);
        }
        public void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            NoPress(e);
        }
    }
}
