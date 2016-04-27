using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triangulering;


namespace LightControl
{
    //used to Control and modify all LightUnits in the system
    public class DALIController
    {
        public List<LightingUnit> AllLights;
        public List<LightingUnit>[] _groups = new List<LightingUnit>[17];
        public List<LightingUnit> UntouchedLights = new List<LightingUnit>();
        public double[] scenes = new double[16] {0, 30, 35, 40, 45, 50, 55, 60, 65, 70, 75, 80, 85, 90, 95, 100};
        public DateTime TimeOfCreation;
        static double _totalWattUsage = 0;
        double _stepInterval = 0.01; //intervallet hvormed der bliver gået op i lysstyrke (skal evt ændres til at tilpasse sig)
        double _fadeRate = 0.001; //same but stepdown

        public DALIController(List<LightingUnit> AllLightsInSystem)
        {
            AllLights = AllLightsInSystem;
            TimeOfCreation = DateTime.Now;
        }        

        #region Calls on a single unit

        //Removes a unit from all groups it is a part of and resets forcedLightlevel.
        public void RemoveUnitFromAllGroups(LightingUnit UnitToRemove)
        {
            foreach (List<LightingUnit> group in _groups)
            {
                if (group.Contains(UnitToRemove))
                    group.Remove(UnitToRemove);
            }
            UnitToRemove.ForcedLightlevel = 0;
        }

        //Removes unit from a single group
        public void RemoveUnitFromGroup(LightingUnit UnitToRemove, int GroupToRemoveFrom)
        {
            _groups[GroupToRemoveFrom].Remove(UnitToRemove);
        }

        //adds a unit to group[16] (broadcast/ single unit control group) and sets its forced lightlevel to the scene
        public void AddressGoToScene(LightingUnit Unit, double scene)
        {
            Unit.ForcedLightlevel = scene / 100;
            AddUnitToGroup(Unit, 16);
        }
        //adds a unit to the wanted group if it does not already exist in that group
        public void AddUnitToGroup(LightingUnit UnitToAdd, int groupNumber)
        {
            bool AddUnit = true;

            if (_groups[groupNumber].Contains(UnitToAdd))
            {
                AddUnit = false;
            }

            if (AddUnit == true)
            {
                _groups[groupNumber].Add(UnitToAdd);
            }
        }
        #endregion

        #region Calls on a group

        //Extinguished all the units in a group
        public void Extinguishgroup(int groupnumber)
        {
            foreach (LightingUnit Unit in _groups[groupnumber])
            {
                Unit.Extinguish();
            }
        }

        //activates all units in a group (used for when they have been extinguished)
        public void TurnOnGroup(int groupnumber)
        {
            foreach (LightingUnit Unit in _groups[groupnumber])
            {
                Unit.TurnOn();
            }
        }

        //calls addressGoToScene for each unit in a group to set all given units to a specific scene
        public void GroupGoToScene(int groupNumber, double scene)
        {
            foreach (LightingUnit Unit in _groups[groupNumber])
            {
                AddressGoToScene(Unit, scene);
            }
        }

        //Removes all units from a group
        public void ClearGroup(int groupNumber)
        {
            _groups[groupNumber].Clear();
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
            foreach (List<LightingUnit> group in _groups)
            {
                group.Clear();
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

            IncrementGroupLights();
            IncrementUntouchedLights();

            ResetWantedLightLevels();
            UpdateUntouchedLights();
        }

        //increments lights in all groups based on their forced lightlevel
        private void IncrementGroupLights()
        {
            foreach (List<LightingUnit> group in _groups)
            {
                foreach (LightingUnit Unit in group)
                {

                    if (Unit.IsUnitOn == false)
                    {
                        Unit.LightingLevel = 0;
                    }

                    else if (Unit.LightingLevel < Unit.minLevel && Unit.ForcedLightlevel > 0)
                    {
                        Unit.LightingLevel = Unit.minLevel;
                    }

                    else if (Unit.LightingLevel > Unit.ForcedLightlevel)
                    {
                        Unit.LightingLevel = Unit.LightingLevel - _fadeRate;
                    }

                    else if (Unit.LightingLevel < Unit.ForcedLightlevel)
                    {
                        Unit.LightingLevel = Unit.LightingLevel + _stepInterval;
                    }

                    else
                    {
                        Unit.LightingLevel = Unit.ForcedLightlevel;
                    }
                }
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

                else if (Unit.LightingLevel < Unit.minLevel && Unit.wantedLightLevel > 0)
                {
                    Unit.LightingLevel = Unit.minLevel;
                }

                else if (Unit.LightingLevel > Unit.wantedLightLevel && _fadeRate < Unit.wantedLightLevel)
                {
                    Unit.LightingLevel = Unit.LightingLevel - _fadeRate;
                }

                else if (Unit.LightingLevel < Unit.wantedLightLevel)
                {
                    Unit.LightingLevel = Unit.LightingLevel + _stepInterval;
                }
                else
                {
                    Unit.LightingLevel = Unit.wantedLightLevel;
                }
            }
        }
        //creates a new UntouchedLights list based on what units are currently not in a group
        private void UpdateUntouchedLights()
        {
            IEnumerable<LightingUnit> q = AllLights;

            foreach (List<LightingUnit> Group in _groups)
            {
                q = q.Except(Group);
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
            for (int i = 0; i <= _groups.Length - 1; i++)
            {
                _groups[i] = new List<LightingUnit>();
            }
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
