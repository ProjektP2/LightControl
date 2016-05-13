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

namespace LightControlTest
{
    [TestFixture]
    public class DALIControllerTest
    {
        public DALIController Controller;

        [SetUp]
        public void Setup()
        {
            List<LightingUnit> AllLights = new List<LightingUnit>();
            AllLights.Add(new LightingUnit(0, 0, 2));
            AllLights.Add(new LightingUnit(0, 1, 2));
            AllLights.Add(new LightingUnit(0, 2, 2));
            Controller = new DALIController(AllLights);
        }

        [Test]
        public void RemoveUnitFromAllGroupsTest()
        {
            Controller.AddUnitToGroup(Controller.AllLights[0], 0);
            Controller.AddUnitToGroup(Controller.AllLights[0], 1);

            Controller.RemoveUnitFromAllGroups(Controller.AllLights[0]);

            foreach (DALIGroup group in Controller._groups)
            {
                Assert.IsEmpty(group.GroupOfLights);
            }
        }

        [Test]
        public void RemoveUnitFromGroupTest()
        {
            Controller.AddUnitToGroup(Controller.AllLights[0], 0);
            Controller.RemoveUnitFromGroup(Controller.AllLights[0], 0);
            Assert.IsEmpty(Controller._groups[0].GroupOfLights);
        }

        [Test]
        public void AddressGoToSceneTest()
        {
            Controller.AddressGoToScene(Controller.AllLights[0], Controller.scenes[2]);
            //See if the unit was added to groups[16]
            Assert.Contains(Controller.AllLights[0], Controller._groups[16].GroupOfLights);
            //Check the ForcedLightLevel
            Assert.AreEqual(Controller._groups[16].GroupOfLights[0].ForcedLightlevel, Controller.scenes[2] / 100);
        }

        [Test]
        public void AddUnitToGroupTest()
        {
            Controller.AddUnitToGroup(Controller.AllLights[0], 0);
            Assert.Contains(Controller.AllLights[0], Controller._groups[0].GroupOfLights);
        }

        [Test]
        public void ExtinguishGroupTest()
        {
            Controller.AddUnitToGroup(Controller.AllLights[0], 0);
            Controller.Extinguishgroup(0);
            Assert.AreEqual(false, Controller._groups[0].GroupOfLights[0].IsUnitOn);
        }

        [Test]
        public void TurnGroupOnTest()
        {
            Controller.AddUnitToGroup(Controller.AllLights[0], 0);
            Controller.TurnOnGroup(0);
            Assert.AreEqual(true, Controller._groups[0].GroupOfLights[0].IsUnitOn);
        }

        [Test]
        public void GroupGoToSceneTest()
        {
            Controller.AddUnitToGroup(Controller.AllLights[0], 0);
            Controller.AddUnitToGroup(Controller.AllLights[1], 0);
            Controller.AddUnitToGroup(Controller.AllLights[2], 0);
            Controller.GroupGoToScene(0, Controller.scenes[2]);

            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(Controller.scenes[2] / 100, Controller._groups[0].GroupOfLights[i].ForcedLightlevel);
            }
        }

        [Test]
        public void ClearGroupTest()
        {
            Controller.AddUnitToGroup(Controller.AllLights[0], 0);
            Controller.ClearGroup(0);
            Assert.IsEmpty(Controller._groups[0].GroupOfLights);
        }

        [Test]
        public void BroadcastOnAllUnitsTest()
        {
            Controller.BroadcastOnAllUnits();
            for (int i = 0; i < 3; i++)
            {
                Assert.Contains(Controller.AllLights[i], Controller._groups[16].GroupOfLights);
            }
        }

        [Test]
        public void ClearBroadcastGroupTest()
        {
            Controller.BroadcastOnAllUnits();
            Controller.ClearBroadcastGroup();
            Assert.IsEmpty(Controller._groups[16].GroupOfLights);
        }

        [Test]
        public void ClearAllGroupsTest()
        {
            Controller.AddUnitToGroup(Controller.AllLights[0], 0);
            Controller.AddUnitToGroup(Controller.AllLights[1], 1);
            Controller.ClearAllGroups();
            Assert.IsEmpty(Controller._groups[0].GroupOfLights);
            Assert.IsEmpty(Controller._groups[1].GroupOfLights);
        }

        [Test]
        public void IncrementAllLights()
        {
            Controller.AddUnitToGroup(Controller.AllLights[0], 0);
            Controller._groups[0].GroupOfLights[0].LightingLevel = 0.5;
            Controller.AllLights[1].LightingLevel = 0.5;
            Controller.AllLights[2].LightingLevel = 0.5;

            Controller._groups[0].GroupOfLights[0].wantedLightLevel = 1;
            Controller.AllLights[1].wantedLightLevel = 1;
            Controller.AllLights[2].wantedLightLevel = 1;

            Controller.IncrementAllLights();

            //step interval is 0.01

            //Check if the LightingLevel has incremented by the step interval
            Assert.AreEqual(0.5 + 0.01, Controller._groups[0].GroupOfLights[0].LightingLevel);
            Assert.AreEqual(0.5 + 0.01, Controller.AllLights[1].LightingLevel);
            Assert.AreEqual(0.5 + 0.01, Controller.AllLights[2].LightingLevel);

            //Check if the wantedLightLevel is now 0

            Assert.AreEqual(0, Controller._groups[0].GroupOfLights[0].wantedLightLevel);
            Assert.AreEqual(0, Controller.AllLights[1].wantedLightLevel);
            Assert.AreEqual(0, Controller.AllLights[2].wantedLightLevel);
        }

        [Test]
        public void InitGroupTest()
        {
            Controller.InitGroups();
            Assert.Contains(Controller.AllLights[0], Controller.UntouchedLights);
            Assert.Contains(Controller.AllLights[1], Controller.UntouchedLights);
            Assert.Contains(Controller.AllLights[2], Controller.UntouchedLights);
        }

        [Test]
        public void FindUnitWithAddressTest()
        {
            Controller.AllLights[0].Address = 0;
            Assert.AreEqual(Controller.AllLights[0], Controller.FindUnitWithAddress(0));
        }
    }
}
