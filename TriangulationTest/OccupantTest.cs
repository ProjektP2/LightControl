using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NUnit.Framework;
using LightControl;
using LightControl;


namespace LightControlTest

{
    [TestFixture]
    class OccupantTest
    {
        private Occupant oc;
        [SetUp]
        public void Init()
        {
            oc = new Occupant();
        }
        //Update Test
        [Test]
        public void UpdatePositionsTestPos1()
        {
            oc.UpdatePositions(0,0);
            Assert.AreEqual(0, oc.Position1.x);
            Assert.AreEqual(0, oc.Position1.y);
        }
        [Test]
        public void UpdatePositionsTestPos2()
        {
            oc.UpdatePositions(0, 0);
            oc.UpdatePositions(2, 2);
            Assert.AreEqual(2, oc.Position2.x);
            Assert.AreEqual(2, oc.Position2.y);
            Assert.AreEqual(0, oc.Position1.x);
            Assert.AreEqual(0, oc.Position1.y);
        }
        [Test]
        [Ignore("Virker ikke")]
        public void UpdateTest()
        {
            
        }
        //Update WiFi Test
        [Test]
        public void UpdateWifiPosition1Test()
        {
            Coords newPosition = new Coords(0,0);
            oc.UpdateWifiPosition(newPosition);
            Assert.AreEqual(0, oc.WiFiPosition1.x);
            Assert.AreEqual(0, oc.WiFiPosition1.y);

        }
        [Test]
        public void UpdateWifiPosition2Test()
        {
            Coords newPosition1 = new Coords(0, 0);
            oc.UpdateWifiPosition(newPosition1);
            Coords newPosition2 = new Coords(2, 2);
            oc.UpdateWifiPosition(newPosition2);

            Assert.AreEqual(0, oc.WiFiPosition1.x);
            Assert.AreEqual(0, oc.WiFiPosition1.y);
            Assert.AreEqual(2, oc.WiFiPosition2.x);
            Assert.AreEqual(2, oc.WiFiPosition2.y);
        }
        //Latest Position
        [Test]
        public void LatestPositionTestOnlyPos1 ()
        {
            oc.UpdatePositions(0,0);
            Coords result = oc.LatestPosition();
            Assert.AreEqual(0, result.x);
            Assert.AreEqual(0, result.y);
        }
        [Test]
        public void LatestPositionTestBothPos1AndPos2()
        {
            oc.UpdatePositions(0, 0);
            oc.UpdatePositions(2, 2);
            Coords result = oc.LatestPosition();
            Assert.AreEqual(2, result.x);
            Assert.AreEqual(2, result.y);
        }
        //Is Initialized Test
        [Test]
        public void IsPosition1InitializedTestFailure()
        {
            bool result = oc.IsPosition1Initialized;
            Assert.AreEqual(false, result);
        }
        [Test]
        public void IsPosition1InitializedTestSuccess()
        {
            oc.UpdatePositions(0,0);
            bool result = oc.IsPosition1Initialized;
            Assert.AreEqual(true, result);
        }
        [Test]
        public void IsPosition2InitializedTestFailure()
        {
            oc.UpdatePositions(0, 0);
            bool result = oc.IsPosition2Initialized;
            Assert.AreEqual(false, result);
        }
        [Test]
        public void IsPosition2InitializedTestSuccess()
        {
            oc.UpdatePositions(0, 0);
            oc.UpdatePositions(2, 2);
            bool result = oc.IsPosition2Initialized;
            Assert.AreEqual(true, result);
        }


    }
}
