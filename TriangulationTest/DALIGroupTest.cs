using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using LightControl;
using TreeStructure;
using Triangulering;
using MapController.Triangulate;

namespace TriangulationTest
{
    [TestFixture]
    class DALIGroupTest
    {
        DALIGroup group;
        LightingUnit unit;
        [SetUp]
        public void Setup()
        {
            group = new DALIGroup();
            unit = new LightingUnit(0, 0, 2);
        }

        [Test]
        public void AddUnitToGroupTest()
        {
            group.AddUnitToGroup(unit);
            Assert.Contains(unit, group.GroupOfLights);
        }

        [Test]
        public void RemoveUnitTest()
        {
            group.AddUnitToGroup(unit);
            group.RemoveUnit(unit);
            Assert.IsEmpty(group.GroupOfLights);
        }

        [Test]
        public void ExtinguishGroupTest()
        {
            group.AddUnitToGroup(unit);
            group.ExtinguishGroup();
            Assert.AreEqual(0, group.GroupOfLights[0].ForcedLightlevel);
        }

        [Test]
        public void TurnOnGroupTest()
        {
            group.AddUnitToGroup(unit);
            group.TurnOnGroup();
            Assert.AreEqual(true, group.GroupOfLights[0].IsUnitOn);
        }

        [Test]
        public void GroupGoToSceneTest()
        {
            group.AddUnitToGroup(unit);
            group.GroupGoToScene(1);
            Assert.AreEqual(1, group.GroupOfLights[0].ForcedLightlevel);
            Assert.AreEqual(true, group.isGroupUsed);
        }

        [Test]
        public void ClearGroupTest()
        {
            group.ClearGroup();
            Assert.IsEmpty(group.GroupOfLights);
            Assert.AreEqual(false, group.isGroupUsed);
        }

        [Test]
        public void IncrementTest()
        {
            double fadeRate = 0.01;
            double stepInterval = 0.01;

            group.AddUnitToGroup(unit);

            //First condition
            group.isGroupUsed = false;
            group.GroupOfLights[0].LightingLevel = 0.5;
            group.GroupOfLights[0].ForcedLightlevel = 0.2;

            group.Increment(fadeRate, stepInterval);
            Assert.AreEqual(0.5, group.GroupOfLights[0].LightingLevel);
            Assert.AreEqual(0.2, group.GroupOfLights[0].ForcedLightlevel);

            //Second condition
            group.isGroupUsed = true;
            group.GroupOfLights[0].IsUnitOn = false;
            group.Increment(fadeRate, stepInterval);
            Assert.AreEqual(0, group.GroupOfLights[0].LightingLevel);

            //Third condition
            group.GroupOfLights[0].LightingLevel = 0.5;
            group.GroupOfLights[0].ForcedLightlevel = 0.2;

            group.GroupOfLights[0].IsUnitOn = true;
            group.Increment(fadeRate, stepInterval);
            Assert.AreEqual(0.5-fadeRate, group.GroupOfLights[0].LightingLevel);

            //Fourth condition
            group.GroupOfLights[0].LightingLevel = 0.2;
            group.GroupOfLights[0].ForcedLightlevel = 0.5;
            group.Increment(fadeRate, stepInterval);
            Assert.AreEqual(0.2+stepInterval, group.GroupOfLights[0].LightingLevel);

            //Fifth condition
            group.GroupOfLights[0].LightingLevel = 0.5;
            group.GroupOfLights[0].ForcedLightlevel = 0.5;
            group.Increment(fadeRate, stepInterval);
            Assert.AreEqual(group.GroupOfLights[0].ForcedLightlevel, group.GroupOfLights[0].LightingLevel);
        }
    }
}
