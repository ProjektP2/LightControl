using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using SimEnvironment;
using LightControl;

namespace Triangulering
{
    class OccupantMove
    {
        Collision collision;
        Bitmap Map;
        Keys _key_forward, _key_backwards, _key_left, _key_right;

        private Form _window;
        TrackBar trackbar1 = new TrackBar();
        Label labeltrack = new Label();
        private int PlayerSpeed;
        bool right, left, up, down;
        public OccupantMove(Bitmap map, Form window, char key_forward, char key_backwards, char key_left, char key_right)
        {
            _window = window;
            Map = map;
            _key_forward = (Keys)key_forward;
            _key_backwards = (Keys)key_backwards;
            _key_left = (Keys)key_left;
            _key_right = (Keys)key_right;

            ((System.ComponentModel.ISupportInitialize)(trackbar1)).BeginInit();
            trackbar1.Size = new Size(150, 100);
            trackbar1.Location = new Point(520, 10);
            trackbar1.Visible = true;
            trackbar1.TabIndex = 1;
            trackbar1.Maximum = 10;
            trackbar1.LargeChange = 1;
            _window.Controls.Add(trackbar1);
            labeltrack.Location = new Point(trackbar1.Location.X + trackbar1.Size.Width + 1, 10);
            labeltrack.Visible = true;
            _window.Controls.Add(labeltrack);

            _window.KeyDown += new KeyEventHandler(this.Form1_KeyDown);
            _window.KeyUp += new KeyEventHandler(this.Form1_KeyUp);
            Map = map;
            collision = new Collision(Map);

        }

        private void trackBar1_Scroll()
        {
            PlayerSpeed = trackbar1.Value;
            labeltrack.Text = ("" + trackbar1.Value);
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == _key_forward)
                up = true;
            if (e.KeyCode == _key_backwards)
                down = true;
            if (e.KeyCode == _key_left)
                left = true;
            if (e.KeyCode == _key_right)
                right = true;

        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == _key_forward)
                up = false;
            if (e.KeyCode == _key_backwards)
                down = false;
            if (e.KeyCode == _key_left)
                left = false;
            if (e.KeyCode == _key_right)
                right = false;
            if (e.KeyCode == Keys.E)
            {
                trackbar1.Value++;
                trackBar1_Scroll();
            }
            if (e.KeyCode == Keys.Q)
            {
                trackbar1.Value--;
                trackBar1_Scroll();
            }
        }

        public Coords PlayerMove(Coords EmployerPosition)
        {
            if (up == true)
            {
                //Checks out if the direction is blocked 
                if (collision.CheckCollison(EmployerPosition.x, EmployerPosition.y - PlayerSpeed, -6, 0, 6, 0))
                    EmployerPosition.y -= PlayerSpeed;
            }
            if (down == true)
            {
                if (collision.CheckCollison(EmployerPosition.x, EmployerPosition.y + PlayerSpeed, -6, 2, 6, 2))
                    EmployerPosition.y += PlayerSpeed;
            }
            if (left == true)
            {
                if (collision.CheckCollison(EmployerPosition.x - PlayerSpeed, EmployerPosition.y, -6, 0, -6, 2))
                    EmployerPosition.x -= PlayerSpeed;
            }
            if (right == true)
            {
                if (collision.CheckCollison(EmployerPosition.x + PlayerSpeed, EmployerPosition.y, 6, 0, 6, 2))
                    EmployerPosition.x += PlayerSpeed;
            }
            return EmployerPosition;
        }
    }
}
