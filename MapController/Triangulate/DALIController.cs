using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triangulering;


namespace LightControl
{
    public class DALIController
    {
        public List<LightingUnit> AllLights;
        public List<LightingUnit>[] _groups = new List<LightingUnit>[17];
        public List<LightingUnit> UntouchedLights = new List<LightingUnit>();
        public double[] scenes = new double[16] {0, 30, 35, 40, 45, 50, 55, 60, 65, 70, 75, 80, 85, 90, 95, 100};


        public DateTime TimeOfCreation;
        static double _totalWattUsage = 0;
        double _stepInterval = 0.01; //intervallet hvormed der bliver ændret ved stepUp og stepDown
        double _fadeRate = 0.005;

        public DALIController(List<LightingUnit> AllLightsInSystem)
        {
            AllLights = AllLightsInSystem;
            TimeOfCreation = DateTime.Now;
        }

        public void RemoveUnitFromAllGroups(LightingUnit UnitToRemove)
        {
            foreach (List<LightingUnit> group in _groups)
            {
                if(group.Contains(UnitToRemove))
                    group.Remove(UnitToRemove);
            }
            UnitToRemove.ForcedLightlevel = 0;
            UntouchedLights.Add(UnitToRemove);
        }


        public void RemoveUnitFromGroup(LightingUnit UnitToRemove, int GroupToRemoveFrom)
        {
            bool GroupsAreClear = true;
            _groups[GroupToRemoveFrom].Remove(UnitToRemove);
            foreach (List<LightingUnit> group in _groups)
            {
                if (group.Contains(UnitToRemove))
                {
                    GroupsAreClear = false;
                }
            }

            if(GroupsAreClear == true)
            {
                UnitToRemove.ForcedLightlevel = 0;
                UntouchedLights.Add(UnitToRemove);
            }
        }

        public void Extinguishgroup(int groupnumber)
        {
            foreach (LightingUnit Unit in _groups[groupnumber])
            {
                Unit.Extinguish();
            }
        }

        public void TurnOnGroup(int groupnumber)
        {
            foreach (LightingUnit Unit in _groups[groupnumber])
            {
                Unit.TurnOn();
            }
        }

        public void GroupGoToScene(int groupNumber, double scene)
        {
            foreach (LightingUnit Unit in _groups[groupNumber])
            {
                AddressGoToScene(Unit, scene);
            }
        }

        public void BroadcastOnAllUnits()
        {
            foreach (LightingUnit Unit in AllLights)
            {
                AddUnitToGroup(Unit, 16);
            }
        }

        public void ClearGroup(int groupNumber)
        {
            UntouchedLights.AddRange(_groups[groupNumber]);
            _groups[groupNumber].Clear();
        }

        public void ClearBroadcastGroup()
        {
            ClearGroup(16);
        }

        public void ClearAllGroups()
        {
            foreach (List<LightingUnit> group in _groups)
            {
                group.Clear();
            }
            UntouchedLights.Clear();
            UntouchedLights.AddRange(AllLights);
        }

        public void AddressGoToScene(LightingUnit Unit, double scene)
        {
            Unit.ForcedLightlevel = scene/100;
        }

        public void InitGroups()
        {
            UntouchedLights.AddRange(AllLights);
            for (int i = 0; i <= _groups.Length-1; i++)
            {
                _groups[i] = new List<LightingUnit>();
            }
        }

        public LightingUnit FindUnitWithAddress(int AddressToFind)
        {
            int index = AllLights.FindIndex(a => a.Address == AddressToFind);
            Console.WriteLine(index);
            return AllLights[index];       
        }

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

            UntouchedLights.Remove(UnitToAdd);
        }

        public void IncrementLights(List<LightingUnit> NewLightList)
        {

            foreach (LightingUnit Unit in AllLights)
            {
                _totalWattUsage += Unit.getWattUsageForLightUnitInHours();
            }

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

            ResetWantedLightLevels();
            UpdateUntouchedLights();
        }

        private void UpdateUntouchedLights()
        {
            IEnumerable<LightingUnit> q = AllLights;

            foreach (List<LightingUnit> Group in _groups)
            {
                q = q.Except(Group);
            }

            UntouchedLights = q.ToList();

        }

        private void ResetWantedLightLevels()
        {
            foreach (LightingUnit Light in AllLights)
            {
                Light.wantedLightLevel = 0;                
            }
        }

        public double GetTotalWattusage()
        {
            return _totalWattUsage;
        }


    }
}
