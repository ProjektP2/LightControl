using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triangulering;

namespace MapController.Triangulate
{
    class DALIController
    {
        List<LightingUnit> LightingUnitsToDim = new List<LightingUnit>();
        List<LightingUnit> OldLightList = new List<LightingUnit>();
        public void DaliController(List<LightingUnit> NewLightList)
        {
            List<LightingUnit> LightsToBeAddedToTheDimmingList = OldLightList.Except(NewLightList).ToList();

           
        }
    }
}
