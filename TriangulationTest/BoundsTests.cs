using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeStructure;
using LightControl;
using NUnit.Framework;

namespace TriangulationTest
{
    [TestFixture]
    public class BoundsTests
    {
        Bounds bound;
        Bounds testBound;
        BoundStub stub;
        [SetUp]
        public void Setup()
        {
            bound = new Bounds(new Coords(0, 0), 100, 100);
            testBound = new Bounds(new Coords(50, 50), 50, 60);
        }
        [TearDown]
        public void Teardown()
        {
            bound = null;
            testBound = null;
        }
        [Test]
        public void Intersection_OneIntersection_ReturnTrue()
        {
            stub = new BoundStub(new Coords(0, 0), new Coords(100, 110));
            testBound.InitializeBoundable(stub);
            Assert.AreEqual(bound.Intersects(testBound), true);
        }
        [Test]
        public void Intersection_ZeroIntersection_Returnfalse()
        {
            stub = new BoundStub(new Coords(0, 0), new Coords(-10, -10));
            testBound.InitializeBoundable(stub);
            Assert.AreEqual(bound.Intersects(testBound), false);
        }
    }

    internal class BoundStub : IBoundable
    {
        Coords TL, BR;
        public BoundStub(Coords topleft, Coords bottomleft)
        {
            TL = topleft;
            BR = bottomleft;
        }
        public void CalculateBoundCoords(Coords Position, out Coords TopLeft, out Coords BottomRight)
        {
            TopLeft = TL;
            BottomRight = BR;
        }
    }
}
