using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightControl;

namespace Triangulering
{
    public class LightingUnit : Coords
    {
        public bool IsUnitOn = true;
        double wattUsageInInterval;
        DateTime Time1;
        DateTime Time2;
        public int Address = 0;

        //max lysstyrke (basically en standard on knap)
        double maxLevel = 1.0;
        
        //min lysstyrke (basically en standard off knap)
        double minLevel = 0.00;
        static private int _address = 0;

        //skal bruges i udregninger til strømforbrug (har gemt et link jeg gerne lige vil snakke om :))
        private double watts = 240; 
        public double wantedLightLevel;
        public double ForcedLightlevel;

        //Lampens nuværende lysniveau  (skal måske laves til private hvis daliCommands skal køres
        public double LightingLevel;

        //for testing purposes only. will be deleted later
        public LightingUnit() : this(0, 0) 
        {
        }

        //constructor for lightingUnit (modtager x og y og skaber en light unit med x, y samt en adresse/id
        public LightingUnit(double X, double Y) 
        {
            x = X;
            y = Y;
            Address = _address;
            _address++;
            Time1 = DateTime.Now;
            //her skal vi have lavet en sikkerhedsforanstaltning der starter en ny liste når _address når 63

        }

        public double getWattUsageForLightUnitInHours()
        {
            Time2 = DateTime.Now;
            TimeSpan WattInterval = Time2 - Time1;
            wattUsageInInterval = WattInterval.TotalHours * LightingLevel * watts;
            Time1 = Time2;
            return wattUsageInInterval;
        }

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

        //sluk med det samme
        public void Extinguish() 
        {
            getWattUsageForLightUnitInHours();
            IsUnitOn = false;
        }

        public void TurnOn()
        {
            getWattUsageForLightUnitInHours();
            IsUnitOn = true;
        }

        public Coords GetCoords()
        {
            Coords ReturnCoords = new Coords(x, y);
            return ReturnCoords;
        }

        //Implements IComparable's CompareTo()
        public int CompareTo(LightingUnit next)
        {
            return next.wantedLightLevel.CompareTo(wantedLightLevel);
        }

    }
}
