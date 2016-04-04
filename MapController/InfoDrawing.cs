using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triangulering;
using System.Windows.Forms;
using System.Drawing;

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
        Label lb = new Label();
        TextBox tb = new TextBox();
        Point p = new Point(50,50);
        Label watts = new Label();
        public void init()
        {
            
            lb.Location = p;
            lb.Visible = true;
            lb.Show();
            window.Controls.Add(lb);
        }

        public void initWattInfo()
        {
            watts.Location = p;
            watts.AutoSize = true;
            watts.Visible = true;
            watts.Show();
            window.Controls.Add(watts);

        }

        public void LightINFO(List<LightingUnit> ActivatedLightingUnitsOnUser)
        {
            if (Environment.TickCount >= startTime + 1000)
            {
                lb.Text = "";
                foreach (var item in ActivatedLightingUnitsOnUser)
                {
                    lb.Height++;
                    lb.Text += item.LightingLevel + "\n";
                }
                startTime = Environment.TickCount;
            }
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
