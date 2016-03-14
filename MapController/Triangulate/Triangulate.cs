using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightControl;
using SimEnvironment;

namespace Triangulering
{

    class Triangulate
    {

        //Calculates the distance between two sets coordinates.
        //This value is a length, so it cannot be negative.
        public static double CalculateDistanceBetweenPoints(Coords Center1, Coords Center2)
        {
            double Distance = (Math.Sqrt(Math.Pow(Center1.x - Center2.x, 2)) + Math.Pow(Center1.y - Center2.y, 2));
            if (Distance < 0)
            {
                Distance = Distance * (-1);
            }
            return (Distance);
        }

        //Calculates 'a', the distance from the Center1's coordinates to the line between the two possible positions for the signal source.
        //Note: We don't know the possible positions yet. However, we do know where the line between the two points is.
        //This value is a length, so it cannot be negative.
        private static double CalculateA(Circle Circle1, Circle Circle2, double DistanceBetweenCenters)
        {
            double a = ((Math.Pow(Circle1.Radius, 2) - Math.Pow(Circle2.Radius, 2) + (Math.Pow(DistanceBetweenCenters, 2))) / (2 * DistanceBetweenCenters));
            if (a < 0)
            {
                a = a * (-1);
            }
            return (a);
        }

        //Calculates 'h', the distance between where 'a' meets the line going between the possible positions, and 
        //one (or both, h is identical in both cases) of the possible positions.
        //This value is a length, so it cannot be negative.
        private static double CalculateH(Circle Circle1, double a)
        {
            double HSquared = (Math.Pow(Circle1.Radius, 2) - Math.Pow(a, 2));

            if (HSquared < 0)
            {
                HSquared = HSquared * (-1);
            }

            return (Math.Sqrt(HSquared));
        }

        //Calculates 'P2', the set of coordinates that makes up the point where 'a' meets the line going between the two possible positions.
        private static Coords CalculateP2(Coords Center1, Coords Center2, double DistanceBetweenCenters, double a)
        {
            Coords P2 = new Coords();
            P2.x = a * (Center2.x - Center1.x) / DistanceBetweenCenters;
            P2.y = a * (Center2.y - Center1.y) / DistanceBetweenCenters;
            return P2;
        }

        //Calculates the two set of coordinates that make up the two possible positions of the source signal
        //In this case, it's where the two radius's endpoints meet.
        private static Coords[] CalculatePositionOfSource(Coords Center1, Coords Center2, Coords P2, double DistanceBetweenCenters, double h)
        {
            Coords PositionOfSource1 = new Coords();
            Coords PositionOfSource2 = new Coords();
            Coords[] PositionsOfSource = new Coords[2];
            PositionsOfSource[0] = PositionOfSource1;
            PositionsOfSource[1] = PositionOfSource2;


            PositionOfSource1.x = P2.x + h * (Center2.y - Center1.y) / DistanceBetweenCenters;
            PositionOfSource1.y = P2.y - h * (Center2.x - Center1.x) / DistanceBetweenCenters;

            PositionOfSource2.x = P2.x - h * (Center2.y - Center1.y) / DistanceBetweenCenters;
            PositionOfSource2.y = P2.y + h * (Center2.x - Center1.x) / DistanceBetweenCenters;

            return PositionsOfSource;
        }

        //Collection of function calls. Creates all variables needed and calls all the functions to determine the possible positions
        //of the source signal.
        private static Coords[] TriangulateSignalSource(Circle Circle1, Circle Circle2)
        {
            //Calculates all variables needed for triangulation (Distance between Centers, a, h and P2)
            double DistanceBetweenCenters = CalculateDistanceBetweenPoints(Circle1, Circle2);
            double a = CalculateA(Circle1, Circle2, DistanceBetweenCenters);
            double h = CalculateH(Circle1, a);
            Coords P2 = CalculateP2(Circle1, Circle2, DistanceBetweenCenters, a);

            Coords[] PositionsOfSignalSource = new Coords[2];
            PositionsOfSignalSource = CalculatePositionOfSource(Circle1, Circle2, P2, DistanceBetweenCenters, h);

            //For debugging only****************************************
            PrintEverythingForDebug(DistanceBetweenCenters, a, h, P2);
            //For debugging only****************************************

            return PositionsOfSignalSource;
        }

