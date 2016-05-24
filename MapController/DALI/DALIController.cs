using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triangulering;
using MapController.Triangulate;



namespace LightControl
{
    //used to Control and modify all LightUnits in the system
    public class DALIController
    {
        public List<LightingUnit> AllLights;
        public List<DALIGroup> _groups = new List<DALIGroup>();
        public List<LightingUnit> UntouchedLights = new List<LightingUnit>();
        public double[] scenes = new double[16] {0, 30, 35, 40, 45, 50, 55, 60, 65, 70, 75, 80, 85, 90, 95, 100};
        public DateTime TimeOfCreation;
        static double _totalWattUsage = 0;
        double _stepInterval = 0.01; //intervallet hvormed der bliver gået op i lysstyrke (skal evt ændres til at tilpasse sig)
        double _fadeRate = 0.005; //same but stepdown

        public DALIController(List<LightingUnit> AllLightsInSystem)
        {
            AllLights = AllLightsInSystem;
            TimeOfCreation = DateTime.Now;
            for(int i = 0; i < 17; i++)
            {
                _groups.Add(new DALIGroup());
            }
        }        

        #region Calls on a single unit

        //Removes a unit from all groups it is a part of and resets forcedLightlevel.
        public void RemoveUnitFromAllGroups(LightingUnit UnitToRemove)
        {
            foreach (DALIGroup group in _groups)
            {
                if (group.GroupOfLights.Contains(UnitToRemove))
                    group.RemoveUnit(UnitToRemove);
            }
            UnitToRemove.ForcedLightlevel = 0;
        }

        //Removes unit from a single group
        public void RemoveUnitFromGroup(LightingUnit UnitToRemove, int GroupToRemoveFrom)
        {
            _groups[GroupToRemoveFrom].RemoveUnit(UnitToRemove);
        }

        //adds a unit to group[16] (broadcast/ single unit control group) and sets its forced lightlevel to the scene
        public void AddressGoToScene(LightingUnit Unit, double scene)
        {
            Unit.ForcedLightlevel = scene / 100;
            AddUnitToGroup(Unit, 16);
            _groups[16].isGroupUsed = true;
        }
        //adds a unit to the wanted group if it does not already exist in that group
        public void AddUnitToGroup(LightingUnit UnitToAdd, int groupNumber)
        {
            _groups[groupNumber].AddUnitToGroup(UnitToAdd);
        }
        #endregion

        #region Calls on a group

        //Extinguished all the units in a group
        public void Extinguishgroup(int groupnumber)
        {
            _groups[groupnumber].ExtinguishGroup();
        }

        //activates all units in a group (used for when they have been extinguished)
        public void TurnOnGroup(int groupnumber)
        {
            _groups[groupnumber].TurnOnGroup();
        }

        //calls addressGoToScene for each unit in a group to set all given units to a specific scene
        public void GroupGoToScene(int groupNumber, double scene)
        {
            scene = scene / 100;
            _groups[groupNumber].GroupGoToScene(scene);
        }

        //Removes all units from a group
        public void ClearGroup(int groupNumber)
        {
            _groups[groupNumber].ClearGroup();
        }

        #endregion

        #region Broadcast calls (calls on all units)

        // adds all units to group[16] (the broadcast/single control group)
        public void BroadcastOnAllUnits()
        {
            foreach (LightingUnit Unit in AllLights)
            {
                AddUnitToGroup(Unit, 16);
            }
        }

        //clears only group[16] (the broadcast/single control group)
        public void ClearBroadcastGroup()
        {
            ClearGroup(16);
        }

        //clears ALL groups for units (including the broadcast group)
        public void ClearAllGroups()
        {
            foreach (DALIGroup group in _groups)
            {
                group.ClearGroup();
            }
        }

        #endregion
       
        #region Incrementation of lights
        //main function to increase/decrease the lightlevels of units in the DALI Network
        //Calculated the watt usage since last run and then increments lights
        //in the end the wanted lightlevels are reset and the untouched lights list is updated
        public void IncrementAllLights()
        {
            foreach (LightingUnit Unit in AllLights)
            {
                _totalWattUsage += Unit.getWattUsageForLightUnitInHours();
            }

            UpdateUntouchedLights();
            IncrementGroupLights();
            IncrementUntouchedLights();
            ResetWantedLightLevels();
        }

        //increments lights in all groups based on their forced lightlevel
        private void IncrementGroupLights()
        {
            foreach (DALIGroup group in _groups)
            {
                group.Increment(_fadeRate, _stepInterval);
            }
        }
        //increments lights in untouchedLights based on the wanted lightlevel
        private void IncrementUntouchedLights()
        {

            foreach (LightingUnit Unit in UntouchedLights)
            {
                if (Unit.IsUnitOn == false)
                {
                    Unit.LightingLevel = 0;
                }
                else if (Unit.LightingLevel > Unit.wantedLightLevel)
                {
                    Unit.LightingLevel = Unit.LightingLevel - _fadeRate;
                }

                else if (Unit.LightingLevel < Unit.wantedLightLevel && Unit.wantedLightLevel > 0.2)
                {
                    Unit.LightingLevel = Unit.LightingLevel + _stepInterval;
                }
            }
        }
        //creates a new UntouchedLights list based on what units are currently not in a group
        private void UpdateUntouchedLights()
        {
            IEnumerable<LightingUnit> q = AllLights;

            foreach (DALIGroup Group in _groups)
            {
                q = q.Except(Group.ReturnListIfGroupIsUsed());
            }
            UntouchedLights = q.ToList();
        }

        //resets all wanted lightlevels
        private void ResetWantedLightLevels()
        {
            foreach (LightingUnit Light in AllLights)
            {
                Light.wantedLightLevel = 0;
            }
        }
        #endregion

        //initializes groups
        public void InitGroups()
        {
            UntouchedLights.AddRange(AllLights);
        }

        //finds a given unit in the AllLights list based on the address
        public LightingUnit FindUnitWithAddress(int AddressToFind)
        {
            int index = AllLights.FindIndex(a => a.Address == AddressToFind);
            Console.WriteLine(index);
            return AllLights[index];
        }

        //getter for the wattusage
        public double GetTotalWattusage()
        {
            return _totalWattUsage;
        }
    }
}
