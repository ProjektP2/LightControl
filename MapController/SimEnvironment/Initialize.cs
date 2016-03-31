using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightControl;
using Triangulering;
using SimEnvironment;

namespace MapController.SimEnvironment
{
    //This class is used to organize the initialization of all objects required for simulating light control. 
    class Initialize
    {
        Occupant SignalSource;

        Circle Router1, Router2;

        List<LightingUnit> AllLightingUnits = new List<LightingUnit>();
        List<LightingUnit> ActivatedLightingUnitsOnUser = new List<LightingUnit>();
        List<LightingUnit> ActivatedLightingUnitsInPath = new List<LightingUnit>();


    }
}
