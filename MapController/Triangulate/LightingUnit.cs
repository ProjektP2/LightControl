using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightControl;

namespace Triangulering
{
    class LightingUnit : Coords
    {
        public bool IsUnitOn = true;
        double wattUsageInInterval;
        DateTime Time1;
        DateTime Time2;
        public int Address = 0;
        double maxLevel = 1.0; //max lysstyrke (basically en standard on knap)
        double minLevel = 0.5; //min lysstyrke (basically en standard off knap)
        static private int _address = 0;
        private double watts = 60; //skal bruges i udregninger til strømforbrug (har gemt et link jeg gerne lige vil snakke om :))
        public double wantedLightLevel;
        public double ForcedLightlevel;
        public double LightingLevel; //Lampens nuværende lysniveau  (skal måske laves til private hvis daliCommands skal køres(forklaring følger))//liste over grupper den enkelte light unit tilhører
        double[] scene = new double[16] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 }; //array af presets (her tænker jeg vi laver nogle standard scener 
        //der gælder for alle light units


        public LightingUnit() : this(0, 0) //for testing purposes only. will be deleted later
        {
        }

        public LightingUnit(double X, double Y) //constructor for lightingUnit (modtager x og y og skaber en light unit med x, y samt en adresse/id
        {
            x = X;
            y = Y;
            Address = _address;
            _address++;
            Time1 = DateTime.Now;
            //Console.WriteLine(_address);
            //her skal vi have lavet en sikkerhedsforanstaltning der starter en ny liste når _address når 63

        }

        public double getWattUsageForLightUnitInHours()
        {
            Time2 = DateTime.Now;
            TimeSpan WattInterval = Time2 - Time1;
            wattUsageInInterval = WattInterval.TotalHours * LightingLevel * watts;
            Time1 = Time2;
            return wattUsageInInterval;
            //Console.WriteLine("{0} er adressen med {1} som wattbrug",Address,wattUsage);
        }
        /*
        public void clearGroupsFromLightingUnit(LightingUnit LightingUnitToClearGroupsFrom) //blot en simpel funktion til at rense grupperne i en Light Unit
        {
            LightingUnitToClearGroupsFrom.groups.Clear();
        }
        */
        public double goToMax()
        {
            getWattUsageForLightUnitInHours();
            LightingLevel = maxLevel;
            return LightingLevel;
        }

        public double goToMin()
        {
            getWattUsageForLightUnitInHours();
            LightingLevel = minLevel;
            return LightingLevel;
        }

        public void Extinguish() //sluk med det samme!!!
        {
            getWattUsageForLightUnitInHours();
            IsUnitOn = false;
        }

        public Coords GetCoords()
        {
            Coords ReturnCoords = new Coords(x, y);
            return ReturnCoords;
        }

    }
}
