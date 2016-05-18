using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triangulering;

namespace MapController.Triangulate
{
    public class DALIGroup
    {
        public List<LightingUnit> GroupOfLights = new List<LightingUnit>();
        private static int _groupNumberCounter = 0;
        public static int Groupnumber;
        public bool isGroupUsed = false;

        public DALIGroup()
        {
            Groupnumber = _groupNumberCounter;
            _groupNumberCounter++;
        }

        public void AddUnitToGroup(LightingUnit Unit)
        {
            bool AddUnit = true;

            if (GroupOfLights.Contains(Unit))
            {
                AddUnit = false;
            }

            if (AddUnit == true)
            {
                GroupOfLights.Add(Unit);
            }
        }

        public void RemoveUnit(LightingUnit Unit)
        {
            GroupOfLights.Remove(Unit);
        }

        public void ExtinguishGroup()
        {
            foreach (LightingUnit Unit in GroupOfLights)
            {
                Unit.Extinguish();
            }
        }

        public void TurnOnGroup()
        {
            foreach (LightingUnit Unit in GroupOfLights)
            {
                Unit.TurnOn();
            }
        }

        public void GroupGoToScene(double scene)
        {
            foreach (LightingUnit Unit in GroupOfLights)
            {
                isGroupUsed = true;
                Unit.ForcedLightlevel = scene;
            }
        }

        public void ClearGroup()
        {
            GroupOfLights.Clear();
            isGroupUsed = false;
        }

        public void Increment(double _fadeRate, double _stepInterval)
        {
            if (isGroupUsed == true)
            {
                foreach (LightingUnit Unit in GroupOfLights)
                {
                    if (Unit.IsUnitOn == false)
                    {
                        Unit.LightingLevel = 0;
                    }
                    else if (Unit.LightingLevel > Unit.ForcedLightlevel)
                    {
                        Unit.LightingLevel = Unit.LightingLevel - _fadeRate;
                    }

                    else if (Unit.LightingLevel < Unit.ForcedLightlevel)
                    {
                        Unit.LightingLevel = Unit.LightingLevel + _stepInterval;
                    }
                }
            }          
        }

        public List<LightingUnit> ReturnListIfGroupIsUsed()
        {

            if (isGroupUsed == false)
            {
                return new List<LightingUnit>();
            }
            else
            {
                return GroupOfLights;
            }


        }
    }
}
