using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Diagnostics;
using LightControl;
using Triangulering;

namespace LightControlTest
{
    [TestFixture]
    public class TriangulationTest
    {
        Coords[] possitions = new Coords[2];
        Circle r1;
        Circle r2;
        Coords p1;
        Coords p2;
        Triangulation trian;
        Occupant oc;

        [SetUp]
        public void Init()
        {
            possitions[0] = new Coords(-23,10);
            possitions[1] = new Coords(2,10);

            r1 = new Circle(0,0);
            r2 = new Circle(0,60);
            p1 = new Coords(0, 0);
            p2 = new Coords(0, 3);
            trian = new Triangulation(r1,r2);
            oc = new Occupant();
            oc.UpdatePositions(60,60);
        }

        [Test]
        public void CalculateDistanceBetweenPointsTest()
        {
            double ExpectedDistance = 3;
            double DistanceBetweenPoints = trian.CalculateDistanceBetweenPoints(p1, p2);
            Assert.AreEqual(ExpectedDistance, DistanceBetweenPoints);
        }
        [Test]
        public void DetermineSignalStrengthFromCoordsTest()
        {
            trian.DetermineSignalStrengthFromCoords(oc, r1, r2);
            Assert.AreEqual(Math.Sqrt(60 * 60 + 60 * 60), r1.Radius);
            Assert.AreEqual(60, r2.Radius);
        }
        [Test]
        public void DetermineSignalStrengthFromCoordsTest2()
        {
            oc.UpdatePositions(80,60);
            trian.DetermineSignalStrengthFromCoords(oc, r1, r2);
            Assert.AreEqual(Math.Sqrt(80 * 80 + 60 * 60), r1.Radius);
            Assert.AreEqual(80, r2.Radius);
        }
        [Test]
        public void ExcludeImpossiblePositionsTest()
        {
            Coords result = trian.ExcludeImpossiblePositions(oc, possitions);
            Assert.AreEqual(possitions[1].x, result.x);
        }

        public void TriangulatePositionOfSignalSourceTest()
        {
            trian.TriangulatePositionOfSignalSource(oc,r1,r2);
            //Assert.AreEqual(,oc.WiFiPosition1);
        }
        [Test]
        public void FindCircleCircleIntersectionsTest()
        {
            r1.Radius = Math.Sqrt(60 * 60 + 60 * 60);
            r2.Radius = 60;
            Coords[] arrayCoordses = trian.FindCircleCircleIntersections(r1, r2);
            Assert.AreEqual(60, arrayCoordses[0].x);
            Assert.AreEqual(-60, arrayCoordses[1].x);

            //Denne metode skal udbygges så y værdierne også bliver tjekket
            //Assert.AreEqual(60, arrayCoordses[0].y);
            //Assert.AreEqual(60, arrayCoordses[1].y);
        }

    }
}