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
        NumericUpDown _textBoxForInput;
        Button ExtraButton;
        int _currentAddress;
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
        }

        public void InitializeControlPanelButtons()
        {
            _boundX = (window.Width/2-GEngine.SimulationWidht/2);
            _buttonHeight = window.Height / 33;
            _startButtonX = _boundX / 25;
            _startButtonY = window.Height / 7;
            _buttonLength = _boundX/ 5;
            _newstartButtonX = _startButtonX;
            _newStartButtonY = _startButtonY;
            _buttons = new List<Button>();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Button _newButton = new Button();
                    _buttons.Add(_newButton);
                    _newButton.Location = new System.Drawing.Point(_newstartButtonX, _newStartButtonY);
                    _newButton.Size = new System.Drawing.Size(_buttonLength, _buttonHeight);
                    window.Controls.Add(_newButton);
                    double DistanceBetweenButtonsHorisontally = _buttonLength * 1.2;
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
            SetUpFirstButtons();
            _textBoxForInput.Location = new Point(_startButtonX, _startButtonY);
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

            _textBoxForInput = InitializeTextBox();

            _buttons[0].Click += new EventHandler(CallAddress);
            _buttons[1].Click += new EventHandler(CallGroup);
            _buttons[2].Click += new EventHandler(CallBroadCast);
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
            _currentGroup = 16;
            CallOnBroadcastActions();
        }

        private void CallGroup(object sender, EventArgs e)
        {
            RemoveClickEvents();
            ShowGroups();
            foreach (Button Button in _buttons)
            {
                Button.Click += new EventHandler(SaveGroup);
            }
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
            _textBoxForInput.KeyPress += new KeyPressEventHandler(TextBoxForInput_KeyPress);
            _textBoxForInput.Location = new Point(_startButtonX, _startButtonY + _buttonHeight);
            _textBoxForInput.Visible = true;
            _textBoxForInput.Enter += new EventHandler(TextBoxForInput_Entered);

            RemoveClickEvents();
        }

        private void TextBoxForInput_Entered(object sender, EventArgs e)
        {
            _textBoxForInput.ResetText();
        }

        private void TextBoxForInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && _textBoxForInput.Value > _textBoxForInput.Maximum-1)
            {
                _textBoxForInput.Value = 0;
            }
            else if (e.KeyChar == (char)Keys.Enter)
            {
                Console.WriteLine(_textBoxForInput.Maximum);
                Console.WriteLine(_textBoxForInput.Value);
                _textBoxForInput.Visible = false;
                _currentAddress = Convert.ToInt32(_textBoxForInput.Value);

                CallOnAdressActions();
            }
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
            LightingUnit CurrentUnit = DALIController.FindUnitWithAddress(_currentAddress);
            DALIController.RemoveUnitFromAllGroups(CurrentUnit);
            _currentAddress = -1;
            SetUpFirstButtons();
        }

        private void AddAddressToGroup(object sender, EventArgs e)
        {
            LightingUnit CurrentUnit = DALIController.FindUnitWithAddress(_currentAddress);
            DALIController.AddUnitToGroup(CurrentUnit, _buttons.IndexOf((Button)sender));
            _currentAddress = -1;
            SetUpFirstButtons();

        }

        private void RemoveAddressFromGroup(object sender, EventArgs e)
        {
            LightingUnit CurrentUnit = DALIController.FindUnitWithAddress(_currentAddress);
            DALIController.RemoveUnitFromGroup(CurrentUnit, _buttons.IndexOf((Button)sender));
            _currentAddress = -1;
            SetUpFirstButtons();
        }

        private void AddressGoToScene(object sender, EventArgs e)
        {
            LightingUnit CurrentUnit = DALIController.FindUnitWithAddress(_currentAddress);
            DALIController.AddressGoToScene(CurrentUnit, DALIController.scenes[_buttons.IndexOf((Button)sender)]);
            DALIController.AddUnitToGroup(CurrentUnit, 16);
            _currentAddress = -1;
            SetUpFirstButtons();
        }

        private void ClearUnit(object sender, EventArgs e)
        {
            LightingUnit CurrentUnit = DALIController.FindUnitWithAddress(_currentAddress);
            DALIController.RemoveUnitFromAllGroups(CurrentUnit);
            _currentAddress = -1;
            SetUpFirstButtons();
        }

        private void ClearGroup(object sender, EventArgs e)
        {
            DALIController.ClearGroup(_currentGroup);
            SetUpFirstButtons();
        }

        private void ExtinguishUnit(object sender, EventArgs e)
        {
            LightingUnit CurrentUnit = DALIController.FindUnitWithAddress(_currentAddress);
            CurrentUnit.Extinguish();
            _currentAddress = -1;
            SetUpFirstButtons();
        }

        private void ExtinguishGroup(object sender, EventArgs e)
        {
            DALIController.Extinguishgroup(_currentGroup);
            SetUpFirstButtons();
        }

        private void TurnUnitOn(object sender, EventArgs e)
        {
            LightingUnit CurrentUnit = DALIController.FindUnitWithAddress(_currentAddress);
            CurrentUnit.TurnOn();
            _currentAddress = -1;
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
            _buttons[5].Click += new EventHandler(ClearUnit);
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
            _buttons[0].Click += new EventHandler(AddAllUnitsToBroadcast);
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
    }
}
