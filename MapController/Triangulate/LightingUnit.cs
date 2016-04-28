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
        double _wattUsageInInterval;
        DateTime _oldTime;
        DateTime _newTime;
        public int Address = 0;

        //max lysstyrke (basically en standard on knap)
        double _maxLevel = 1.0;
        
        //min lysstyrke (basically en standard off knap)
        public double minLevel = 0;
        static private int _address = 0;

        //skal bruges i udregninger til strømforbrug (har gemt et link jeg gerne lige vil snakke om :))
        private double _watts;

        public double Watts
        {
            get { return _watts; }
            private set { _watts = value; }
        }

        public double wantedLightLevel;
        public double ForcedLightlevel;

        //Lampens nuværende lysniveau  (skal måske laves til private hvis daliCommands skal køres
        public double LightingLevel;

        //for testing purposes only. will be deleted later
        public LightingUnit() : this(0, 0, 0) 
        {
        }

        //constructor for lightingUnit (modtager x og y og skaber en light unit med x, y samt en adresse/id
        public LightingUnit(double X, double Y, double watts) 
        {
            x = X;
            y = Y;
            Address = _address;
            _address++;
            _oldTime = DateTime.Now;
            Watts = watts;
            //her skal vi have lavet en sikkerhedsforanstaltning der starter en ny liste når _address når 63

        }

        public double getWattUsageForLightUnitInHours()
        {
            _newTime = DateTime.Now;
            TimeSpan WattInterval = _newTime - _oldTime;
            _wattUsageInInterval = WattInterval.TotalHours * LightingLevel * _watts;
            _oldTime = _newTime;
            return _wattUsageInInterval;
        }

        public double goToMax()
        {
            getWattUsageForLightUnitInHours();
            LightingLevel = _maxLevel;
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
