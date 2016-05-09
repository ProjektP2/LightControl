using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightControl;

namespace Triangulering
{
    public class DetermineLightsToActivate
    {
        #region Constructors
        public DetermineLightsToActivate(double radius, double distance, double scaling, Triangulation triangulation)
        {
            Radius = radius;
            MaxDistanceFromPath = distance;
            PredictedMovementScaling = scaling;
            _triangulation = triangulation;
        }
        #endregion

        #region Fields and Properties
        //Lights that are further than 3 meters away from the user will not be activated.
        private double _radius;
        public double Radius
        {
            get { return _radius; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Radius must be larger than, or equal to, zero.");
                else
                    _radius = value;
            }
        }

        //The maximum distance lighting units can stray from the path of direction to be activated.
        private double _maxDistanceFromPath;
        public double MaxDistanceFromPath
        {
            get { return _maxDistanceFromPath; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Distance from path must be larger than, or equal to, zero.");
                else
                    _maxDistanceFromPath = value;
            }
        }

        //The amount of times we scale the movement vector when predicting movement.
        private double _predictedMovementScaling;
        public double PredictedMovementScaling
        {
            get { return _predictedMovementScaling; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Path scaling must be larger than, or equal to, zero.");
                else
                    _predictedMovementScaling = value;
            }
        }

        private Triangulation _triangulation;

        #endregion

        #region Methods
        //Determines what lights need to be activated around the user. Not based on movement. 
        //Receives a list of coordinates for every lighting unit in a room (LightingUnitsCoordinates), as well as the last known position of the 
        //signal source (UserCoordinates). Calls "ExistsInCircle" to determine whether or not the lights in the room are close
        //enough to activate. If they are (if their coordinates exist in the circle around the user), the coordinates to the lighting
        //unit is stored in the LightingUnitsToActivate list (List<LightingUnits>), which is returned when every lighting unit has been checked.
        //Furthermore, the LightingLevel variable from the LightingUnit class is set by CalculateLightingLevel.

        public List<LightingUnit> LightsToActivateOnUser(Occupant Occupant, List<LightingUnit> LightingUnits)
        {
            List<LightingUnit> LightingUnitsToActivateOnUser = new List<LightingUnit>();

            foreach (LightingUnit LightingUnitToCheck in LightingUnits)
            {
                if (ExistsInCircle(Occupant.LatestPosition(), LightingUnitToCheck))
                {
                    LightingUnitToCheck.wantedLightLevel = Math.Round((CalculateLightingLevel(Occupant.LatestPosition(),
                                                            LightingUnitToCheck, 
                                                            _radius)),2);

                    LightingUnitsToActivateOnUser.Add(LightingUnitToCheck);
                }
            }

            EnsureCorrectLightingLevels(LightingUnitsToActivateOnUser);
            return LightingUnitsToActivateOnUser;
        }

        public Coords GetMovementVector(Occupant Occupant)
        {
            return VectorMath.CalculateVector(Occupant.Position1, Occupant.Position2);
        }

        //Checks whether or not a set of coordinates exist inside a given circle. We check if the distance between the centre
        //and the given coordinates is smaller than the radius. If it is, the coordinates exist inside the circle.
        private bool ExistsInCircle(Coords Centre, Coords CoordinatesToCheck)
        {
            if (_triangulation.CalculateDistanceBetweenPoints(Centre, CoordinatesToCheck) < _radius)
                return true;
            else
                return false;
        }

        //Calculates the percentage the distance between the lighting unit and the lighting unit makes up of the given distance. 
        //1 minus the percentage will be equal to the lighting level in that given lighting unit.
        //We're calling the CalculateDistanceBetweenPoints from the class Triangulate.
        private double CalculateLightingLevel(Coords UserCoordinates, Coords LightingUnitCoordinates, double distance)
        {
            double DistanceBetweenPoints = (_triangulation.CalculateDistanceBetweenPoints(UserCoordinates, 
                                                                                       LightingUnitCoordinates));
            return 1 - (DistanceBetweenPoints / distance);
        }

