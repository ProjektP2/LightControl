using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using  System.Diagnostics;
using LightControl;
using NUnit.Framework;
using Triangulering;

namespace TriangulationTest
{
    [TestFixture]
    class DetermineLightsToActivateTest
    {

        Coords[] possitions = new Coords[2];
        Circle r1;
        Circle r2;
        Coords p1;
        Coords p2;
        Triangulation trian;
        DetermineLightsToActivate lightToActivate;
        Occupant oc;
        private List<LightingUnit> list = new List<LightingUnit>();

        [SetUp]
        public void Init()
        {
            
            possitions[0] = new Coords(-23, 10);
            possitions[1] = new Coords(2, 10);
            r1 = new Circle(0, 0);
            r2 = new Circle(0, 60);
            p1 = new Coords(0, 0);
            p2 = new Coords(0, 3);
            trian = new Triangulation(r1, r2);
            lightToActivate = new DetermineLightsToActivate(200, 60, 400, trian);
            oc = new Occupant();
            oc.UpdatePositions(100, 100);
        }

        [Test]
        public void ExistsInCircle()
        {
            List<LightingUnit> Expectet = new List<LightingUnit>();
            Expectet.Add(new LightingUnit(100,100));
            Expectet.Add(new LightingUnit(50,100));

            list.Add(new LightingUnit(100,100));
            list.Add(new LightingUnit(400, 100));
            list.Add(new LightingUnit(50, 100));
            list.Add(new LightingUnit(-200, 100));
            List<LightingUnit> result = lightToActivate.LightsToActivateOnUser(oc, list);
            Assert.AreEqual(Expectet[0].x, result[0].x);
            Assert.AreEqual(Expectet[1].x, result[1].x);
        }
        [Test]
        public void CalculateLightingLevel()
        {
            list.Add(new LightingUnit(100, 100));
            list.Add(new LightingUnit(50, 100));
            List<LightingUnit> result = lightToActivate.LightsToActivateOnUser(oc, list);
            Assert.AreEqual((1.0-(0.0/200.0)) ,result[0].wantedLightLevel);
            Assert.AreEqual((1.0-(50.0/200.0)) ,result[1].wantedLightLevel);
        }
    }
}
