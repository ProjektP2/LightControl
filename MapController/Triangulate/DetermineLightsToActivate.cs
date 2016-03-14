using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightControl;

namespace Triangulering
{
    class DetermineLightsToActivate
    {
        private static double Radius = 960; //Lights that are further than 3 meters away from the user will not be activated.
        private static double MaxDistanceFromPath = 1.5; //The maximum distance lighting units can stray from the path of direction
                                                         //to be activated.
        private static double PredictedMovementScaling = 3; //The amount of times we scale the movement vector when predicting movement.

        //Determines what lights need to be activated around the user. Not based on movement. 
        //Receives a list of coordinates for every lighting unit in a room (LightingUnitsCoordinates), as well as the last known position of the 
        //signal source (UserCoordinates). Calls "ExistsInCircle" to determine whether or not the lights in the room are close
        //enough to activate. If they are (if their coordinates exist in the circle around the user), the coordinates to the lighting
        //unit is stored in the LightingUnitsToActivate list (List<LightingUnits>), which is returned when every lighting unit has been checked.
        //Furthermore, the LightingLevel variable from the LightingUnit class is set by CalculateLightingLevel.
        public static List<LightingUnit> LightsToActivateOnUser(Coords UserCoordinates, List<LightingUnit> LightingUnits)
        {
            List<LightingUnit> LightingUnitsToActivateOnUser = new List<LightingUnit>();

            foreach (LightingUnit LightingUnitToCheck in LightingUnits)
            {
                if (ExistsInCircle(UserCoordinates, LightingUnitToCheck))
                {
                    LightingUnitToCheck.LightingLevel = (CalculateLightingLevel(UserCoordinates, LightingUnitToCheck, Radius));
                    LightingUnitsToActivateOnUser.Add(LightingUnitToCheck);
                    Console.WriteLine(LightingUnitToCheck.LightingLevel);
                }


            }
            return LightingUnitsToActivateOnUser;
        }

        //Checks whether or not a set of coordinates exist inside a given circle. We check if the distance between the centre
        //and the given coordinates is smaller than the radius. If it is, the coordinates exist inside the circle.
        private static bool ExistsInCircle(Coords Centre, Coords CoordinatesToCheck)
        {
            if (Triangulate.CalculateDistanceBetweenPoints(Centre, CoordinatesToCheck) < Math.Pow(Radius, 2))
                return true;
            else
                return false;
        }

        //Calculates the percentage the distance between the lighting unit and the lighting unit makes up of the given distance. 
        //1 minus the percentage will be equal to the lighting level in that given lighting unit.
        //We're calling the CalculateDistanceBetweenPoints from the class Triangulate.
        private static double CalculateLightingLevel(Coords UserCoordinates, Coords LightingUnitCoordinates, double distance)
        {
            double DistanceBetweenPoints = (Triangulate.CalculateDistanceBetweenPoints(UserCoordinates, LightingUnitCoordinates));
            return 1- (DistanceBetweenPoints / distance);
        }

        public static List<LightingUnit> LightsToActivateInPath(Coords StartingPosition, Coords EndingPosition, List<LightingUnit> LightingUnits)
        {
            if (EndingPosition.x == 99999 && EndingPosition.y == 99999) //Makes sure we don't calculate a movement vector when we don't have
                return null;                                            //two points


            Coords MovementVector = VectorMath.CalculateVector(StartingPosition, EndingPosition); //Defines the movement vector
            List<LightingUnit> LightingUnitsInPath = FindLightsInPath(EndingPosition, MovementVector, LightingUnits); 
            List<LightingUnit> ReducedLightingUnitsInPath = ReduceLightingUnitsInPath(LightingUnitsInPath, EndingPosition, MovementVector);
            List<LightingUnit> LightingUnitsToActivateInPath = new List<LightingUnit>(); //List to return

            foreach (LightingUnit LightingUnitToSave in ReducedLightingUnitsInPath)
            {
                LightingUnitToSave.LightingLevel = (CalculateLightingLevel(EndingPosition, LightingUnitToSave, PredictedMovementScaling));
                LightingUnitsToActivateInPath.Add(LightingUnitToSave);
            }
            return LightingUnitsToActivateInPath;
        }

        //Returns a list of coordinates for lighting units. A lighting units coordinates are added to the returned list if the length 
        //of the perpendicular vector from the lighting unit to the movement vector is less than MaxDistanceFromPath. 
        private static List<LightingUnit> FindLightsInPath(Coords EndingPosition, Coords MovementVector, List<LightingUnit> LightingUnits)
        {
            List<LightingUnit> LightingUnitsInPath = new List<LightingUnit>();
            double DistanceFromPath;

            foreach (LightingUnit LightingUnitToCheck in LightingUnits)
            {
                DistanceFromPath = VectorMath.LengthOfVector(VectorMath.PerpendicularVectorBetweenPointAndVector(MovementVector, LightingUnitToCheck, EndingPosition));

                if (DistanceFromPath < MaxDistanceFromPath)
                {
                    LightingUnitsInPath.Add(LightingUnitToCheck);
                }
            }
            return LightingUnitsInPath;
        }

        //Checks to see if the lighting units that are in the line of direction (path) of the user are too far away. 
        //If the lighting units are less than (Length of movement vector)*PredictedMovementScaling away from the last known 
        //position of the user (EndingPosition), their coordinates are added to a new list ReducedLightingUnitsInPath, which is returned.
        //Maybe this function could be implemented into FindLightsInPath by adding the condition from the if statement in this function
        //to the if statement in FindLightsInPath. This would reduce the time complexity of the program by an order of magnitude.
        //However, they are separate for now for the sake of readability.
        private static List<LightingUnit> ReduceLightingUnitsInPath(List<LightingUnit> LightingUnitsInPath, Coords EndingPosition, Coords MovementVector)
        {
            List<LightingUnit> ReducedLightingUnitsInPath = new List<LightingUnit>();
            double LengthOfPredictedPath = VectorMath.LengthOfVector(MovementVector) * PredictedMovementScaling;

            foreach (LightingUnit LightingUnitToCheck in LightingUnitsInPath)
            {
                if (Triangulate.CalculateDistanceBetweenPoints(LightingUnitToCheck,EndingPosition)<LengthOfPredictedPath)
                {
                    ReducedLightingUnitsInPath.Add(LightingUnitToCheck);
                }
            }
            return ReducedLightingUnitsInPath;
        }
    }
}
