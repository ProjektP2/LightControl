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
using System.Reflection;


namespace MapController
{
    public class InfoScreen
    {
        Form CurrentWindow;
        DALIController _controller;
        Label LightunitInfo;
        LightingUnit WantedUnit;

        public InfoScreen(Form window, DALIController Controller)
        {
            CurrentWindow = window;
            _controller = Controller;
            LightunitInfo = new Label();
            LightunitInfo.Location = new Point(GEngine.SimulationWidht / 2 + CurrentWindow.Width / 2, CurrentWindow.Height / 2- GEngine.SimulationHeigt / 4);
            LightunitInfo.AutoSize = true;
            CurrentWindow.Controls.Add(LightunitInfo);
        }

        public void DisplayLightingUnitInfo(LightingUnit CurrentUnit)
        {
            if (CurrentUnit != null)
            {
                LightunitInfo.Text = "Address: " + CurrentUnit.Address + Environment.NewLine
                        + "Current level of light: " + (CurrentUnit.LightingLevel * 100).ToString("F0") + "%" + Environment.NewLine
                        + "X coordinate: " + CurrentUnit.x + Environment.NewLine
                        + "Y coordinate: " + CurrentUnit.y + Environment.NewLine
                        + "Unit is working: " + CurrentUnit.IsUnitOn + Environment.NewLine
                        + "Unit is part of groups: " + GetUnitGroups(CurrentUnit);
            }

        }

        private string GetUnitGroups(LightingUnit Unit)
        {
            List<int> groupNumbers = new List<int>();
            for (int i = 0; i < _controller._groups.Length-1; i++)
            {
                if (_controller._groups[i].Contains(Unit))
                {
                    groupNumbers.Add(i);
                }
            }

            string groupNumbersAsString = "";

            foreach (var number in groupNumbers)
            {
                groupNumbersAsString += number.ToString()  + ", ";
            }

            return groupNumbersAsString;
        }

        public void ChangeCurrentLightingUnit(LightingUnit Unit)
        {
            LightunitInfo.Visible = true;
            WantedUnit = Unit;
        }

        public LightingUnit getWantedUnit()
        {
            return WantedUnit;
        }
    }
}
