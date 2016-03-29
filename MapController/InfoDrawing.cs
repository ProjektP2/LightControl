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
        public void init()
        {
            
            lb.Location = p;
            lb.Visible = true;
            lb.Show();
            window.Controls.Add(lb);
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
    }
}
