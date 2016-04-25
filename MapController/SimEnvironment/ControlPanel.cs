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
        LightUnitsCoords LightCoords;
        List<Button> _buttons;
        List<CheckBox> CheckBoxes;
        Button ExtraButton;
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


        public ControlPanel(Form form, DALIController Controller, List<LightingUnit> Lights)
        {
            window = form;
            DALIController = Controller;
            InitializeControlPanelButtons();
            _currentUnitIds = new List<int>();
        }

        public void InitializeControlPanelButtons()
        {
            NumberOfLightsPerRow = FindNumberOfLightsPerRow();
            _boundX = (window.Width/2-GEngine.SimulationWidht/2);
            _buttonHeight = window.Height / 33;
            _startButtonX = _boundX / 25;
            _startButtonY = window.Height / 7;
            _buttonLength = _boundX/ 5 + 5;
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
            ExtraButton = new Button();
            ExtraButton.Location = new Point(_startButtonX, _startButtonY + (6 * _buttonHeight));
            ExtraButton.Size = new Size(_buttonLength, _buttonHeight);
            window.Controls.Add(ExtraButton);
            ExtraButton.Visible = false;

            _FinishAddressChoice = new Button();
            _FinishAddressChoice.Location = new Point(_buttons[2].Location.X, _buttons[2].Location.Y - _buttonHeight * 2);
            _FinishAddressChoice.Text = "Done";
            _FinishAddressChoice.Size = new Size(_buttonLength, _buttonHeight);
            window.Controls.Add(_FinishAddressChoice);
            _FinishAddressChoice.Click += new EventHandler(DoneChoosingAddresses);
            _FinishAddressChoice.Visible = false;

            BackButton = new Button();
            BackButton.Location = new Point(_buttons[3].Location.X, _buttons[3].Location.Y - _buttonHeight* 2);
            BackButton.Size = new Size(_buttonLength, _buttonHeight);
            BackButton.Text = "Back";
            BackButton.Click += new EventHandler(BackButtonPress);
            window.Controls.Add(BackButton);
            InitializeCheckBoxes();

            SetUpFirstButtons();

        }



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
            ExtraButton.Visible = false;
            _buttons[0].Text = "Call Address";
            _buttons[1].Text = "Call Group";
            _buttons[2].Text = "Broadcast";

            _buttons[0].Click += new EventHandler(CallAddress);
            _buttons[1].Click += new EventHandler(CallGroup);
            _buttons[2].Click += new EventHandler(CallBroadCast);


            foreach (var item in CheckBoxes)
            {
                item.Visible = false;
            }
            _FinishAddressChoice.Visible = false;
            BackButton.Visible = false;
        }

        private void InitializeCheckBoxes()
        {
            CheckBoxes = new List<CheckBox>();
            int currentIndex = 0;
            int _newCheckBoxX = 0;
            int _newCheckBoxY = _startButtonY;

            for (int i = 0; i < Math.Ceiling((Double)DALIController.AllLights.Count()/NumberOfLightsPerRow); i++)
            {
                for (int j = 0; j < NumberOfLightsPerRow && currentIndex <= DALIController.AllLights.Count()-1; j++)
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

        private void DoneChoosingAddresses(object sender, EventArgs e)
        {
            foreach (CheckBox checkBox in window.Controls.OfType<CheckBox>().Where(c => c.Checked))
            {
                int index = Int32.Parse(checkBox.Text);
                _currentUnitIds.Add(index);
            }

            foreach (var item in CheckBoxes)
            {
                item.Visible = false;
            }
            _FinishAddressChoice.Visible = false;
            CallOnAdressActions();
        }

        private void AddAllUnitsToBroadcast(object sender, EventArgs e)
        {
            DALIController.BroadcastOnAllUnits();
        }

        private void BackButtonPress(object sender, EventArgs e)
        {
            foreach (var item in CheckBoxes)
            {
                item.Visible = false;
            }
            RemoveClickEvents();
            SetUpFirstButtons();
        }

        private void CallBroadCast(object sender, EventArgs e)
        {
            RemoveClickEvents();
            _currentGroup = 16;
            CallOnBroadcastActions();
            BackButton.Visible = true;
        }

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
        private void SaveGroup(object sender, EventArgs e)
        {
            _currentGroup = _buttons.IndexOf((Button)sender);
            CallOnGroupActions();
        }


        private void CallAddress(object sender, EventArgs e)
        {
            foreach (Button Button in _buttons)
            {
                Button.Visible = false;
            }
            RemoveClickEvents();

            foreach (var item in CheckBoxes)
            {
                item.Visible = true;
            }
            _FinishAddressChoice.Visible = true;
            BackButton.Visible = true;
        }

        private void ShowGroups()
        {
            RemoveClickEvents();
            for (int i = 0; i < _buttons.Count(); i++)
            {
                _buttons[i].Visible = true;
                _buttons[i].Text = "Group[" + i + "]"; 
            }
        }

        private void ShowScenes()
        {
            RemoveClickEvents();
            for (int i = 0; i < _buttons.Count(); i++)
            {
                _buttons[i].Visible = true;
                _buttons[i].Text = string.Format("Scene {0}: {1}%", i, DALIController.scenes[i]);
            }
        }

        private void DisplayGroupsForAddingOfAddress(object sender, EventArgs e)
        {
            ShowGroups();
            for(int i = 0; _buttons.Count() > i; i++)
            {
                _buttons[i].Click += new EventHandler(AddAddressToGroup);
            }
        }

        private void DisplayGroupsForRemovalOfAddress(object sender, EventArgs e)
        {
            ShowGroups();
            ExtraButton.Visible = true;
            ExtraButton.Text = "All";
            ExtraButton.Click += new EventHandler(RemoveAddressFromAllGroups);
            for (int i = 0; _buttons.Count() > i; i++)
            {
                _buttons[i].Click += new EventHandler(RemoveAddressFromGroup);
            }
        }

        private void DisplayScenesToGoToForAddress(object sender, EventArgs e)
        {
            ShowScenes();
            for (int i = 0; _buttons.Count() > i; i++)
            {
                _buttons[i].Click += new EventHandler(AddressGoToScene);
            }
        }

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

        private void AddressGoToScene(object sender, EventArgs e)
        {
            foreach (int index in _currentUnitIds)
            {
                LightingUnit CurrentUnit = DALIController.FindUnitWithAddress(index);
                DALIController.AddressGoToScene(CurrentUnit, DALIController.scenes[_buttons.IndexOf((Button)sender)]);
                DALIController.AddUnitToGroup(CurrentUnit, 16);
            }
            _currentUnitIds.Clear();
            SetUpFirstButtons();
        }


        private void ClearGroup(object sender, EventArgs e)
        {
            DALIController.ClearGroup(_currentGroup);
            SetUpFirstButtons();
        }

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

        private void ExtinguishGroup(object sender, EventArgs e)
        {
            DALIController.Extinguishgroup(_currentGroup);
            SetUpFirstButtons();
        }

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

        private void TurnGroupOn(object sender, EventArgs e)
        {
            DALIController.TurnOnGroup(_currentGroup);
            SetUpFirstButtons();
        }

        private void DisplayScenesForGroupToGoTo(object sender, EventArgs e)
        {
            ShowScenes();
            foreach (Button Button in _buttons)
            {
                Button.Click += new EventHandler(AddAllUnitsToBroadcast);
                Button.Click += new EventHandler(GroupGoToScene);
            }
        }

        private void GroupGoToScene(object sender, EventArgs e)
        {
            DALIController.GroupGoToScene(_currentGroup, DALIController.scenes[_buttons.IndexOf((Button)sender)]);
            _currentGroup = -1;
            SetUpFirstButtons();
        }

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
            //_buttons[4].Visible = true;
            //_buttons[4].Text = "Info";
            //_buttons[4].Click += new EventHandler(TurnUnitOn);

        }

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

            FieldInfo f2 = typeof(Control).GetField("EventClick",
                BindingFlags.Static | BindingFlags.NonPublic);
            object obj2 = f2.GetValue(ExtraButton);
            PropertyInfo pi2 = ExtraButton.GetType().GetProperty("Events",
                BindingFlags.NonPublic | BindingFlags.Instance);
            EventHandlerList list2 = (EventHandlerList)pi2.GetValue(ExtraButton, null);
            list2.RemoveHandler(obj2, list2[obj2]);

        }


        private int FindNumberOfLightsPerRow()
        {
            int LightCount = 0;
            double firstUnitX = DALIController.AllLights[0].x;
            foreach (var LightingUnit in DALIController.AllLights)
            {
                if(LightingUnit.x == firstUnitX)
                {
                    LightCount++;
                }
            }
            return LightCount;
        }
    }
}
