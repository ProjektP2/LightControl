using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeStructure;
using LightControl;
using Triangulering;
using NUnit.Framework;

namespace LightControlTest
{
    [TestFixture]
    public class QueryTests
    {

        private RadiusSearchQuery RQ;
        private VectorSearchQuery VQ;
        private QuadTree QT;
        private IMovementVectorProvider DLA;
        private Occupant occupant;

        [SetUp]
        public void Setup()
        {
            QuadTreeCreator QTCreator = new QuadTreeCreator(100, 100);
            QT = QTCreator.Create();

            Creator creator = new RadiusQueryCreator(20, QT);
            RQ = (RadiusSearchQuery)creator.Create();
            occupant = new Occupant();
            DLA = new MovementVector();
            creator = new VectorQueryCreator(DLA, QT, occupant);
            VQ = (VectorSearchQuery)creator.Create();
        }

        [TearDown]
        public void Teardown()
        {
            RQ = null;
            QT = null;
            VQ = null;
            occupant = null;
        }
        
        [Test]
        public void RadiusQuery_ValidBounding_ReturnTrue()
        {
            Coords TL = new Coords();
            Coords BR = new Coords();

            int expectedTopLeftX = -20;
            int expectedTopLeftY = -20;
            int expectedBottomRightX = 20;
            int expectedBottomRightY = 20;

            RQ.CalculateBoundCoords(new Coords(0, 0), out TL, out BR);

            Assert.AreEqual(TL.x, expectedTopLeftX);
            Assert.AreEqual(TL.y, expectedTopLeftY);
            Assert.AreEqual(BR.x, expectedBottomRightX);
            Assert.AreEqual(BR.y, expectedBottomRightY);
        }

        [Test]
        public void VectorQuery_ValidBounding_ReturnTrue()
        {
            Coords TL = new Coords();
            Coords BR = new Coords();

            int expectedTopLeftX = 30;
            int expectedTopLeftY = 70;
            int expectedBottomRightX = -110;
            int expectedBottomRightY = -110;

            MovementVector.vector = new Coords(-1, 1);
            VQ.CalculateBoundCoords(new Coords(50,50), out TL, out BR);
            
            Assert.AreEqual(TL.x, expectedTopLeftX);
            Assert.AreEqual(TL.y, expectedTopLeftY);
            Assert.AreEqual(BR.x, expectedBottomRightX);
            Assert.AreEqual(BR.y, expectedBottomRightY);
        }
    }
    internal class MovementVector : IMovementVectorProvider
    {
        public static Coords vector = new Coords(0,0);
        public Coords GetMovementVector(Occupant occupant)
        {
            return vector;
        }
    }
}