        public List<LightingUnit> LightsToActivateInPath(Occupant Occupant, List<LightingUnit> LightingUnits)
        {
            //Makes sure we don't find lights in path when no path is available
            if (Occupant.IsPosition1Initialized == false && 
                Occupant.IsPosition2Initialized == false)
                return null;

            //List to return
            List<LightingUnit> LightingUnitsToActivateInPath = new List<LightingUnit>();

            //Defines the movement vector
            Coords MovementVector = VectorMath.CalculateVector(Occupant.Position1, Occupant.Position2);

            List<LightingUnit> LightingUnitsInPath = FindLightsInPath(Occupant.Position2, MovementVector,
                                                                      LightingUnits);

            foreach (LightingUnit LightingUnitToSave in LightingUnitsInPath)
            {
                LightingUnitToSave.wantedLightLevel = (CalculateLightingLevel(Occupant.Position2, LightingUnitToSave,
                                                                              _predictedMovementScaling));

                LightingUnitsToActivateInPath.Add(LightingUnitToSave);
            }
            return LightingUnitsToActivateInPath;
        }

        //Returns a list of coordinates for lighting units. A lighting units coordinates are added to the returned list if the length 
        //of the perpendicular vector from the lighting unit to the movement vector is less than MaxDistanceFromPath. 
        private List<LightingUnit> FindLightsInPath(Coords EndingPosition, Coords MovementVector, List<LightingUnit> LightingUnits)
        {
            //List to return
            List<LightingUnit> LightingUnitsInPath = new List<LightingUnit>();

            //Point used for "distanceFromPath" check
            Coords IntersectionPoint = new Coords();

            //Scaling the movement vector
            MovementVector = VectorMath.ScaleVector(MovementVector, _predictedMovementScaling);

            foreach (LightingUnit LightingUnitToCheck in LightingUnits)
            {
                IntersectionPoint = VectorMath.projectionLength(LightingUnitToCheck, MovementVector, EndingPosition);

                if (IntersectionPoint.x >= 0 && 
                    IntersectionPoint.x <= MovementVector.x || 
                    IntersectionPoint.x <= 0 && 
                    IntersectionPoint.x >= MovementVector.x)
                {
                    if (IntersectionPoint.y >= 0 && 
                        IntersectionPoint.y <= MovementVector.y || 
                        IntersectionPoint.y <= 0 && 
                        IntersectionPoint.y >= MovementVector.y)
                    {
                        if ((_triangulation.CalculateDistanceBetweenPoints
                                                    (VectorMath.SubtractVectors
                                                           (LightingUnitToCheck.GetCoords(), EndingPosition), 
                                                            IntersectionPoint)
                                                         < _maxDistanceFromPath))
                        {
                            LightingUnitsInPath.Add(LightingUnitToCheck);
                        }
                    }
                }
            }
            return LightingUnitsInPath;
        }

        private void ConcatLightingUnits(List<LightingUnit> AllLightingUnits, List<LightingUnit> OnUser, List<LightingUnit> InPath)
        {
            int i = 0;
            int k = 0;

            foreach (LightingUnit OriginalUnit in AllLightingUnits)
            {
                if (i >= OnUser.Count)
                    break;

                if (OriginalUnit.Address == OnUser[i].Address)
                    OriginalUnit.wantedLightLevel = OnUser[i].wantedLightLevel;

                i++;
            }

            foreach (LightingUnit OriginalUnit in AllLightingUnits)
            {
                if (k >= InPath.Count)
                    break;

                if (OriginalUnit.Address == InPath[k].Address && 
                    OriginalUnit.wantedLightLevel < InPath[k].wantedLightLevel)
                    OriginalUnit.wantedLightLevel = InPath[k].wantedLightLevel;
                else

                k++;
            }
        }

        public void FindUnitsToActivate(List<LightingUnit> AllLightingUnits, Occupant Occupant)
        {
            foreach (var item in AllLightingUnits)
                item.wantedLightLevel = 0;

            ConcatLightingUnits(AllLightingUnits, LightsToActivateOnUser(Occupant, AllLightingUnits),
                                                  LightsToActivateInPath(Occupant, AllLightingUnits));
        }

        //Makes sure that the cummulative lighting level around the user is at least 100% of one lighting unit
        private void EnsureCorrectLightingLevels(List<LightingUnit> listToCheck)
        {
            double totalLightingLevel = 0;
            LightingUnit unitToCorrect;

            foreach (LightingUnit unit in listToCheck)
                totalLightingLevel += unit.wantedLightLevel;

            if (totalLightingLevel < 1)
            {
                unitToCorrect = listToCheck.Aggregate((i, j) => i.wantedLightLevel > j.wantedLightLevel ? i : j);
                unitToCorrect.wantedLightLevel = 1;
            }
        }
        #endregion

    }
}
