using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using NUnit.Framework;
using LightControl;
using Triangulering;

namespace LightControlTest
{
    [TestFixture]
    class LightUnitsCoordsTest
    {
        private LightUnitsCoords lightUnitsCoords;
        private List<LightingUnit> list;
        [SetUp]
        public void Init()
        {
            lightUnitsCoords = new LightUnitsCoords(100,100,10);
            list = new List<LightingUnit>();
        }

        [Test]
        [Ignore("Not Complete")]
        public void GetLightUnitCoordsTest()
        {
            lightUnitsCoords.GetLightUnitCoords(list);
            Assert.AreEqual(15, list[0].x);
            Assert.AreEqual(15, list[0].y);
            Assert.AreEqual(30, list[1].x);
            Assert.AreEqual(15, list[1].y);
            Assert.AreEqual(75, list[9].x);
            Assert.AreEqual(30, list[9].y);
            Assert.AreEqual(15, list[10].x);
            Assert.AreEqual(45, list[10].y);
            Assert.AreEqual(15, list[20].x);
            Assert.AreEqual(75, list[20].y);
        }
    }
}