        private static void PrintEverythingForDebug(double DistanceBetweenCenters, double a, double h, Coords P2)
        {
            Console.WriteLine($"Distance between the centers was calculated to: {Math.Round(DistanceBetweenCenters,3)}");
            Console.WriteLine($"a was calculated to: {Math.Round(a,3)}");
            Console.WriteLine($"h was calculated to: {Math.Round(h,3)}");
            Console.WriteLine($"Coordinates of P2 were calculated to: ({Math.Round(P2.x,3)},{Math.Round(P2.y,3)})");
        }

        public static void DetermineSignalStrengthFromCoords(Occupant SignalSource, Circle Router1, Circle Router2)
        {
            Coords CoordinatesTouse = new Coords();
            Coords Router1Center = new Coords(Router1.x, Router1.y);
            Coords Router2Center = new Coords(Router2.x, Router2.y);

            if (SignalSource.IsPosition2Initialized == false)
            {
                CoordinatesTouse = SignalSource.Position1;
            }

            else
            {
                CoordinatesTouse = SignalSource.Position2;
            }

            //Distance between Router1 and CoordinatesTouse, set this to the radius of Router1

            Router1.Radius = CalculateDistanceBetweenPoints(CoordinatesTouse, Router1Center);
            Router2.Radius = CalculateDistanceBetweenPoints(CoordinatesTouse, Router2Center);
        }

        public static Coords ExcludeImpossiblePosition(
            Occupant SignalSource, Circle Router1, Circle Router2, Coords[] PositionsOfSignalSource)
        {
            //If Routers are placed on the western wall
            if (Router1.x == 0 && Router2.x == 0)
            {
                if (PositionsOfSignalSource[0].x < 0)
                {
                    return PositionsOfSignalSource[1];
                }
                else
                {
                    return PositionsOfSignalSource[0];
                }
            }

            //If routers are placed on southern wall
            else if (Router1.y == GEngine.FormHeigt && Router2.y == GEngine.FormHeigt)
            {
                if (PositionsOfSignalSource[0].y > GEngine.FormHeigt)
                {
                    return PositionsOfSignalSource[1];
                }
                else
                {
                    return PositionsOfSignalSource[0];
                }
            }

            //If routers are placed on eastern wall
            else if (Router1.x == GEngine.FormWidht && Router2.x == GEngine.FormWidht)
            {
                if (PositionsOfSignalSource[0].x > GEngine.FormWidht)
                {
                    return PositionsOfSignalSource[1];
                }
                else
                {
                    return PositionsOfSignalSource[0];
                }
            }

            //If routers are placed on northern wall
            else if (Router1.y == 0 && Router2.y == 0)
            {
                if (PositionsOfSignalSource[0].y < 0)
                {
                    return PositionsOfSignalSource[1];
                }
                else
                {
                    return PositionsOfSignalSource[0];
                }
            }

            else
            {
                Console.WriteLine("No possible positions.");
                return (null);
            }

        }

        //Calls all functions in this class... returns the single possible position of the signal source.
        public static void TriangulatePositionOfSignalSource(
            Occupant SignalSource, Circle Router1, Circle Router2)
        {
            DetermineSignalStrengthFromCoords(SignalSource, Router1, Router2);
            Coords[] PossiblePositions = TriangulateSignalSource(Router1, Router2);
            SignalSource.UpdatePositions(ExcludeImpossiblePosition(SignalSource, Router1, Router2, PossiblePositions));
        }

    }
}
