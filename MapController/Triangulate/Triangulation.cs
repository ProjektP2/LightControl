using LightControl;
using SimEnvironment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Triangulering
{
    //This class contains methods for triangulating the position of the signal source. This is done by
    //finding the intersection points between two circles, the radii of which are the received signal strengths.
    //Refer to chapter 10 for visual reference and mathematical understanding.

    public class Triangulation
    {
        #region Constructors

        public Triangulation(Circle router1, Circle router2)
        {
            Router1 = router1;
            Router2 = router2;
        }
        #endregion

        #region Fields and Properties
        private Circle _router1;
        public Circle Router1
        {
            get { return _router1; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Router cannot be null.");
                else
                    _router1 = value;
            }
        }

        private Circle _router2;
        public Circle Router2
        {
            get { return _router2; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Router cannot be null.");
                else
                    _router2 = value;
            }
        }

        #endregion

        #region Methods
        //Calculates the distance between two sets coordinates.
        //This value is a length, so it cannot be negative.
        public double CalculateDistanceBetweenPoints(Coords Router1, Coords Router2)
        {
            double Distance = (Math.Sqrt((Math.Pow(Router1.x - Router2.x, 2)) + Math.Pow(Router1.y - Router2.y, 2)));
            if (Distance < 0)
                Distance = Distance * (-1);

            return (Distance);
        }

        public void DetermineSignalStrengthFromCoords(Occupant SignalSource, Circle Router1, Circle Router2)
        {
            Coords CoordinatesTouse = new Coords();

            if (SignalSource.IsPosition2Initialized == false)
                CoordinatesTouse = SignalSource.Position1;
            else
                CoordinatesTouse = SignalSource.Position2;

            //Distance between Router and CoordinatesTouse, set this to the radius of Router

            Router1.Radius = CalculateDistanceBetweenPoints(CoordinatesTouse, Router1);
            Router2.Radius = CalculateDistanceBetweenPoints(CoordinatesTouse, Router2);
        }

        public void RandomizeSignalStrength(Circle Router)
        {
            double DegreeOfRandomization = Router.Radius / 40;
            double min = (Router.Radius - DegreeOfRandomization);
            double max = (Router.Radius + DegreeOfRandomization);

            Random random = new Random();
            Router.Radius = random.NextDouble() * (max - min) + min;
        }

        public Coords ExcludeImpossiblePositions(Occupant Source, Coords[] PositionsOfSignalSource)
        {
            if (PositionsOfSignalSource[0].x < 0 || PositionsOfSignalSource[0].x > GEngine.SimulationWidht)
            {
                return PositionsOfSignalSource[1];
            }
            else if (PositionsOfSignalSource[0].y < 0 || PositionsOfSignalSource[0].y > GEngine.SimulationHeigt)
            {
                return PositionsOfSignalSource[1];
            }
            else
            {
                return PositionsOfSignalSource[0];
            }
        }

        //Calls all functions in this class... returns the single possible position of the signal source.
        public void TriangulatePositionOfSignalSource(
            Occupant SignalSource, Circle Router1, Circle Router2)
        {
            DetermineSignalStrengthFromCoords(SignalSource, Router1, Router2);
            RandomizeSignalStrength(Router1);
            RandomizeSignalStrength(Router2);
            Coords[] PossiblePositions = FindCircleCircleIntersections(Router1, Router2);
            SignalSource.UpdateWifiPosition(ExcludeImpossiblePositions(SignalSource, PossiblePositions));
        }

        // Find the points where the two circles intersect.
        public Coords[] FindCircleCircleIntersections(Circle Router1, Circle Router2)
        {
            Coords P2;
            Coords Intersection1;
            Coords Intersection2;
            Coords[] Intersections = new Coords[2];

            // Find the distance between the centers.
            double dx = Router1.x - Router2.x;
            double dy = Router1.y - Router2.y;
            double Distance = CalculateDistanceBetweenPoints(Router1, Router2);

            // Get the values for a and h
            double a = (Router1.Radius * Router1.Radius -
                        Router2.Radius * Router2.Radius + Distance * Distance) /
                        (2 * Distance);
            double h = Math.Sqrt(Router1.Radius * Router1.Radius - a * a);

            // Get the coordinates of P2
            P2 = new Coords((Router1.x + a * (Router2.x - Router1.x) / Distance),
                            (Router1.y + a * (Router2.y - Router1.y) / Distance));

            // Get the coordinates for the two intersections P3 and P3'
            Intersection1 = new Coords(
                (P2.x + h * (Router2.y - Router1.y) / Distance),
                (P2.y - h * (Router2.x - Router1.x) / Distance));
            Intersection2 = new Coords(
                (P2.x - h * (Router2.y - Router1.y) / Distance),
                (P2.y + h * (Router2.x - Router1.x) / Distance));
            Intersections[0] = Intersection1;
            Intersections[1] = Intersection2;

            return Intersections;
        }
        #endregion

    }
}
