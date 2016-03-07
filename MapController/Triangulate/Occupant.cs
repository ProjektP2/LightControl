using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Triangulering
{
    //This class is used to simulate an occupant - or rather, the source of a given signal that is received by the access points
    //(instances of the Circle class, in this case.)
    //Contains two positions and an ID. The ID is never set so far. The two coordinates are used by the method CalculateVelocity
    //to calculate the speed of which the signal source is moving between two points.
    class Occupant
    {
        private bool IsPosition1Initialized = false;
        private bool IsPosition2Initialized = false;

        public Coords Position1 = new Coords();
        public Coords Position2 = new Coords();
        public Coords PositionVector = new Coords();
        public double Velocity; //Should only have a public getter
        public string Identity;

        //Sets the starting coordinates (i.e. first signal reading)
        public void SetStartingPosition(Coords Coordinates)
        {
            Position1.x = Coordinates.x;
            Position1.y = Coordinates.y;
            IsPosition1Initialized = true;
        }

        //Sets the identity of the signal source.
        public void SetIdentity(string NewIdentity)
        {
            Identity = NewIdentity;
        }

        //Updates the source signals known coordinates. If the second coordinate isn't yet initialized, Position2 will simply
        //be updated with the given coordinates. If Position2 has been initialized before, Position2's coordinates replaces
        //Position1, and Position2 is updated with the new coordinates.
        public void UpdatePositions(Coords Coordinates)
        {
            if (IsPosition2Initialized)
            {
                Position1.x = Position2.x;
                Position1.y = Position2.y;
                Position2.x = Coordinates.x;
                Position2.y = Coordinates.y;
            }

            else if (IsPosition1Initialized)
            {
                Position2.x = Coordinates.x;
                Position2.y = Coordinates.y;
                IsPosition2Initialized = true;
            }

            else
                Console.WriteLine("Need a starting position.");
        }

        //Calculates the position vector given by the two coordinates.
        public void CalculatePositionVector()
        {
            if (IsPosition1Initialized == false || IsPosition2Initialized == false)
            {
                Console.WriteLine("Need two positions to calculate velocity.");
            }
            else
            {
                PositionVector.x = Position2.x - Position1.x;
                PositionVector.x = Position2.y - Position1.y;
            }

        }

        //Calculates the magnitude of the position vector which, in our case, is the velocity of the signal source.
        public void CalculateVelocity()
        {
            CalculatePositionVector();
            Velocity = Math.Sqrt((Math.Pow(PositionVector.x,2)+Math.Pow(PositionVector.y,2)));
        }

        public void PrintEverything()
        {
            Console.WriteLine($"First position: ({Position1.x},{Position1.y})\n");
            Console.WriteLine($"Second position: ({Position2.x},{Position2.y})");
            Console.WriteLine($"Position vector: <{PositionVector.x},{PositionVector.y}>");
            Console.WriteLine($"Velocity of Occupant1: {Velocity}");
            Console.ReadKey();
        }

    }

}
