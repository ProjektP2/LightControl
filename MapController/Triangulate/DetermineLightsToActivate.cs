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
        private static double Radius = 200; //Lights that are further than 3 meters away from the user will not be activated.
        private static double MaxDistanceFromPath = 60; //The maximum distance lighting units can stray from the path of direction
                                                         //to be activated.
        private static double PredictedMovementScaling = 800; //The amount of times we scale the movement vector when predicting movement.

        internal Triangulate Triangulate
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        internal VectorMath VectorMath
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        internal Triangulate Triangulate1
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        internal VectorMath VectorMath1
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        //Determines what lights need to be activated around the user. Not based on movement. 
        //Receives a list of coordinates for every lighting unit in a room (LightingUnitsCoordinates), as well as the last known position of the 
        //signal source (UserCoordinates). Calls "ExistsInCircle" to determine whether or not the lights in the room are close
        //enough to activate. If they are (if their coordinates exist in the circle around the user), the coordinates to the lighting
        //unit is stored in the LightingUnitsToActivate list (List<LightingUnits>), which is returned when every lighting unit has been checked.
        //Furthermore, the LightingLevel variable from the LightingUnit class is set by CalculateLightingLevel.
        public static List<LightingUnit> LightsToActivateOnUser(Occupant Occupant, List<LightingUnit> LightingUnits)
        {
            List<LightingUnit> LightingUnitsToActivateOnUser = new List<LightingUnit>();

            foreach (LightingUnit LightingUnitToCheck in LightingUnits)
            {
                if (ExistsInCircle(Occupant.LatestPosition(), LightingUnitToCheck))
                {
                    LightingUnitToCheck.LightingLevel = (CalculateLightingLevel(Occupant.LatestPosition(), LightingUnitToCheck, Radius));
                    LightingUnitsToActivateOnUser.Add(LightingUnitToCheck);
                }
            }
            return LightingUnitsToActivateOnUser;
        }

        //Checks whether or not a set of coordinates exist inside a given circle. We check if the distance between the centre
        //and the given coordinates is smaller than the radius. If it is, the coordinates exist inside the circle.
        private static bool ExistsInCircle(Coords Centre, Coords CoordinatesToCheck)
        {
            if (Triangulate.CalculateDistanceBetweenPoints(Centre, CoordinatesToCheck) < Radius)
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

        public static List<LightingUnit> LightsToActivateInPath(Occupant Occupant, List<LightingUnit> LightingUnits)
        {
            if (Occupant.IsPosition1Initialized == false && Occupant.IsPosition2Initialized == false)
                return null;
            //Makes sure we don't calculate a movement vector when we don't have
            //two points


            Coords MovementVector = VectorMath.CalculateVector(Occupant.Position1, Occupant.Position2); //Defines the movement vector
            List<LightingUnit> LightingUnitsInPath = FindLightsInPath(Occupant.Position2, MovementVector, LightingUnits); 
            List<LightingUnit> LightingUnitsToActivateInPath = new List<LightingUnit>(); //List to return

            foreach (LightingUnit LightingUnitToSave in LightingUnitsInPath)
            {
                LightingUnitToSave.LightingLevel = (CalculateLightingLevel(Occupant.Position2, LightingUnitToSave, PredictedMovementScaling));
                LightingUnitsToActivateInPath.Add(LightingUnitToSave);
            }
            return LightingUnitsToActivateInPath;
        }

        //Returns a list of coordinates for lighting units. A lighting units coordinates are added to the returned list if the length 
        //of the perpendicular vector from the lighting unit to the movement vector is less than MaxDistanceFromPath. 
        private static List<LightingUnit> FindLightsInPath(Coords EndingPosition, Coords MovementVector, List<LightingUnit> LightingUnits)
        {
            List<LightingUnit> LightingUnitsInPath = new List<LightingUnit>();
            //double DistanceFromPath;
            Coords IntersectionPoint = new Coords();
            MovementVector.x *= PredictedMovementScaling;
            MovementVector.y *= PredictedMovementScaling;
            
            foreach (LightingUnit LightingUnitToCheck in LightingUnits)
            {
                 IntersectionPoint = VectorMath.projectionLength(LightingUnitToCheck, MovementVector, EndingPosition);

                if (IntersectionPoint.x >= 0 && IntersectionPoint.x <= MovementVector.x || IntersectionPoint.x <= 0 && IntersectionPoint.x >= MovementVector.x)
                {
                    if (IntersectionPoint.y >= 0 && IntersectionPoint.y <= MovementVector.y || IntersectionPoint.y <= 0 && IntersectionPoint.y >= MovementVector.y)
                    {
                        if ((Triangulate.CalculateDistanceBetweenPoints(VectorMath.SubtractVectors(LightingUnitToCheck.GetCoords(), EndingPosition) , IntersectionPoint) < MaxDistanceFromPath))
                        {
                            LightingUnitsInPath.Add(LightingUnitToCheck);
                        }    
                    }
                }
            }
            return LightingUnitsInPath;
        }

        private static void ConcatLightingUnits(ref List<LightingUnit> AllLightingUnits, List<LightingUnit> OnUser, List<LightingUnit> InPath)
        {
            int i = 0;
            int k = 0;
            int j = 0;

            foreach (LightingUnit OriginalUnit in AllLightingUnits)
            {
                if (i >= OnUser.Count)
                {
                    break;
                }

                if (OriginalUnit.Address == OnUser[i].Address)
                {
                    OriginalUnit.wantedLightLevel = OnUser[i].LightingLevel;
                }
                else
                {
                    OriginalUnit.wantedLightLevel = 0;
                }
                i++;
            }

            foreach (LightingUnit OriginalUnit in AllLightingUnits)
            {
                if (k >= InPath.Count)
                {
                    break;
                }

                if (OriginalUnit.Address == InPath[k].Address && OriginalUnit.LightingLevel < InPath[k].LightingLevel)
                {
                    OriginalUnit.wantedLightLevel = InPath[k].LightingLevel;
                }
                else
                {
                    OriginalUnit.wantedLightLevel = 0;
                }
                k++;
            }
        }

        public static void FindUnitsToActivate(ref List<LightingUnit> AllLightingUnits, Occupant Occupant)
        {
            ConcatLightingUnits(ref AllLightingUnits, LightsToActivateOnUser(Occupant, AllLightingUnits), 
                                                      LightsToActivateInPath(Occupant, AllLightingUnits));
        }
    }
}
