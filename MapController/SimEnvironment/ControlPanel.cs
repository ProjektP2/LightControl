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
    public class ControlPanel
    {
        Form window;
        DALIController DALIController;
        List<LightingUnit> AllLights;
        List<Button> Buttons;
        NumericUpDown TextBoxForInput;
        Button ExtraButton;
        int currentAddress;
        int currentGroup;
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
                    window.Controls.Add(newButton);
                    double DistanceBetweenButtonsHorisontally = ButtonLength * 1.2;
                    newStartButtonX += (int)Math.Floor(DistanceBetweenButtonsHorisontally);
                    newButton.Visible = false;
                }
                newStartButtonX = StartButtonX;
                double DistanceBetweenButtonsVertically = ButtonHeight * 1.5;
                newStartButtonY += (int)Math.Floor(DistanceBetweenButtonsVertically);
            }
            ExtraButton = new Button();
            ExtraButton.Location = new Point(StartButtonX, StartButtonY + (6 * ButtonHeight));
            ExtraButton.Size = new Size(ButtonLength, ButtonHeight);
            window.Controls.Add(ExtraButton);
            ExtraButton.Visible = false;
            SetUpFirstButtons();
            TextBoxForInput.Location = new Point(StartButtonX, StartButtonY);
        }



        private void SetUpFirstButtons()
        {
            RemoveClickEvents();
            for (int i = 0; i < 3; i++)
            {
                Buttons[i].Visible = true;
            }
            for (int i = 3; i < Buttons.Count(); i++)
            {
                Buttons[i].Visible = false;
            }
            ExtraButton.Visible = false;
            Buttons[0].Text = "Call Address";
            Buttons[1].Text = "Call Group";
            Buttons[2].Text = "Broadcast";

            TextBoxForInput = InitializeTextBox();

            Buttons[0].Click += new EventHandler(CallAddress);
            Buttons[1].Click += new EventHandler(CallGroup);
            Buttons[2].Click += new EventHandler(CallBroadCast);
        }

        private NumericUpDown InitializeTextBox()
        {
            NumericUpDown CreatedTextBox = new NumericUpDown();
            window.Controls.Add(CreatedTextBox);
            CreatedTextBox.Visible = false;
            CreatedTextBox.Maximum = DALIController.AllLights.Count();
            return CreatedTextBox;
        }

        private void AddAllUnitsToBroadcast(object sender, EventArgs e)
        {
            DALIController.BroadcastOnAllUnits();
        }

        private void CallBroadCast(object sender, EventArgs e)
        {
            RemoveClickEvents();
            currentGroup = 16;
            CallOnBroadcastActions();
        }

        private void CallGroup(object sender, EventArgs e)
        {
            RemoveClickEvents();
            ShowGroups();
            foreach (var Button in Buttons)
            {
                Button.Click += new EventHandler(SaveGroup);
            }
        }
        private void SaveGroup(object sender, EventArgs e)
        {
            currentGroup = Buttons.IndexOf((Button)sender);
            CallOnGroupActions();
        }


        private void CallAddress(object sender, EventArgs e)
        {
            foreach (var item in Buttons)
            {
                item.Visible = false;
            }
            TextBoxForInput.KeyPress += new KeyPressEventHandler(TextBoxForInput_KeyPress);
            TextBoxForInput.Location = new Point(StartButtonX, StartButtonY + ButtonHeight);
            TextBoxForInput.Visible = true;
            TextBoxForInput.Enter += new EventHandler(TextBoxForInput_Entered);

            RemoveClickEvents();
        }

        private void TextBoxForInput_Entered(object sender, EventArgs e)
        {
            TextBoxForInput.ResetText();
        }

        private void TextBoxForInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && TextBoxForInput.Value > TextBoxForInput.Maximum-1)
            {
                TextBoxForInput.Value = 0;
            }
            else if (e.KeyChar == (char)Keys.Enter)
            {
                Console.WriteLine(TextBoxForInput.Maximum);
                Console.WriteLine(TextBoxForInput.Value);
                TextBoxForInput.Visible = false;
                currentAddress = Convert.ToInt32(TextBoxForInput.Value);

                CallOnAdressActions();
            }
        }

        private void ShowGroups()
        {
            RemoveClickEvents();
            for (int i = 0; i < Buttons.Count(); i++)
            {
                Buttons[i].Visible = true;
                Buttons[i].Text = "Group[" + i + "]"; 
            }
        }

        private void ShowScenes()
        {
            RemoveClickEvents();
            for (int i = 0; i < Buttons.Count(); i++)
            {
                Buttons[i].Visible = true;
                Buttons[i].Text = string.Format("Scene {0}: {1}%", i, DALIController.scenes[i]);
            }
        }

        private void DisplayGroupsForAddingOfAddress(object sender, EventArgs e)
        {
            ShowGroups();
            for(int i = 0; Buttons.Count() > i; i++)
            {
                Buttons[i].Click += new EventHandler(addAddressToGroup);
            }
        }

        private void DisplayGroupsForRemovalOfAddress(object sender, EventArgs e)
        {
            ShowGroups();
            ExtraButton.Visible = true;
            ExtraButton.Text = "All";
            ExtraButton.Click += new EventHandler(RemoveAddressFromAllGroups);
            for (int i = 0; Buttons.Count() > i; i++)
            {
                Buttons[i].Click += new EventHandler(RemoveAddressFromGroup);
            }
        }

        private void DisplayScenesToGoToForAddress(object sender, EventArgs e)
        {
            ShowScenes();
            for (int i = 0; Buttons.Count() > i; i++)
            {
                Buttons[i].Click += new EventHandler(AddressGoToScene);
            }
        }

        private void RemoveAddressFromAllGroups(object sender, EventArgs e)
        {
            LightingUnit CurrentUnit = DALIController.findUnitWithAddress(currentAddress);
            DALIController.RemoveUnitFromAllGroups(CurrentUnit);
            currentAddress = -1;
            SetUpFirstButtons();
        }

        private void addAddressToGroup(object sender, EventArgs e)
        {
            LightingUnit CurrentUnit = DALIController.findUnitWithAddress(currentAddress);
            DALIController.AddUnitToGroup(CurrentUnit, Buttons.IndexOf((Button)sender));
            currentAddress = -1;
            SetUpFirstButtons();

        }

        private void RemoveAddressFromGroup(object sender, EventArgs e)
        {
            LightingUnit CurrentUnit = DALIController.findUnitWithAddress(currentAddress);
            DALIController.RemoveUnitFromGroup(CurrentUnit, Buttons.IndexOf((Button)sender));
            currentAddress = -1;
            SetUpFirstButtons();
        }

        private void AddressGoToScene(object sender, EventArgs e)
        {
            LightingUnit CurrentUnit = DALIController.findUnitWithAddress(currentAddress);
            DALIController.AddressGoToScene(CurrentUnit, DALIController.scenes[Buttons.IndexOf((Button)sender)]);
            DALIController.AddUnitToGroup(CurrentUnit, 16);
            currentAddress = -1;
            SetUpFirstButtons();
        }

        private void ClearUnit(object sender, EventArgs e)
        {
            LightingUnit CurrentUnit = DALIController.findUnitWithAddress(currentAddress);
            DALIController.RemoveUnitFromAllGroups(CurrentUnit);
            currentAddress = -1;
            SetUpFirstButtons();
        }

        private void ClearGroup(object sender, EventArgs e)
        {
            DALIController.ClearGroup(currentGroup);
            SetUpFirstButtons();
        }

        private void ExtinguishUnit(object sender, EventArgs e)
        {
            LightingUnit CurrentUnit = DALIController.findUnitWithAddress(currentAddress);
            CurrentUnit.Extinguish();
            currentAddress = -1;
            SetUpFirstButtons();
        }

        private void ExtinguishGroup(object sender, EventArgs e)
        {
            DALIController.Extinguishgroup(currentGroup);
            SetUpFirstButtons();
        }

        private void TurnUnitOn(object sender, EventArgs e)
        {
            LightingUnit CurrentUnit = DALIController.findUnitWithAddress(currentAddress);
            CurrentUnit.TurnOn();
            currentAddress = -1;
            SetUpFirstButtons();
        }

        private void turnGroupOn(object sender, EventArgs e)
        {
            DALIController.TurnOnGroup(currentGroup);
            SetUpFirstButtons();
        }

        private void DisplayScenesForGroupToGoTo(object sender, EventArgs e)
        {
            ShowScenes();
            foreach (var Button in Buttons)
            {
                Button.Click += new EventHandler(GroupGoToScene);
            }
        }

        private void GroupGoToScene(object sender, EventArgs e)
        {
            DALIController.GroupGoToScene(currentGroup, DALIController.scenes[Buttons.IndexOf((Button)sender)]);
            currentGroup = -1;
            SetUpFirstButtons();
        }

        private void CallOnAdressActions()
        {
            Buttons[0].Visible = true;
            Buttons[0].Text = "Add to";
            Buttons[0].Click += new EventHandler(DisplayGroupsForAddingOfAddress);
            Buttons[1].Visible = true;
            Buttons[1].Text = "Remove from";
            Buttons[1].Click += new EventHandler(DisplayGroupsForRemovalOfAddress);
            Buttons[2].Visible = true;
            Buttons[2].Text = "Go to scene";
            Buttons[2].Click += new EventHandler(DisplayScenesToGoToForAddress);
            Buttons[3].Visible = true;
            Buttons[3].Text = "Extinguish";
            Buttons[3].Click += new EventHandler(ExtinguishUnit);
            Buttons[4].Visible = true;
            Buttons[4].Text = "Turn unit on";
            Buttons[4].Click += new EventHandler(TurnUnitOn);
            Buttons[5].Visible = true;
            Buttons[5].Text = "Clear unit";
            Buttons[5].Click += new EventHandler(ClearUnit);
        }

        private void CallOnGroupActions()
        {
            foreach (var item in Buttons)
            {
                item.Visible = false;
            }
            RemoveClickEvents();
            Buttons[0].Visible = true;
            Buttons[0].Text = "Go to scene";
            Buttons[0].Click += new EventHandler(DisplayScenesForGroupToGoTo);
            Buttons[1].Visible = true;
            Buttons[1].Text = "Extinguish";
            Buttons[1].Click += new EventHandler(ExtinguishGroup);
            Buttons[2].Visible = true;
            Buttons[2].Text = "Turn on";
            Buttons[2].Click += new EventHandler(turnGroupOn);
            Buttons[3].Visible = true;
            Buttons[3].Text = "Clear";
            Buttons[3].Click += new EventHandler(ClearGroup);
            //Buttons[4].Visible = true;
            //Buttons[4].Text = "Info";
            //Buttons[4].Click += new EventHandler(TurnUnitOn);

        }

        private void CallOnBroadcastActions()
        {
            foreach (var item in Buttons)
            {
                item.Visible = false;
            }
            RemoveClickEvents();
            Buttons[0].Visible = true;
            Buttons[0].Text = "Go to scene";
            Buttons[0].Click += new EventHandler(DisplayScenesForGroupToGoTo);
            Buttons[0].Click += new EventHandler(AddAllUnitsToBroadcast);
            Buttons[1].Visible = true;
            Buttons[1].Text = "Extinguish";
            Buttons[1].Click += new EventHandler(ExtinguishGroup);
            Buttons[1].Click += new EventHandler(AddAllUnitsToBroadcast);
            Buttons[2].Visible = true;
            Buttons[2].Text = "Turn on";
            Buttons[2].Click += new EventHandler(turnGroupOn);
            Buttons[2].Click += new EventHandler(AddAllUnitsToBroadcast);
            Buttons[3].Visible = true;
            Buttons[3].Text = "Clear broadcast";
            Buttons[3].Click += new EventHandler(ClearBroadcast);
            Buttons[4].Visible = true;
            Buttons[4].Text = "Clear all groups";
            Buttons[4].Click += new EventHandler(ClearAllGroups);
        }

        private void ClearAllGroups(object sender, EventArgs e)
        {
            DALIController.ClearAllGroups();
            SetUpFirstButtons();
        }

        private void ClearBroadcast(object sender, EventArgs e)
        {
            DALIController.ClearBroadcastGroup();
            SetUpFirstButtons();
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

            FieldInfo f2 = typeof(Control).GetField("EventClick",
                BindingFlags.Static | BindingFlags.NonPublic);
            object obj2 = f2.GetValue(ExtraButton);
            PropertyInfo pi2 = ExtraButton.GetType().GetProperty("Events",
                BindingFlags.NonPublic | BindingFlags.Instance);
            EventHandlerList list2 = (EventHandlerList)pi2.GetValue(ExtraButton, null);
            list2.RemoveHandler(obj2, list2[obj2]);

        }
    }
}
