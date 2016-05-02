using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using  System.Diagnostics;
using LightControl;
using NUnit.Framework;
using Triangulering;

namespace LightControlTest
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
            Expectet.Add(new LightingUnit(100,100, 240));
            Expectet.Add(new LightingUnit(50,100, 240));

            list.Add(new LightingUnit(100,100, 240));
            list.Add(new LightingUnit(400, 100, 240));
            list.Add(new LightingUnit(50, 100, 240));
            list.Add(new LightingUnit(-200, 100, 240));
            List<LightingUnit> result = lightToActivate.LightsToActivateOnUser(oc, list);
            Assert.AreEqual(Expectet[0].x, result[0].x);
            Assert.AreEqual(Expectet[1].x, result[1].x);
        }
        [Test]
        public void CalculateLightingLevel()
        {
            list.Add(new LightingUnit(100, 100, 240));
            list.Add(new LightingUnit(50, 100, 240));
            List<LightingUnit> result = lightToActivate.LightsToActivateOnUser(oc, list);
            Assert.AreEqual((1.0-(0.0/200.0)) ,result[0].wantedLightLevel);
            Assert.AreEqual((1.0-(50.0/200.0)) ,result[1].wantedLightLevel);
        }
        [Test]
        public void LightsToActivateOnUser_LightingUnitTest()
        {
            oc.UpdatePositions(0, 0);
            List<LightingUnit> expectedList = InitLightingUnitsPositionsForOnUserTest(lightToActivate.Radius);
            List<LightingUnit> actualList = lightToActivate.LightsToActivateOnUser(oc, list);
            Assert.AreEqual(expectedList.Count, actualList.Count);
        }
        [Test]
        public void LightsToActivateOnUser_wantedLightLevelTest()
        {
            oc.UpdatePositions(0,0);
            List<LightingUnit> expectedList = InitLightingUnitWantedLightLevelsForOnUserTest();
            List<LightingUnit> actualList = lightToActivate.LightsToActivateOnUser(oc, list);
            Assert.AreEqual(expectedList[0].wantedLightLevel, actualList[0].wantedLightLevel);
        }

        [Test]
        public void LightsToActivateOnUser_EnsureCorrectLightingLevelTest()
        {
            list.Clear();
            list.Add(new LightingUnit(0,50,0));
            lightToActivate.LightsToActivateOnUser(oc, list);
            Assert.AreEqual(1, list[0].wantedLightLevel);
        }

        private List<LightingUnit> InitLightingUnitsPositionsForOnUserTest(double radius)
        {
            list.Clear();
            //Directly on user
            list.Add(new LightingUnit(0, 1, 0));
            list.Add(new LightingUnit(0, -1, 0));
            list.Add(new LightingUnit(1, 0, 0));
            list.Add(new LightingUnit(-1, 0, 0));
            //Just inside the circle
            list.Add(new LightingUnit(0, radius-1, 0));
            list.Add(new LightingUnit(0, -radius+1, 0));
            list.Add(new LightingUnit(radius - 1,0, 0));
            list.Add(new LightingUnit(-radius + 1, 0, 0));
            //Just outside the circle
            list.Add(new LightingUnit(0, radius, 0));//
            list.Add(new LightingUnit(0, -radius, 0));//
            list.Add(new LightingUnit(radius, 0, 0));
            list.Add(new LightingUnit(-radius, 0, 0));
            //Way out of circle
            list.Add(new LightingUnit(0, radius*2, 0));//
            list.Add(new LightingUnit(0, -radius*2, 0));//
            list.Add(new LightingUnit(radius*2, 0, 0));
            list.Add(new LightingUnit(-radius*2, 0, 0));

            //List containing correct lighting units
            List<LightingUnit> returnlist = new List<LightingUnit>();

            returnlist.Add(new LightingUnit(0, 1, 0));
            returnlist.Add(new LightingUnit(0, -1, 0));
            returnlist.Add(new LightingUnit(1, 0, 0));
            returnlist.Add(new LightingUnit(-1, 0, 0));
            //Just inside the circle
            returnlist.Add(new LightingUnit(0, radius - 1, 0));
            returnlist.Add(new LightingUnit(0, -radius + 1, 0));
            returnlist.Add(new LightingUnit(radius - 1, 0, 0));
            returnlist.Add(new LightingUnit(-radius + 1, 0, 0));

            return returnlist;
        }

        private List<LightingUnit> InitLightingUnitWantedLightLevelsForOnUserTest()
        {
            List <LightingUnit> listToReturn = InitLightingUnitsPositionsForOnUserTest(lightToActivate.Radius);
            //On user
            listToReturn[0].wantedLightLevel = 1 - (1 / lightToActivate.Radius);
            listToReturn[1].wantedLightLevel = 1 - (1 / lightToActivate.Radius);
            listToReturn[2].wantedLightLevel = 1 - (1 / lightToActivate.Radius);
            listToReturn[3].wantedLightLevel = 1 - (1 / lightToActivate.Radius);
            //Just below radius
            listToReturn[4].wantedLightLevel = 1 - (lightToActivate.Radius-1 / lightToActivate.Radius);
            listToReturn[5].wantedLightLevel = 1 - (lightToActivate.Radius - 1 / lightToActivate.Radius);
            listToReturn[6].wantedLightLevel = 1 - (lightToActivate.Radius - 1 / lightToActivate.Radius);
            listToReturn[7].wantedLightLevel = 1 - (lightToActivate.Radius - 1 / lightToActivate.Radius);

            return listToReturn;
        }
    }
}
