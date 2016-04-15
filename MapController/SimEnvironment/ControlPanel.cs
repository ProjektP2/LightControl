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

namespace MapController.SimEnvironment
{
    class ControlPanel
    {
        Form window;
        DALIController DALIController;
        List<LightingUnit> AllLights;
        List<Button> Buttons;
        NumericUpDown TextBoxForInput;
        int currentindex;
        int StartButtonY;
        int StartButtonX;
        int newStartButtonX;
        int newStartButtonY;
        int boundX;
        int ButtonHeight;
        int ButtonLength;


        public ControlPanel(Form form, DALIController Controller, List<LightingUnit> Lights)
        {
            window = form;
            DALIController = Controller;
            AllLights = Lights;
            InitializeControlPanelButtons();
        }

        public void InitializeControlPanelButtons()
        {
            boundX = (window.Width/2-GEngine.SimulationWidht/2);
            ButtonHeight = window.Height / 33;
            StartButtonX = boundX / 25;
            StartButtonY = window.Height / 7;
            ButtonLength = boundX/ 5;
            newStartButtonX = StartButtonX;
            newStartButtonY = StartButtonY;
            Buttons = new List<Button>();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Button newButton = new Button();
                    Buttons.Add(newButton);
                    newButton.Location = new System.Drawing.Point(newStartButtonX, newStartButtonY);
                    newButton.Size = new System.Drawing.Size(ButtonLength, ButtonHeight);
                    newButton.TabIndex = 0;
                    newButton.UseVisualStyleBackColor = true;
                    window.Controls.Add(newButton);
                    double DistanceBetweenButtonsHorisontally = ButtonLength * 1.2;
                    newStartButtonX += (int)Math.Floor(DistanceBetweenButtonsHorisontally);
                    newButton.Visible = false;
                }
                newStartButtonX = StartButtonX;
                double DistanceBetweenButtonsVertically = ButtonHeight * 1.5;
                newStartButtonY += (int)Math.Floor(DistanceBetweenButtonsVertically);
            }
            for(int i = 0; i < 3; i++)
            {
                Buttons[i].Visible = true;
            }
            Buttons[0].Text = "Call Address";
            Buttons[1].Text = "Call Group";
            Buttons[2].Text = "Broadcast";

            Buttons[0].Click += new EventHandler(CallAddress);
            Buttons[2].Click += new EventHandler(testButton2test_Click);

            TextBoxForInput = InitializeTextBox();
            TextBoxForInput.Location = new Point(StartButtonX, StartButtonY);
        }

        private NumericUpDown InitializeTextBox()
        {
            NumericUpDown CreatedTextBox = new NumericUpDown();
            window.Controls.Add(CreatedTextBox);
            CreatedTextBox.Visible = false;

            return CreatedTextBox;
        }

        private void CallAddress(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            foreach (var button in Buttons)
            {
                button.Visible = false;
            }
            TextBoxForInput.KeyPress += new KeyPressEventHandler(TextBoxForInput_KeyPress);
            TextBoxForInput.Location = new Point(StartButtonX, StartButtonY + ButtonHeight);
            TextBoxForInput.Visible = true;
            TextBoxForInput.Text = "Input address:";
            TextBoxForInput.Enter += new EventHandler(TextBoxForInput_Entered);
        }


        private void TextBoxForInput_Entered(object sender, EventArgs e)
        {
            TextBoxForInput.ResetText();
        }

        private void TextBoxForInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                TextBoxForInput.Visible = false;
                Buttons[1].Visible = true;
                Buttons[2].Visible = true;
                Buttons[0].Visible = true;
                LightingUnit CurrentUnit = DALIController.findUnitWithAddress(Convert.ToInt32(TextBoxForInput.Value));
                DALIController.AddUnitToGroup(CurrentUnit, 5);
                CurrentUnit.ForcedLightlevel = 1;
                Buttons[0].Text = "Call Address";
                Buttons[1].Text = "Call Group";
                Buttons[2].Text = "Broadcast";

                Buttons[0].Click += new EventHandler(CallAddress);
                Buttons[2].Click += new EventHandler(testButton2test_Click);

                currentindex = AllLights.IndexOf(CurrentUnit);

            }
        }



        private void testButton2test_Click(object sender, EventArgs e)
        {
            DALIController.AddUnitToGroup(DALIController.findUnitWithAddress(80), 0);
            Button btn = (Button)sender;
            btn.Visible = true;
            Console.WriteLine("hej mor");
        }
    }
}
