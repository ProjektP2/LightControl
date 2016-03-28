using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightControl;

namespace Triangulering
{
    class VectorMath
    {
        internal Circle Circle
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

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

        //Normalizes a vector by dividing the vector coordinates x and y by the length of the same vector.
        public static Coords NormalizeVector(Coords VectorToNormalize, double LengthOfVector)
        {
            Coords NormalizedVector = new Coords(VectorToNormalize.x / LengthOfVector, VectorToNormalize.y / LengthOfVector);
            return NormalizedVector;
        }

        //Subtracts the coordinates from Vector 2 from Vector 1 and returns the resulting vector.
        public static Coords SubtractVectors(Coords Vector1, Coords Vector2)
        {
            Coords ResultVector = new Coords((Vector1.x - Vector2.x), (Vector1.y - Vector2.y));
            return ResultVector;
        }

        //Calculates and returns the angle in degrees between two vectors Vector1 and Vector2.
        public static double AngleBetweenVectors(Coords Vector1, Coords Vector2)
        {
            return Math.Acos((((Vector1.x * Vector2.x) + (Vector1.y * Vector2.y)) / (LengthOfVector(Vector1) * LengthOfVector(Vector2))));
        }

        //Calculates and returns the dot product of two vectors Vector1 and Vector2.
        public static double DotProduct(Coords Vector1, Coords Vector2)
        {
            return LengthOfVector(Vector1) * LengthOfVector(Vector2) * AngleBetweenVectors(Vector1, Vector2);
        }

        //Scales any given vectors coordinates by a scalar.
        public static Coords ScaleVector(Coords Vector, double Scalar)
        {
            Coords ScaledVector = new Coords(Vector.x * Scalar, Vector.y * Scalar);
            return (ScaledVector);
        }

        //Calculates and returns the perpendicular vector between any given point and any given vector.
        //The mathematical process can be found in section 9.1.2 in the report.
        public static Coords PerpendicularVectorBetweenPointAndVector(Coords Vector, Coords Point, Coords LastKnownPosition)
        {
            Coords NormalizedVector = new Coords();
            Coords PminusA = new Coords(Point.x - LastKnownPosition.x, Point.y - LastKnownPosition.y);
            Coords ComponentP = new Coords();
            double Length = LengthOfVector(Vector);

            NormalizedVector = NormalizeVector(Vector, Length);
            ComponentP = ScaleVector(NormalizedVector, (DotProduct(PminusA, NormalizedVector)));

            return (SubtractVectors(PminusA, ComponentP));
        }
    }
}
