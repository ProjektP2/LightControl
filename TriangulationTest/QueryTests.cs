using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeStructure;
using LightControl;
using Triangulering;
using NUnit.Framework;
using System.Drawing;

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
        private StartTreeSearch Start;

        [SetUp]
        public void Setup()
        {
            
            QT = new QuadTree(new Rectangle(0,0, 100,100));
            RQ = new RadiusSearchQuery(50, new Rectangle(0, 0, 100, 100), QT);
            occupant = new Occupant();
            DLA = new MovementVector();
            VQ = new VectorSearchQuery(new Rectangle(0, 0, 100, 100), QT, occupant, DLA);
            Start = new StartTreeSearch(QT);
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
            Start.SearchQuery(new Coords(50, 50), RQ);
            Assert.AreEqual(RQ.Bound.X, 25);
            Assert.AreEqual(RQ.Bound.Y, 25);
            Assert.AreEqual(RQ.Bound.Bottom, 75);
            Assert.AreEqual(RQ.Bound.Top, 25);
        }
        

        [Test]
        public void VectorQuery_SpeedOneRightWalkBounding_ReturnTrue()
        {
            MovementVector.vector = new Coords(1, 0);
            Start.SearchQuery(new Coords(50, 50), VQ);
            Assert.AreEqual(VQ.Bound.X, 50);
            Assert.AreEqual(VQ.Bound.Y, 20);
            Assert.AreEqual(VQ.Bound.Height, 60);
            Assert.AreEqual(VQ.Bound.Width, 60);
        }
        [Test]
        public void VectorQuery_SpeedTwoRightWalkBounding_ReturnTrue()
        {
            MovementVector.vector = new Coords(2, 0);
            Start.SearchQuery(new Coords(50, 50), VQ);
            Assert.AreEqual(VQ.Bound.X, 50);
            Assert.AreEqual(VQ.Bound.Y, 20);
            Assert.AreEqual(VQ.Bound.Height, 60);
            Assert.AreEqual(VQ.Bound.Width, 120);
        }
        [Test]
        public void VectorQuery_SpeedOneLeftWalkBounding_ReturnTrue()
        {
            MovementVector.vector = new Coords(-1, 0);
            Start.SearchQuery(new Coords(100, 50), VQ);
            Assert.AreEqual(VQ.Bound.X, 40);
            Assert.AreEqual(VQ.Bound.Y, 20);
            Assert.AreEqual(VQ.Bound.Height, 60);
            Assert.AreEqual(VQ.Bound.Width, 60);
        }
        [Test]
        public void VectorQuery_SpeedTwoLeftWalkBounding_ReturnTrue()
        {
            MovementVector.vector = new Coords(-2, 0);
            Start.SearchQuery(new Coords(100, 50), VQ);
            Assert.AreEqual(VQ.Bound.X, -20);
            Assert.AreEqual(VQ.Bound.Y, 20);
            Assert.AreEqual(VQ.Bound.Height, 60);
            Assert.AreEqual(VQ.Bound.Width, 120);
        }
        [Test]
        public void VectorQuery_SpeedOneUpWalkBounding_ReturnTrue()
        {
            MovementVector.vector = new Coords(0, 1);
            Start.SearchQuery(new Coords(50, 50), VQ);
            Assert.AreEqual(VQ.Bound.X, 20);
            Assert.AreEqual(VQ.Bound.Y, 50);
            Assert.AreEqual(VQ.Bound.Height, 60);
            Assert.AreEqual(VQ.Bound.Width, 60);
        }
        [Test]
        public void VectorQuery_SpeedOneDownWalkBounding_ReturnTrue()
        {
            MovementVector.vector = new Coords(0, -1);
            Start.SearchQuery(new Coords(50, 50), VQ);
            Assert.AreNotEqual(VQ.Bound.X, 70);
            Assert.AreNotEqual(VQ.Bound.Y, 50);
            Assert.AreEqual(VQ.Bound.Height, 60);
            Assert.AreEqual(VQ.Bound.Width, 60);
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
