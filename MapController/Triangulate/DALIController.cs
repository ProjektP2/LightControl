using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triangulering;

namespace LightControl
{
    class DALIController
    {
        List<int>[] groups = new List<int>[16];
        static double totalWattUsage = 0;
        double stepInterval = 0.02; //intervallet hvormed der bliver ændret ved stepUp og stepDown
        double fadeRate = 0.005;

        public void InitGroups()
        {
            for (int i = 0; i <= groups.Length-1; i++)
            {
                groups[i] = new List<int>();
            }
        }

        public void IncrementLights(ref List<LightingUnit> NewLightList)
        {
            for (int i = 0; i < NewLightList.Count; i++)
            {
                if (NewLightList[i].Address >= 128)
                {
                    NewLightList[i].goToMax();
                }
            }
            for (int i = 0; i < NewLightList.Count; i++)
            {
                if (NewLightList[i].IsUnitOn == false)
                {
                    NewLightList[i].LightingLevel = 0;
                }

                else if (NewLightList[i].LightingLevel > NewLightList[i].wantedLightLevel)
                {
                    totalWattUsage += NewLightList[i].getWattUsageForLightUnitInHours();
                    NewLightList[i].LightingLevel = NewLightList[i].LightingLevel - fadeRate;
                }

                else if (NewLightList[i].LightingLevel < NewLightList[i].wantedLightLevel)
                {
                    totalWattUsage += NewLightList[i].getWattUsageForLightUnitInHours();
                    NewLightList[i].LightingLevel = NewLightList[i].LightingLevel + stepInterval;
                }
            }
        }

        public double Wattusage()
        {
            return totalWattUsage;
        }
    }
}
