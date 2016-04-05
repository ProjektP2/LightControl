﻿using LightControl;
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

    class Triangulate
    {

        //Calculates the distance between two sets coordinates.
        //This value is a length, so it cannot be negative.
        public static double CalculateDistanceBetweenPoints(Coords Center1, Coords Center2)
        {
            double Distance = (Math.Sqrt((Math.Pow(Center1.x - Center2.x, 2)) + Math.Pow(Center1.y - Center2.y, 2)));
            if (Distance < 0)
                Distance = Distance * (-1);

            return (Distance);
        }

        private static void PrintEverythingForDebug(double DistanceBetweenCenters, double a, double h, Coords P2)
        {
            Console.WriteLine($"Distance between the centers was calculated to: {Math.Round(DistanceBetweenCenters, 3)}");
            Console.WriteLine($"a was calculated to: {Math.Round(a, 3)}");
            Console.WriteLine($"h was calculated to: {Math.Round(h, 3)}");
            Console.WriteLine($"Coordinates of P2 were calculated to: ({Math.Round(P2.x, 3)},{Math.Round(P2.y, 3)})");
        }

        public static void DetermineSignalStrengthFromCoords(Occupant SignalSource, Circle Router1, Circle Router2)
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

        public static Coords ExcludeImpossiblePositions(Occupant Source, Coords[] PositionsOfSignalSource)
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
        public static void TriangulatePositionOfSignalSource(
            Occupant SignalSource, Circle Router1, Circle Router2)
        {
            DetermineSignalStrengthFromCoords(SignalSource, Router1, Router2);
            Coords[] PossiblePositions = FindCircleCircleIntersections(Router1, Router2);
            SignalSource.UpdateWifiPosition(ExcludeImpossiblePositions(SignalSource, PossiblePositions));
        }

        // Find the points where the two circles intersect.
        public static Coords[] FindCircleCircleIntersections(Circle Router1, Circle Router2)
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
    }
}
