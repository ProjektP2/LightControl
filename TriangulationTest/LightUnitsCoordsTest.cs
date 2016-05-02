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
        public void GetLightUnitCoordsTest()
        {
            lightUnitsCoords.GetLightUnitCoords(list);
            Assert.AreEqual(15, list[0].x);
            Assert.AreEqual(15, list[0].y);
            Assert.AreEqual(30, list[1].x);
            Assert.AreEqual(15, list[1].y);
            Assert.AreEqual(16, list.Count);
        }
    }
}
