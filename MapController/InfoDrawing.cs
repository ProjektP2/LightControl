using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using MapController.SimEnvironment;
using LightControl;
using Triangulering;
using SimEnvironment;

namespace LightControl

{
    class InfoDrawing
    {
        Form window;
        public InfoDrawing(Form form)
        {
            window = form;
        }
        long startTime = Environment.TickCount;
        Label Router1 = new Label();
        Label Router2 = new Label();
        Label WiFiPos1 = new Label();
        Label WiFiPos2 = new Label();
        Label Pos1 = new Label();
        Label Pos2 = new Label();
        TextBox tb = new TextBox();
        Point p;
        Point RouterP1;
        Point WiFiPosP1;
        Point PosP1;
        Point RouterP2;
        Point WiFiPosP2;
        Point PosP2;
        Label watts = new Label();
        public void initSignalInfo()
        {
            RouterP1 = new Point(window.Width/4*3, window.Height / 7 + 50);
            RouterP2 = new Point(RouterP1.X, RouterP1.Y + Router1.Height);
            Router1.Location = RouterP1;
            Router2.Location = RouterP2;
            Router1.AutoSize = true;
            Router2.AutoSize = true;
            Router1.Visible = true;
            Router2.Visible = true;
            Router1.Show();
            Router2.Show();
            window.Controls.Add(Router1);
            window.Controls.Add(Router2);
        }
        public void InitBrugerPosWiFi()
        {
            WiFiPosP1 = new Point(RouterP1.X + window.Width / 15, RouterP1.Y);
            WiFiPosP2 = new Point(RouterP1.X + window.Width / 15 , RouterP1.Y + WiFiPos1.Height);
            WiFiPos1.Location = WiFiPosP1;
            WiFiPos2.Location = WiFiPosP2;
            WiFiPos1.AutoSize = true;
            WiFiPos2.AutoSize = true;
            WiFiPos1.Visible = true;
            WiFiPos2.Visible = true;
            WiFiPos1.Show();
            WiFiPos2.Show();
            window.Controls.Add(WiFiPos1);
            window.Controls.Add(WiFiPos2);
        }
        public void InitBrugerPos()
        {
            PosP1 = new Point(WiFiPosP1.X + window.Width / 15, RouterP1.Y);
            PosP2 = new Point(PosP1.X, PosP1.Y + Pos1.Height);
            Pos1.Location = PosP1;
            Pos2.Location = PosP2;
            Pos1.AutoSize = true;
            Pos2.AutoSize = true;
            Pos1.Visible = true;
            Pos2.Visible = true;
            Pos1.Show();
            Pos2.Show();
            window.Controls.Add(Pos1);
            window.Controls.Add(Pos2);
        }


        public void initWattInfo()
        {
            watts.Location = new Point(window.Width/4 * 3, window.Height/7);
            watts.AutoSize = true;
            watts.Visible = true;
            watts.Show();
            window.Controls.Add(watts);

        }

        public void BrugerWiFi(Coords p)
        {
            WiFiPos1.Text = ("P X: " + p.x.ToString("F2"));
            WiFiPos2.Text = ("P Y: " + p.y.ToString("F2"));
        }

        public void Brugerpos(Coords p)
        {
            Pos1.Text = ("Bruger X: " + p.x.ToString("F2"));
            Pos2.Text = ("Bruger Y: " + p.y.ToString("F2"));
        }

        public void SignalInfo(double radius1, double radius2)
        {
            Router1.Text = ("Router 1: " + radius1.ToString("F2"));
            Router2.Text = ("Router 2: " + radius2.ToString("F2"));
        }

        public void WattUsageInfo(double WattsUsed)
        {
            if (Environment.TickCount >= startTime + 100)
            {
                watts.Text =("Total watt usage: " + WattsUsed.ToString("F2"));
                startTime = Environment.TickCount;
            }
        }
    }
}
