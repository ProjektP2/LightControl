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
            VQ = new VectorSearchQuery(mapBound, QT, occupant, DLA);
        }

        [TearDown]
        public void Teardown()
        {
            RQ = null;
            mapBound = null;
            QT = null;
            VQ = null;
            occupant = null;
        }
        
        [Test]
        public void RadiusQuery_ValidBounding_ReturnTrue()
        {
            Coords TL = new Coords();
            Coords BR = new Coords();

            int expectedTopLeftX = -20;
            int expectedTopLeftY = -20;
            int expectedBottomRightX = 20;
            int expectedBottomRightY = 20;

            RQ.CalculateBoundCoords(new Coords(0, 0), out TL, out BR);

            Assert.AreEqual(TL.x, expectedTopLeftX);
            Assert.AreEqual(TL.y, expectedTopLeftY);
            Assert.AreEqual(BR.x, expectedBottomRightX);
            Assert.AreEqual(BR.y, expectedBottomRightY);
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
