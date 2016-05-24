using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightControl;

namespace Triangulering
{
    public class VectorMath
    {
        //Calculates the vector between two points StartingPosition (x1,y1) and EndingPosition (x2,y2).
        public static Coords CalculateVector(Coords StartingPosition, Coords EndingPosition)
        {
            Coords Vector = new Coords((EndingPosition.x - StartingPosition.x), (EndingPosition.y - StartingPosition.y));
            return Vector;
        }

        //Calculates the length (magnitude) of a vector. 
        public static double LengthOfVector(Coords Vector)
        {
            return Math.Sqrt(Math.Pow(Vector.x, 2) + Math.Pow(Vector.y, 2));
        }

        //Subtracts the coordinates from Vector 2 from Vector 1 and returns the resulting vector.
        public static Coords SubtractVectors(Coords Vector1, Coords Vector2)
        {
            Coords ResultVector = new Coords((Vector1.x - Vector2.x), (Vector1.y - Vector2.y));
            return ResultVector;
        }


        //Scales any given vectors coordinates by a scalar.
        public static Coords ScaleVector(Coords Vector, double Scalar)
        {
            Coords ScaledVector = new Coords(Vector.x * Scalar, Vector.y * Scalar);
            return (ScaledVector);
        }

        public static Coords projectionLength(Coords Light, Coords ScalledVector, Coords occupant)
        {
            Coords Vector = new Coords();
            Coords LightVector = new Coords();
            LightVector = SubtractVectors(Light,occupant);
            double tal = ((LightVector.x * ScalledVector.x) + (LightVector.y * ScalledVector.y)) / 
                (Math.Pow((Math.Sqrt(Math.Pow(ScalledVector.x, 2)+(Math.Pow(ScalledVector.y,2)))),2));
            Vector.x = tal * ScalledVector.x;
            Vector.y = tal * ScalledVector.y;
            return (Vector);
        }
    }
}
