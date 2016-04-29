using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using LightControl;
using TreeStructure;

namespace TriangulationTest
{
    [TestFixture]
    public class QuadTreeTests
    {
        Coords startPosition;
        Bounds bound;
        QuadTree QTree;
        [SetUp]
        public void Setup()
        {
            startPosition = new Coords(0, 0);
            bound = new Bounds(startPosition, 100, 100);
            QTree = new QuadTree(bound);
        }

        [Test]
        public void Split_OneSplit_ReturnTrue()
        {
            Assert.AreNotEqual(null, QTree);
        }

        [TearDown]
        public void TearDown()
        {

        }
    }
}