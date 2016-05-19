using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Threading.Tasks;
using NUnit.Framework;
using LightControl;
using TreeStructure;
using Triangulering;
using System.Drawing;

namespace LightControlTest
{
    [TestFixture]
    public class QuadTreeTests
    {
        Point startPosition;
        Rectangle bound;
        QuadTree QTree;
        List<LightingUnit> list;

        [SetUp]
        public void Setup()
        {
            startPosition = new Point(0, 0);
            bound = new Rectangle(startPosition, new Size(100,100));
            QTree = new QuadTree(bound);
            list = new List<LightingUnit>();
        }

        [TearDown]
        public void TearDown()
        {
            QTree = null;
            list = null;
        }

        [Test]
        public void Split_ZeroSplit_ReturnTrue()
        {
            list.Add(new LightingUnit(0, 0, 0));
            QTree.CreateQuadTree(list);
            Assert.AreEqual(QTree.nodes[0], null);
        }
        [Test]
        public void Split_OneSplit_ReturnTrue()
        {
            list.Add(new LightingUnit(0, 0, 0));
            list.Add(new LightingUnit(0, 0, 0));
            list.Add(new LightingUnit(0, 0, 0));
            QTree.CreateQuadTree(list);
            Assert.AreEqual(QTree.nodes[0].Bound.Width, 50);
        }
        [Test]
        public void Split_TwoSplit_ReturnTrue()
        {
            list.Add(new LightingUnit(0, 0, 0));
            list.Add(new LightingUnit(0, 0, 0));
            list.Add(new LightingUnit(0, 0, 0));
            list.Add(new LightingUnit(0, 0, 0));
            list.Add(new LightingUnit(0, 0, 0));
            QTree.CreateQuadTree(list);
            Assert.AreEqual(QTree.nodes[0].nodes[0].Bound.Width, 25);
        }

        [Test]
        public void SplitPosition_AllPositionsFilled_ReturnTrue()
        {
            list.Add(new LightingUnit(0, 0, 0));
            list.Add(new LightingUnit(100, 0, 0));
            list.Add(new LightingUnit(0, 100, 0));
            list.Add(new LightingUnit(100, 100, 0));
            QTree.CreateQuadTree(list);
            Assert.AreNotEqual(QTree.nodes[0], null);
            Assert.AreNotEqual(QTree.nodes[1], null);
            Assert.AreNotEqual(QTree.nodes[2], null);
            Assert.AreNotEqual(QTree.nodes[3], null);
        }
        [Test]
        public void InsertNode_CorrectPositionsFilled_ReturnTrue()
        {
            list.Add(new LightingUnit(0, 0, 0));
            list.Add(new LightingUnit(100, 0, 0));
            list.Add(new LightingUnit(0, 100, 0));
            list.Add(new LightingUnit(100, 100, 0));
            QTree.CreateQuadTree(list);
            Assert.AreEqual(QTree.nodes[0].QuadNodesList[0].LightUnit.x, 0);
            Assert.AreEqual(QTree.nodes[0].QuadNodesList[0].LightUnit.y, 0);
            Assert.AreEqual(QTree.nodes[1].QuadNodesList[0].LightUnit.x, 100);
            Assert.AreEqual(QTree.nodes[1].QuadNodesList[0].LightUnit.y, 0);
            Assert.AreEqual(QTree.nodes[2].QuadNodesList[0].LightUnit.x, 0);
            Assert.AreEqual(QTree.nodes[2].QuadNodesList[0].LightUnit.y, 100);
            Assert.AreEqual(QTree.nodes[3].QuadNodesList[0].LightUnit.x, 100);
            Assert.AreEqual(QTree.nodes[3].QuadNodesList[0].LightUnit.y, 100);
        }
    }
}