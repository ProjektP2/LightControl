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
        List<Button> _buttons;
        List<CheckBox> CheckBoxes;
        Button BackButton;
        Button _FinishAddressChoice;
        List<int> _currentUnitIds;
        int NumberOfLightsPerRow;
        int _currentGroup;
        int _startButtonY;
        int _startButtonX;
        int _newstartButtonX;
        int _newStartButtonY;
        int _boundX;
        int _buttonHeight;
        int _buttonLength;
        InfoScreen InfoScreen;

        private PictureBox SimulationRoom;
        private List<LightingUnit> light;
        private Label l;


        public ControlPanel(Form form, DALIController Controller, List<LightingUnit> Lights, InfoScreen Info, PictureBox simulationRoom)
        {
            window = form;
            SimulationRoom = simulationRoom;
            light = Lights;
            DALIController = Controller;
            InitializeControlPanelButtons();
            _currentUnitIds = new List<int>();
            InfoScreen = Info;
            SimulationRoom.MouseClick += new MouseEventHandler(this.Form1_MouseClick);
            l = new Label();
            l.Show();
            l.Location = new Point(100, 100);
            window.Controls.Add(l);
        }


        #region Initialization

        //Creates all 16 buttons for future use as well as the three special buttons(EkstraButton, BackButton and _FinishAddressChoice)
        public void InitializeControlPanelButtons()
        {
            NumberOfLightsPerRow = FindNumberOfLightsPerRow();
            _boundX = (window.Width / 2 - GEngine.SimulationWidht / 2);
            _buttonHeight = window.Height / 33;
            _startButtonX = _boundX / 25;
            _startButtonY = window.Height / 7;
            _buttonLength = _boundX / 5 + 5;
            _newstartButtonX = _startButtonX;
            _newStartButtonY = _startButtonY;
            _buttons = new List<Button>();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Button _newButton = new Button();
                    _buttons.Add(_newButton);
                    _newButton.Location = new Point(_newstartButtonX, _newStartButtonY);
                    _newButton.Size = new Size(_buttonLength, _buttonHeight);
                    window.Controls.Add(_newButton);

                    double DistanceBetweenButtonsHorisontally = _buttonLength * 1.2 - 5;
                    _newstartButtonX += (int)Math.Floor(DistanceBetweenButtonsHorisontally);
                    _newButton.Visible = false;
                }
                _newstartButtonX = _startButtonX;
                double DistanceBetweenButtonsVertically = _buttonHeight * 1.5;
                _newStartButtonY += (int)Math.Floor(DistanceBetweenButtonsVertically);
            }

            _FinishAddressChoice = new Button();
            _FinishAddressChoice.Location = new Point(_buttons[2].Location.X, _buttons[2].Location.Y - _buttonHeight * 2);
            _FinishAddressChoice.Text = "Done";
            _FinishAddressChoice.Size = new Size(_buttonLength, _buttonHeight);
            window.Controls.Add(_FinishAddressChoice);
            _FinishAddressChoice.Click += new EventHandler(DoneChoosingAddresses);
            _FinishAddressChoice.Visible = false;

            BackButton = new Button();
            BackButton.Location = new Point(_buttons[3].Location.X, _buttons[3].Location.Y - _buttonHeight * 2);
            BackButton.Size = new Size(_buttonLength, _buttonHeight);
            BackButton.Text = "Back";
            BackButton.Click += new EventHandler(BackButtonPress);
            window.Controls.Add(BackButton);
            InitializeCheckBoxes();

            SetUpFirstButtons();

        }

        //initializes CheckBoxes based on the amount of LightUnits in the DALI Network
        private void InitializeCheckBoxes()
        {
            CheckBoxes = new List<CheckBox>();
            int currentIndex = 0;
            int _newCheckBoxX = 0;
            int _newCheckBoxY = _startButtonY;

            for (int i = 0; i < Math.Ceiling((Double)DALIController.AllLights.Count() / NumberOfLightsPerRow); i++)
            {
                for (int j = 0; j < NumberOfLightsPerRow && currentIndex <= DALIController.AllLights.Count() - 1; j++)
                {
                    CheckBox _newCheckBox = new CheckBox();
                    _newCheckBox.Width = _boundX / NumberOfLightsPerRow;
                    _newCheckBox.Font = new Font(_newCheckBox.Font.FontFamily, 4);
                    _newCheckBox.Text = currentIndex.ToString();
                    window.Controls.Add(_newCheckBox);
                    _newCheckBox.Location = new Point(_newCheckBoxX, _newCheckBoxY);
                    _newCheckBoxX += _newCheckBox.Width;
                    _newCheckBox.Visible = false;
                    CheckBoxes.Add(_newCheckBox);
                    currentIndex++;
                }
                _newCheckBoxX = 0;
                _newCheckBoxY += _buttonHeight;
            }
        }

        //used to find the number of Units in a row to more fluidly display the checkboxes
        //this will give the checkboxes a position simular to the on in the simulation
        private int FindNumberOfLightsPerRow()
        {
            int LightCount = 0;
            double firstUnitX = DALIController.AllLights[0].x;
            foreach (var LightingUnit in DALIController.AllLights)
            {
                if (LightingUnit.x == firstUnitX)
                {
                    LightCount++;
                }
            }
            return LightCount;
        }
        #endregion

        //setting up the main menu / resetting buttons to main menu
        #region menu items (resets/backbutton)

        //sets up the main menu buttons and
        //clears Click events and makes the first 3 buttons visible with the correct eventhandlers
        private void SetUpFirstButtons()
        {
            RemoveClickEvents();
            for (int i = 0; i < 3; i++)
            {
                _buttons[i].Visible = true;
            }
            for (int i = 3; i < _buttons.Count(); i++)
            {
                _buttons[i].Visible = false;
            }

            _buttons[0].Text = "Call Address";
            _buttons[1].Text = "Call Group";
            _buttons[2].Text = "Broadcast";

            _buttons[0].Click += new EventHandler(CallAddress);
            _buttons[1].Click += new EventHandler(CallGroup);
            _buttons[2].Click += new EventHandler(CallBroadCast);

            _FinishAddressChoice.Visible = false;
            BackButton.Visible = false;
        }

        //used for easy reset to main menu
        private void BackButtonPress(object sender, EventArgs e)
        {
            RemoveClickEvents();
            SetUpFirstButtons();
        }

        //displays group buttons without eventhandlers
        private void ShowGroups()
        {
            RemoveClickEvents();
            for (int i = 0; i < _buttons.Count(); i++)
            {
                _buttons[i].Visible = true;
                _buttons[i].Text = "Group[" + i + "]";
            }
        }

        //displays Scene buttons without eventhandlers
        private void ShowScenes()
        {
            RemoveClickEvents();
            for (int i = 0; i < _buttons.Count(); i++)
            {
                _buttons[i].Visible = true;
                _buttons[i].Text = string.Format("Scene {0}: {1}%", i, DALIController.scenes[i]);
            }
        }

        //removes the checkmarks from the checkboxes and makes them invissible
        private void clearCheckBoxes()
        {
            foreach (CheckBox Checkbox in CheckBoxes)
            {
                Checkbox.Checked = false;
                Checkbox.Visible = false;
            }
        }

        //clears click event from all of the 16 main buttons
        //used to stop buttons from keeping old eventhandlers
        private void RemoveClickEvents()
        {
            foreach (Button Button in _buttons)
            {
                FieldInfo f1 = typeof(Control).GetField("EventClick",
                BindingFlags.Static | BindingFlags.NonPublic);
                object obj = f1.GetValue(Button);
                PropertyInfo pi = Button.GetType().GetProperty("Events",
                    BindingFlags.NonPublic | BindingFlags.Instance);
                EventHandlerList list = (EventHandlerList)pi.GetValue(Button, null);
                list.RemoveHandler(obj, list[obj]);
            }
        }

        //event for mouseclick on lighting units to display information about the wanted unit
        //also functions as a second method of checking the checkboxes when choosing addresses
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            l.Location = new Point(0, _buttons[2].Location.Y - _buttonHeight * 2);
            l.AutoSize = true;
            Point LocalMousePosition = SimulationRoom.PointToClient(Cursor.Position);
            foreach (var VARIABLE in light)
            {
                if (VARIABLE.x <= LocalMousePosition.X + 10 && VARIABLE.x >= LocalMousePosition.X - 10
                    && VARIABLE.y <= LocalMousePosition.Y + 10 && VARIABLE.y >= LocalMousePosition.Y - 10)
                {
                    l.Text = "Current Ligting Unit: " + VARIABLE.Address;
                    LightingUnit Current = DALIController.FindUnitWithAddress(VARIABLE.Address);
                    InfoScreen.ChangeCurrentLightingUnit(Current);
                    if (CheckBoxes[VARIABLE.Address].Checked == false)
                    {
                        CheckBoxes[VARIABLE.Address].Checked = true;
                    }
                    else if (CheckBoxes[VARIABLE.Address].Checked == true)
                    {
                        CheckBoxes[VARIABLE.Address].Checked = false;
                    }
                }
            }
        }
        #endregion

        //Button events in the main menu
        #region Main menu events       

        //Shows group Buttons and adds the eventHandler SaveGroup to each button
        //used when user want to call an entire group
        private void CallGroup(object sender, EventArgs e)
        {
            RemoveClickEvents();
            ShowGroups();
            foreach (Button Button in _buttons)
            {
                Button.Click += new EventHandler(SaveGroup);
            }
            BackButton.Visible = true;
        }

        //shows Actions to do on broadcast and sets current group to 16 (broadcast/single unit control group)
        //used when user wants to call all units in the DALI Network
        private void CallBroadCast(object sender, EventArgs e)
        {
            RemoveClickEvents();
            _currentGroup = 16;
            CallOnBroadcastActions();
            BackButton.Visible = true;
        }

        #endregion


        #region Call address items and events

            #region Choosing the addresses and displaying action
        //Makes the buttons in _buttons invisible and the Checkboxes visible as well as the finishAddressChoise button and BackButton
        //Used when user wants to call on single units in the DALI network
        private void CallAddress(object sender, EventArgs e)
        {
            foreach (Button Button in _buttons)
            {
                Button.Visible = false;
            }
            RemoveClickEvents();

            foreach (var item in CheckBoxes)
            {
                item.Checked = false;
                item.Visible = true;
            }
            _FinishAddressChoice.Visible = true;
            BackButton.Visible = true;
        }

        //adds the address of each checked Checkedbox to a list used for calling on the wanted LightingUnits
        //removes the checkBoxes and displays action that can be made on single units
        private void DoneChoosingAddresses(object sender, EventArgs e)
        {
            bool IsnotEmpty = false;
            foreach (CheckBox checkBox in CheckBoxes.Where(c => c.Checked))
            {
                int index = Int32.Parse(checkBox.Text);
                _currentUnitIds.Add(index);
                IsnotEmpty = true;
            }

            if (IsnotEmpty == true)
            {
                clearCheckBoxes();
                _FinishAddressChoice.Visible = false;
                CallOnAdressActions();
            }
        }

        //displays the different actions that can be done on single units and add fitting eventhandlers
        //called in DoneChoosingAddresses()
        private void CallOnAdressActions()
        {
            _buttons[0].Visible = true;
            _buttons[0].Text = "Add to";
            _buttons[0].Click += new EventHandler(DisplayGroupsForAddingOfAddress);
            _buttons[1].Visible = true;
            _buttons[1].Text = "Remove from";
            _buttons[1].Click += new EventHandler(DisplayGroupsForRemovalOfAddress);
            _buttons[2].Visible = true;
            _buttons[2].Text = "Go to scene";
            _buttons[2].Click += new EventHandler(DisplayScenesToGoToForAddress);
            _buttons[3].Visible = true;
            _buttons[3].Text = "Extinguish";
            _buttons[3].Click += new EventHandler(ExtinguishUnit);
            _buttons[4].Visible = true;
            _buttons[4].Text = "Turn unit on";
            _buttons[4].Click += new EventHandler(TurnUnitOn);
            _buttons[5].Visible = true;
            _buttons[5].Text = "Clear unit";
            _buttons[5].Click += new EventHandler(RemoveAddressFromAllGroups);
        }
        #endregion

            #region adding address to group
        //Shows group buttons and add eventhandlers for adding the units to a given group
        private void DisplayGroupsForAddingOfAddress(object sender, EventArgs e)
        {
            ShowGroups();
            for (int i = 0; _buttons.Count() > i; i++)
            {
                _buttons[i].Click += new EventHandler(AddAddressToGroup);
            }
        }

        //adds every Unit with the addresses in the _currentUnitIds List to the wanted group and resets the list and goes to main menu
        private void AddAddressToGroup(object sender, EventArgs e)
        {
            foreach (int index in _currentUnitIds)
            {
                LightingUnit CurrentUnit = DALIController.FindUnitWithAddress(index);
                DALIController.AddUnitToGroup(CurrentUnit, _buttons.IndexOf((Button)sender));
            }
            _currentUnitIds.Clear();
            SetUpFirstButtons();
        }
        #endregion

            #region removing address from group(s)
        //Displays group buttons and adds the remove eventhandler
        private void DisplayGroupsForRemovalOfAddress(object sender, EventArgs e)
        {
            ShowGroups();
            for (int i = 0; _buttons.Count() > i; i++)
            {
                _buttons[i].Click += new EventHandler(RemoveAddressFromGroup);
            }
        }

        //removes every Unit with the addresses in the _currentUnitIds List from the wanted group
        //clears _currentUnitIds and goes to main menu
        private void RemoveAddressFromGroup(object sender, EventArgs e)
        {
            foreach (int index in _currentUnitIds)
            {
                LightingUnit CurrentUnit = DALIController.FindUnitWithAddress(index);
                DALIController.RemoveUnitFromGroup(CurrentUnit, _buttons.IndexOf((Button)sender));
            }
            _currentUnitIds.Clear();
            SetUpFirstButtons();
        }
        #endregion

            #region Setting address to scene
        //Displays scenes to set the wanted units to
        private void DisplayScenesToGoToForAddress(object sender, EventArgs e)
        {
            ShowScenes();
            for (int i = 0; _buttons.Count() > i; i++)
            {
                _buttons[i].Click += new EventHandler(AddressGoToScene);
            }
        }

        //sets every Units forced light level with the addresses in the _currentUnitIds List to the wanted scene
        private void AddressGoToScene(object sender, EventArgs e)
        {
            foreach (int index in _currentUnitIds)
            {
                LightingUnit CurrentUnit = DALIController.FindUnitWithAddress(index);
                DALIController.AddressGoToScene(CurrentUnit, DALIController.scenes[_buttons.IndexOf((Button)sender)]);
            }
            _currentUnitIds.Clear();
            SetUpFirstButtons();
        }

        #endregion

            #region Extinguishing and turn on Units
        //sets every Unit with the addresses in the _currentUnitIds List to be turned off not allowing them to have a light level
        //then resets to main menu 
        private void ExtinguishUnit(object sender, EventArgs e)
        {
            foreach (int index in _currentUnitIds)
            {
                LightingUnit CurrentUnit = DALIController.FindUnitWithAddress(index);
                CurrentUnit.Extinguish();
            }
            _currentUnitIds.Clear();
            SetUpFirstButtons();
        }

        //resets the Units that were extinguised to now be able to work again
        //then goes to main menu
        private void TurnUnitOn(object sender, EventArgs e)
        {
            foreach (int index in _currentUnitIds)
            {
                LightingUnit CurrentUnit = DALIController.FindUnitWithAddress(index);
                CurrentUnit.TurnOn();
            }
            _currentUnitIds.Clear();
            SetUpFirstButtons();
        }
        #endregion

            #region Clearing units
        //same as RemoveAddressFromGroup but instead calls for the DALI Controller to remove the unit from all groups
        //Then resets to Main Menu
        private void RemoveAddressFromAllGroups(object sender, EventArgs e)
        {
            foreach (int index in _currentUnitIds)
            {
                LightingUnit CurrentUnit = DALIController.FindUnitWithAddress(index);
                DALIController.RemoveUnitFromAllGroups(CurrentUnit);
            }
            _currentUnitIds.Clear();
            SetUpFirstButtons();
        }
        #endregion
        #endregion

        #region Call Group items and event
        //saves the index of the wanted group for future use
        private void SaveGroup(object sender, EventArgs e)
        {
            _currentGroup = _buttons.IndexOf((Button)sender);
            CallOnGroupActions();
        }

        //displays Actions that can be called on groups
        private void CallOnGroupActions()
        {
            foreach (Button Button in _buttons)
            {
                Button.Visible = false;
            }
            RemoveClickEvents();
            _buttons[0].Visible = true;
            _buttons[0].Text = "Go to scene";
            _buttons[0].Click += new EventHandler(DisplayScenesForGroupToGoTo);
            _buttons[1].Visible = true;
            _buttons[1].Text = "Extinguish";
            _buttons[1].Click += new EventHandler(ExtinguishGroup);
            _buttons[2].Visible = true;
            _buttons[2].Text = "Turn on";
            _buttons[2].Click += new EventHandler(TurnGroupOn);
            _buttons[3].Visible = true;
            _buttons[3].Text = "Clear";
            _buttons[3].Click += new EventHandler(ClearGroup);         
        }

        //displays scenes and adds an Eventhandler to go to the wanted scene
        //if the call is a broadcast add all units to group 16
        private void DisplayScenesForGroupToGoTo(object sender, EventArgs e)
        {
            ShowScenes();
            foreach (Button Button in _buttons)
            {
                Button.Click += new EventHandler(GroupGoToScene);
                if (_currentGroup == 16)
                {
                    Button.Click += new EventHandler(AddAllUnitsToBroadcast);
                }
            }
        }

        //sets the forced lightlevel of the current group to the wanted scene
        //then resets to main menu
        private void GroupGoToScene(object sender, EventArgs e)
        {
            DALIController.GroupGoToScene(_currentGroup, DALIController.scenes[_buttons.IndexOf((Button)sender)]);
            _currentGroup = -1;
            SetUpFirstButtons();
        }

        //Extinguishes all Units in the wanted group
        //then resets to main menu
        private void ExtinguishGroup(object sender, EventArgs e)
        {
            DALIController.Extinguishgroup(_currentGroup);
            SetUpFirstButtons();
        }

        //Resets the Units in the given group to allow for them to turn on again
        //then resets to main menu
        private void TurnGroupOn(object sender, EventArgs e)
        {
            DALIController.TurnOnGroup(_currentGroup);
            SetUpFirstButtons();
        }

        //clears the entire group for all units
        //then resets to main menu
        private void ClearGroup(object sender, EventArgs e)
        {
            DALIController.ClearGroup(_currentGroup);
            SetUpFirstButtons();
        }

        #endregion

        #region Broadcast calls
        
        //displays Actions that can be done on all units at the same time
        //"Go to scene", "Extinguish" and "Turn on" are the same as for group calls but with the added eventhandler
        //this handler adds all units to group 16 for the broadcast
        private void CallOnBroadcastActions()
        {
            foreach (Button Button in _buttons)
            {
                Button.Visible = false;
            }
            RemoveClickEvents();
            _buttons[0].Visible = true;
            _buttons[0].Text = "Go to scene";
            _buttons[0].Click += new EventHandler(DisplayScenesForGroupToGoTo);
            _buttons[1].Visible = true;
            _buttons[1].Text = "Extinguish";
            _buttons[1].Click += new EventHandler(ExtinguishGroup);
            _buttons[1].Click += new EventHandler(AddAllUnitsToBroadcast);
            _buttons[2].Visible = true;
            _buttons[2].Text = "Turn on";
            _buttons[2].Click += new EventHandler(TurnGroupOn);
            _buttons[2].Click += new EventHandler(AddAllUnitsToBroadcast);
            _buttons[3].Visible = true;
            _buttons[3].Text = "Clear broadcast";
            _buttons[3].Click += new EventHandler(ClearBroadcast);
            _buttons[4].Visible = true;
            _buttons[4].Text = "Clear all groups";
            _buttons[4].Click += new EventHandler(ClearAllGroups);
        }

        //adds all units to the broadcast group (16)
        private void AddAllUnitsToBroadcast(object sender, EventArgs e)
        {
            DALIController.BroadcastOnAllUnits();
        }

        //only clears the broadcast group (16)
        private void ClearBroadcast(object sender, EventArgs e)
        {
            DALIController.ClearBroadcastGroup();
            SetUpFirstButtons();
        }

        //clear ALL groups including the broadcast group(16)
        private void ClearAllGroups(object sender, EventArgs e)
        {
            DALIController.ClearAllGroups();
            SetUpFirstButtons();
        }


        #endregion

    }
}
