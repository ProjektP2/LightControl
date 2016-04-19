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
        List<LightingUnit>[] groups = new List<LightingUnit>[17];
        public List<LightingUnit> UntouchedLights = new List<LightingUnit>();
        List<LightingUnit> LightsOff = new List<LightingUnit>();
        public double[] scenes = new double[16] {5, 10, 20, 30, 40, 45, 50, 55, 60, 65, 70, 75, 80, 85, 90, 100};

        
        static double totalWattUsage = 0;
        double stepInterval = 0.01; //intervallet hvormed der bliver ændret ved stepUp og stepDown
        double fadeRate = 0.005;

        public DALIController(List<LightingUnit> AllLightsInSystem)
        {
            AllLights = AllLightsInSystem;
        }

        public void RemoveUnitFromAllGroups(LightingUnit UnitToRemove)
        {
            foreach (var item in groups)
            {
                if(item.Contains(UnitToRemove))
                    item.Remove(UnitToRemove);
            }

            UntouchedLights.Add(UnitToRemove);
        }

        public void RemoveUnitFromGroup(LightingUnit UnitToRemove, int GroupToRemoveFrom)
        {
            bool GroupsAreClear = true;
            groups[GroupToRemoveFrom].Remove(UnitToRemove);
            foreach (var item in groups)
            {
                if (item.Contains(UnitToRemove))
                {
                    GroupsAreClear = false;
                }
            }

            if(GroupsAreClear == true)
            {
                UntouchedLights.Add(UnitToRemove);
            }
        }

        public void AddressGoToScene(LightingUnit Unit, double scene)
        {
            AddUnitToGroup(Unit, 16);
            Unit.ForcedLightlevel = scene/100;
        }

        public void InitGroups()
        {
            UntouchedLights.AddRange(AllLights);
            for (int i = 0; i <= groups.Length-1; i++)
            {
                groups[i] = new List<LightingUnit>();
            }
        }

        public LightingUnit findUnitWithAddress(int AddressToFind)
        {
            var index = AllLights.FindIndex(a => a.Address == AddressToFind);
            Console.WriteLine(index);
            return AllLights[index];
        }

        public void AddUnitToGroup(LightingUnit UnitToAdd, int groupNumber)
        {
            bool AddUnit = true;

            if (groups[groupNumber].Contains(UnitToAdd))
            {
                AddUnit = false;
            }

            if (AddUnit == true)
            {
                groups[groupNumber].Add(UnitToAdd);
            }

            if (UntouchedLights.Contains(UnitToAdd))
            {
                UntouchedLights.Remove(UnitToAdd);
            }

        }

        public void IncrementLights(List<LightingUnit> NewLightList)
        {

            foreach (var group in groups)
            {
                foreach (var item in group)
                {

                    if (item.IsUnitOn == false)
                    {
                        totalWattUsage += item.getWattUsageForLightUnitInHours();
                        item.LightingLevel = 0;
                    }

                    else if (item.LightingLevel > item.ForcedLightlevel)
                    {
                        totalWattUsage += item.getWattUsageForLightUnitInHours();
                        item.LightingLevel = item.LightingLevel - fadeRate;
                    }

                    else if (item.LightingLevel < item.ForcedLightlevel)
                    {
                        totalWattUsage += item.getWattUsageForLightUnitInHours();
                        item.LightingLevel = item.LightingLevel + stepInterval;
                    }

                    else
                    {
                        item.LightingLevel = item.ForcedLightlevel;
                    }
                }
            }
            foreach (var item in UntouchedLights)
            {
                if (item.IsUnitOn == false)
                {
                    totalWattUsage += item.getWattUsageForLightUnitInHours();
                    item.LightingLevel = 0;
                }
                /*else if (item.wantedLightLevel <= item.LightingLevel + 0.01 || item.wantedLightLevel >= item.LightingLevel + 0.01)
                {
                    item.LightingLevel = item.wantedLightLevel;
                }*/
                else if (item.LightingLevel > item.wantedLightLevel)
                {
                    totalWattUsage += item.getWattUsageForLightUnitInHours();
                    item.LightingLevel = item.LightingLevel - fadeRate;
                }

                else if (item.LightingLevel < item.wantedLightLevel)
                {
                    totalWattUsage += item.getWattUsageForLightUnitInHours();
                    item.LightingLevel = item.LightingLevel + stepInterval;
                }
            }


            /*foreach (var item in groups[0])
            {
                Console.WriteLine(item.Address);
            }
            */

        }


        public double Wattusage()
        {
            return totalWattUsage;
        }
    }
}
