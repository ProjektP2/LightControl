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

            RemoveClickEvents();
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
                LightingUnit CurrentUnit = DALIController.findUnitWithAddress(Convert.ToInt32(TextBoxForInput.Value));
                currentindex = AllLights.IndexOf(CurrentUnit);

                CallOnAdressActions();
            }
        }

        private void ShowGroups(object sender, EventArgs e)
        {
            RemoveClickEvents();
            for (int i = 0; i < Buttons.Count(); i++)
            {
                Buttons[i].Visible = true;
                Buttons[i].Text = "Group[" + i + "]"; 
            }
        }



        private void CallOnAdressActions()
        {
            Buttons[0].Visible = true;
            Buttons[0].Text = "Add to";
            Buttons[0].Click += new EventHandler(ShowGroups);
            Buttons[1].Visible = true;
            Buttons[1].Text = "Remove from";
            Buttons[1].Click += new EventHandler(ShowGroups);
            Buttons[2].Visible = true;
            Buttons[2].Text = "Go To Scene";
            //Buttons[2].Click += new EventHandler(ShowGroups);
            Buttons[3].Visible = true;
            Buttons[3].Text = "Add to Group";
            //Buttons[3].Click += new EventHandler(ShowGroups);


        }


        private void testButton2test_Click(object sender, EventArgs e)
        {
            DALIController.AddUnitToGroup(DALIController.findUnitWithAddress(80), 0);
            Button btn = (Button)sender;
            btn.Visible = true;
            Console.WriteLine("hej mor");
        }


        private void RemoveClickEvents()
        {
            foreach (var item in Buttons)
            {
                FieldInfo f1 = typeof(Control).GetField("EventClick",
                BindingFlags.Static | BindingFlags.NonPublic);
                object obj = f1.GetValue(item);
                PropertyInfo pi = item.GetType().GetProperty("Events",
                    BindingFlags.NonPublic | BindingFlags.Instance);
                EventHandlerList list = (EventHandlerList)pi.GetValue(item, null);
                list.RemoveHandler(obj, list[obj]);
            }
        }
    }
}
