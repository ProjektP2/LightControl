using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using LightControl;
using Triangulering;

namespace LightControl
{
    //This class is used to simulate an occupant - or rather, the source of a given signal that is received by the access points
    //(instances of the Circle class, in this case.)
    //Contains two positions and an ID. The ID is never set so far. The two coordinates are used by the method CalculateVelocity
    //to calculate the speed of which the signal source is moving between two points.
    class Occupant
    {
        Form _Window;
        Bitmap _Map;
        OccupantMove move;
        public Occupant(Bitmap map, Form window, char key_forward, char key_backwards, char key_left, char key_right)
        {
            _Window = window;
            _Map = map;
            IsPosition1Initialized = false;
            IsPosition2Initialized = false;
            Position2.x = 50;
            Position2.y = 50;
            move = new OccupantMove(_Map, _Window, key_forward, key_backwards, key_left, key_right);
        }

        public Coords LatestPosition()
        {
            if (IsPosition2Initialized)
                return Position2;
            else
                return Position1;
        }

        public bool IsPosition1Initialized { get; private set; }

        public bool IsPosition2Initialized { get; private set; }

        public Coords Position1 = new Coords();
        public Coords Position2 = new Coords();
        public Coords WiFiPosition1 = new Coords();
        public Coords WiFiPosition2 = new Coords();
        public Coords PositionVector = new Coords();
        public double Velocity { get; private set; }
        public string Identity;

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
                Position2 = Coordinates;
                IsPosition2Initialized = true;
            }

            else
            {
                Position1 = Coordinates;
                IsPosition1Initialized = true;
            }
        }

        public void UpdatePositions(double x, double y)
        {
            Coords CoordinatesToUpdateFrom = new Coords(x,y);
            UpdatePositions(CoordinatesToUpdateFrom);
        }
        public void Update()
        {
            Coords ny = new Coords(Position2.x, Position2.y);
            UpdatePositions(move.PlayerMove(ny));
        }
        public void UpdateWifiPosition(Coords Coordinates)
        {

            WiFiPosition1.x = WiFiPosition2.x;
            WiFiPosition1.y = WiFiPosition2.y;
            WiFiPosition2.x = Coordinates.x;
            WiFiPosition2.y = Coordinates.y;
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
                if (Position1 != null && Position2 != null)
                {
                    PositionVector.x = Position2.x - Position1.x;
                    PositionVector.y = Position2.y - Position1.y;
                }
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
