using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeStructure;
using LightControl;
using Triangulering;
using NUnit.Framework;

namespace TriangulationTest
{
    [TestFixture]
    public class QueryTests
    {

        RadiusSearchQuery RQ;
        VectorSearchQuery VQ;
        Bounds mapBound;
        QuadTree QT;
        IMovementVectorProvider DLA;
        Occupant occupant;

        [SetUp]
        public void Setup()
        {
            Coords startPosition = new Coords(0, 0);
            mapBound = new Bounds(startPosition, 100, 100);
            occupant = new Occupant();
            QT = new QuadTree(mapBound);
            RQ = new RadiusSearchQuery(20, mapBound, QT);
        }

        [TearDown]
        public void Teardown()
        {
            RQ = null;
            mapBound = null;
            QT = null;
        }
        
        [Test]
        public void RadiusQuery_ValidBounding_ReturnTrue()
        {
            Coords TL = new Coords();
            Coords BR = new Coords();
            int expectedTopLeft = 0;
            int expectedBottomRight = 0;
            RQ.CalculateBoundCoords(new Coords(0, 0), out TL, out BR);

            Assert.AreEqual(TL, expectedTopLeft);
            Assert.AreEqual(BR, expectedBottomRight);
        }
    }
    internal class MovementVector : IMovementVectorProvider
    {
        public Coords Vector { get; set; }
        public Coords GetMovementVector(Occupant occupant)
        {
            return Vector;
        }
    }
}
