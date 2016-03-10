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
        double stepInterval = 0.01; //intervallet hvormed der bliver ændret ved stepUp og stepDown
        double maxLevel = 1.0; //max lysstyrke (basically en standard on knap)
        double minLevel = 0.0; //min lysstyrke (basically en standard off knap)
        private int _address = 0;
        private double fadeRate = 0.5; //jeg har en ide med den her (kan måske undgåes at skulle sammenligne to lister for at fade lamperne ud efter brug)
        private double watts = 60; //skal bruges i udregninger til strømforbrug (har gemt et link jeg gerne lige vil snakke om :))
        public double LightingLevel = 0.0; //Lampens nuværende lysniveau  (skal måske laves til private hvis daliCommands skal køres(forklaring følger))
        List<int> groups = new List<int>(); //liste over grupper den enkelte light unit tilhører
        double[] scene = new double[16] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 0, 0, 0, 0, 0, 0}; //array af presets (her tænker jeg vi laver nogle standard scener 
        //der gælder for alle light units


        public LightingUnit() :this(0, 0) //for testing purposes only. will be deleted later
        {
        }

        public LightingUnit(double X, double Y) //constructor for lightingUnit (modtager x og y og skaber en light unit med x, y samt en adresse/id
        {
            x = X;
            y = Y;
            Address = _address;
            _address++;
            //her skal vi have lavet en sikkerhedsforanstaltning der starter en ny liste når _address når 63

        }

        public void addLightUnitToGroup(int groupToAdd) //tilføjer en lightingUnit til en gruppe
        {

            if (groups.Contains(groupToAdd))
            {
                //her skal laves en rigtig exeption
                Console.WriteLine("The lighting unit is already part of that group");
            }

            else if (groups.Count() >= 4)
            {
                //exeption
                Console.WriteLine("There is currently no room to add another group please delete one before adding another");
            }

            else if (!groups.Contains(groupToAdd) && groups.Count() < 4) //her bliver 
            {
                groups.Add(groupToAdd);
            }

            else
            {
                Console.WriteLine("Something went completely wrong trying to add the light unit to another group");
            }
          

        }

        public void removeLightUnitFromGroup(int groupToRemove) //fjerner en gruppe fra en LightingUnit
        {
            if (!groups.Contains(groupToRemove))
            {
                //igen her skal laves en bedre exeption
                Console.WriteLine("The lighting unit is not a member of that group");
            }
            else if (groups.Contains(groupToRemove))
            {
                groups.Remove(groupToRemove);
            }
            else
            {
                //bedre exeption
                Console.WriteLine("Something went completely wrong trying to remove the light unit to another group");
            }


        }

        public void clearGroupsFromLightingUnit(LightingUnit LightingUnitToClearGroupsFrom) //blot en simpel funktion til at rense grupperne i en Light Unit
        {
            LightingUnitToClearGroupsFrom.groups.Clear();
        }

        public int Address
        {
            get { return _address; }
            private set { _address = value; }
        }


        public double goToMax()
        {
            LightingLevel = maxLevel;
            return LightingLevel;
        }

        public double goToMin()
        {
            LightingLevel = minLevel;
            return LightingLevel;
        }

        public double stepUp()
        {
            LightingLevel = LightingLevel + stepInterval;
            return LightingLevel;
        }

        public double stepDown()
        {
            LightingLevel = LightingLevel - stepInterval;
            return LightingLevel;
        }

        public void Extinguish(LightingUnit lightingUnitToExtinguish) //sluk med det samme!!!
        {
            lightingUnitToExtinguish.LightingLevel = 0;
        }
        /*public double setLightLevel(double wantedLightLevel)
        {
            while (LightingLevel > wantedLightLevel)
            {
                stepUp();
            }
            while (LightingLevel < wantedLightLevel)
            {
                stepUp();
            }
            return LightingLevel;
        }
        */
    }
}
