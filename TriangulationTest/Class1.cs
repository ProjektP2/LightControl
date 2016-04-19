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
        Circle r1;
        Circle r2;
        Coords p1;
        Coords p2;
        Triangulation trian;

        [SetUp]
        public void Init()
        {
            r1 = new Circle();
            r2 = new Circle();
            p1 = new Coords(0, 0);
            p2 = new Coords(0, 3);
            trian = new Triangulation(r1,r2);
        }

        [Test]
        public void CalculateDistanceBetweenPointsTest()
        {
            double ExpectedDistance = 3;
            double DistanceBetweenPoints = trian.CalculateDistanceBetweenPoints(p1, p2);
            Assert.AreEqual(DistanceBetweenPoints, ExpectedDistance);
        }
    }
}